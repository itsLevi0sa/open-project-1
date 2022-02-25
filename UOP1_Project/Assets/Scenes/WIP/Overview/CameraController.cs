using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	[Header("Configurable Properties")]
	[Tooltip("This is the Y offset of our focal point. 0 Means we're looking at the ground.")]
	public float LookOffset;
	[Tooltip("The angle that we want the camera to be at.")]
	public float CameraAngle;
	[Tooltip("The default amount the player is zoomed into the game world.")]
	public float DefaultZoom;
	[Tooltip("The most a player can zoom in to the game world.")]
	public float ZoomMax;
	[Tooltip("The furthest point a player can zoom back from the game world.")]
	public float ZoomMin;
	[Tooltip("How fast the camera rotates")]
	public float RotationSpeed;

	//Camera specific variables
	private Camera _actualCamera;
	private Vector3 _cameraPositionTarget;

	//Zoom variables
	private float _currentZoomAmount;
	public float CurrentZoom
	{
		get => _currentZoomAmount;
		private set
		{
			_currentZoomAmount = value;
			UpdateCameraTarget();
		}
	}
	private float _internalZoomSpeed = 4;

	//Movement variables
	private float InternalMoveTargetSpeed = 8;
	private const float InternalMoveSpeed = 4;
	private Vector3 _moveTarget;
	private Vector3 _moveDirection;
	bool isAccelerating = false;

	//Rotation variables
	private bool _rightMouseDown = false;
	private const float InternalRotationSpeed = 4;
	private Quaternion _rotationTarget_horizontal;
	private Quaternion _rotationTarget_vertical;
	private Quaternion _rotationTarget;
	private Vector2 _mouseDelta;

	void Start()
	{
		//Have the following 2 lines enabled if you want the cursor to be hidden
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = true;

		//Store a reference to the camera rig
		_actualCamera = GetComponentInChildren<Camera>();

		//Set the rotation of the camera based on the CameraAngle property
		Vector3 v = this.transform.rotation.eulerAngles;
		_actualCamera.transform.rotation = Quaternion.AngleAxis(CameraAngle, Vector3.right+ v);
		//_actualCamera.transform.rotation = this.transform.rotation;

		//Set the position of the camera based on the look offset, angle and default zoom properties. This will make sure we're focusing on the right focal point.
		CurrentZoom = DefaultZoom;
		_actualCamera.transform.position = _cameraPositionTarget;

		//Set the initial rotation value
		_rotationTarget = transform.rotation;	
		_rotationTarget_horizontal = transform.rotation;
		_rotationTarget_vertical = transform.rotation;
	}

	/// Sets the direction of movement based on the input provided by the player
	public void OnΧΥΖMove(InputAction.CallbackContext context)
	{
		//Read the input value that is being sent by the Input System
		Vector3 value = context.ReadValue<Vector3>();

		//Store the value as a Vector3, making sure to move the Y input on the Z axis.
		_moveDirection = new Vector3(value.x, value.y, value.z);
	}

	/// Calculates a new position based on various properties
	private void UpdateCameraTarget()
	{
		_cameraPositionTarget = (Vector3.up * LookOffset) + (Quaternion.AngleAxis(CameraAngle, Vector3.right) * Vector3.back) * _currentZoomAmount;
	}

	/// Sets whether the player has the right mouse button down
	public void OnRotateToggle(InputAction.CallbackContext context)
	{
		_rightMouseDown = context.ReadValue<float>() == 1;
	}

	/// Sets the rotation target quaternion if the right mouse button is pushed when the player is moving the mouse
	public void OnRotate(InputAction.CallbackContext context)
	{
		// If the right mouse is down then we'll read the mouse delta value. If it is not, we'll clear it out.
		// Note: Clearing the mouse delta prevents a 'death spin' from occuring if the player flings the mouse really fast in a direction.
		_mouseDelta = _rightMouseDown ? context.ReadValue<Vector2>() : Vector2.zero;

		
	}

	/// Sets the logic for zooming in and out of the level. Clamped to a min and max value.
	public void OnZoom(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed)
		{
			return;
		}

		// Adjust the current zoom value based on the direction of the scroll - this is clamped to our zoom min/max. 
		CurrentZoom = Mathf.Clamp(_currentZoomAmount - context.ReadValue<Vector2>().y, ZoomMax, ZoomMin);
	}

	public void OnAccelerate(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				isAccelerating = true;
				//Debug.Log("Accelarate!");
				break;
			case InputActionPhase.Canceled:
				isAccelerating = false;
				//Debug.Log("Accelarate cancelled!");
				break;
		}
	}

	private void LateUpdate()
	{
		//Lerp  the camera to a new move target position
		transform.position = Vector3.Lerp(transform.position, _moveTarget, Time.deltaTime * InternalMoveSpeed);

		//Move the _actualCamera's local position based on the new zoom factor
		_actualCamera.transform.localPosition = Vector3.Lerp(_actualCamera.transform.localPosition, _cameraPositionTarget, Time.deltaTime * _internalZoomSpeed);

		//Set the target rotation based on the mouse delta position and our rotation speed
	// Rotation
		// Pitch
		transform.rotation *= Quaternion.AngleAxis(_mouseDelta.y * Time.deltaTime * RotationSpeed, Vector3.left);

		// Paw
		transform.rotation = Quaternion.Euler(
			transform.eulerAngles.x,
			transform.eulerAngles.y + _mouseDelta.x * Time.deltaTime * RotationSpeed,
			transform.eulerAngles.z
		);

		/*
		_rotationTarget_horizontal *= Quaternion.AngleAxis(_mouseDelta.x * Time.deltaTime * RotationSpeed, Vector3.up);
		_rotationTarget_vertical *= Quaternion.AngleAxis(_mouseDelta.y * Time.deltaTime * RotationSpeed, Vector3.left);

		//_rotationTarget = Quaternion.Slerp(_rotationTarget_horizontal, _rotationTarget_vertical, 0.5f);

		//Slerp the camera rig's rotation based on the new target
		//transform.rotation = Quaternion.Slerp(transform.rotation, _rotationTarget_horizontal, Time.deltaTime * InternalRotationSpeed);

		//transform.rotation = Quaternion.AngleAxis(_mouseDelta.x * Time.deltaTime * RotationSpeed, Vector3.up);
		//transform.rotation *= Quaternion.AngleAxis(_mouseDelta.y * Time.deltaTime * RotationSpeed, Vector3.left);
		*/
	}


	private void FixedUpdate()
{
//Sets the move target position based on the move direction. Must be done here as there's no logic for the input system to calculate holding down an input
if (isAccelerating == false)
{
	InternalMoveTargetSpeed = 8;
	_moveTarget += (transform.forward * _moveDirection.z + transform.right * _moveDirection.x + transform.up * _moveDirection.y) * Time.fixedDeltaTime * InternalMoveTargetSpeed;
}
else
{
	InternalMoveTargetSpeed = 38;
	_moveTarget += (transform.forward * _moveDirection.z + transform.right * _moveDirection.x + transform.up * _moveDirection.y) * Time.fixedDeltaTime * InternalMoveTargetSpeed;
}
}

}
