using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Cinemachine;
using System;

public class FlythroughCameraController : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private Protagonist _playerPrefab = default;
	[SerializeField] private TransformAnchor _playerTransformAnchor = default;
	[SerializeField] private TransformAnchor _gameplayCameraTransform = default;

	//public InputReader inputReader;
	private bool _isRMBPressed;
	private Vector3 _inputVector;
	public Camera mainCamera;

	[NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script

	//public float movementSpeed = 50.0f;
	[SerializeField] [Range(.5f, 3f)] private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings
	public float rotationSpeed = 50;
    public float lerpSpeed = 20.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Vector3 targetPosition;
	private float _previousSpeed;

	void Start()
	{
		_inputReader.EnableCameraControlsInput();
		//Screen.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
		targetPosition = transform.position;
		//_inputReader.MoveEvent += OnXYZMovement;
		//_inputReader.XYZMoveEvent += OnCameraXYZMove;
	}

	private void SpawnPlayer()
	{
		//_playerInstantiatedChannel.RaiseEvent(playerInstance.transform);
		//_playerTransformAnchor.Provide(playerInstance.transform); //the CameraSystem will pick this up to frame the player

		//TODO: Probably move this to the GameManager once it's up and running
		_inputReader.EnableGameplayInput();
	}

	private void OnEnable()
	{
		//Adds listeners for events being triggered in the InputReader script
		//_inputReader.XYZMoveEvent += OnCameraXYZMove;
		//inputReader.EnableMouseControlCameraEvent += OnEnableMouseControlCamera;
		//inputReader.DisableMouseControlCameraEvent += OnDisableMouseControlCamera;
	}

	private void OnDisable()
	{
		//Adds listeners for events being triggered in the InputReader script
		//_inputReader.XYZMoveEvent -= OnCameraXYZMove;
	}

	void Update()
    {
		//_inputReader.XYZMoveEvent += OnCameraXYZMove;
		//Move();
		//Rotate();

		RecalculateMovement();
	}

	private void RecalculateMovement()
	{
		Debug.Log("Hello");
		float targetSpeed;
		Vector3 adjustedMovement;

		if (_gameplayCameraTransform.isSet)
		{
			//Get the three axes from the camera
			Vector3 cameraForward = _gameplayCameraTransform.Value.forward;
			Vector3 cameraRight = _gameplayCameraTransform.Value.right;
			Vector3 cameraUp = _gameplayCameraTransform.Value.up;


			//Use the three axes, modulated by the corresponding inputs, and construct the final vector
			adjustedMovement = cameraRight.normalized * _inputVector.x +
				cameraForward.normalized * _inputVector.y +
				cameraUp.normalized * _inputVector.z;
		}
		else
		{
			//No CameraManager exists in the scene, so the input is just used absolute in world-space
			Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
			adjustedMovement = new Vector3(_inputVector.x, _inputVector.y, _inputVector.z);
		}

		//Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
		if (_inputVector.sqrMagnitude == 0f)
			adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
		
		targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);

		movementInput = adjustedMovement.normalized * targetSpeed;

		_previousSpeed = targetSpeed;

	}

	private void OnXYZMovement(Vector3 movement)
	{

		_inputVector = movement;
	}

	private void Move()
    {
		/*
        targetPosition += transform.forward * movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        targetPosition += transform.right * movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
		*/
    }

    private void Rotate()
    {
		/*
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
		*/
    }

	
	private void OnCameraXYZMove(Vector3 cameraMovement, bool isDeviceMouse)
	{
		Debug.Log("Hello");
		//Using a "fixed delta time" if the device is mouse,
		//since for the mouse we don't have to account for frame duration
		float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime;

		targetPosition += transform.forward * cameraMovement.x * deviceMultiplier * _speedMultiplier;
		targetPosition += transform.right * cameraMovement.y * deviceMultiplier * _speedMultiplier;
		targetPosition += transform.up * cameraMovement.z * deviceMultiplier * _speedMultiplier;
	}

}
