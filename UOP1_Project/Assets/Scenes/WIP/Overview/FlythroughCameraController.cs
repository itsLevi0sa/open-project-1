using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Cinemachine;

public class FlythroughCameraController : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	private bool _isRMBPressed;

	//public float movementSpeed = 50.0f;
	[SerializeField] [Range(.5f, 3f)] private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings
	public float rotationSpeed = 50;
    public float lerpSpeed = 20.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Vector3 targetPosition;

    void Start()
	{
		//Screen.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
		targetPosition = transform.position;
    }

	void Update()
    {
		inputReader.CameraMoveEvent += OnCameraMove;
		//Move();
		//Rotate();
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

	
	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		//Using a "fixed delta time" if the device is mouse,
		//since for the mouse we don't have to account for frame duration
		float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime;

		targetPosition += transform.forward * cameraMovement.x * deviceMultiplier * _speedMultiplier;
		targetPosition += transform.right * cameraMovement.y * deviceMultiplier * _speedMultiplier;
	}

}
