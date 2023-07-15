using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;   //used to set position of mouse

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

	//Cursor
	public Texture2D _cursorPanTexture;
	public Texture2D _cursorDefaultTexture;
	private Texture2D _cursorTexture;
	public CursorMode _cursorMode;
	public Vector2 _hotSpot = Vector2.zero;
	[DllImport("user32.dll")]
	static extern bool SetCursorPos(int X, int Y);
	public Vector2 _mousePos;
	private int cursorSize = 32;
	private Vector2 _newMousePos;
	private int screenOffset_y = 200;
	private int screenOffset_x = 325;
	public bool buildForWebgl = false;
	public bool hideSystemCursor = false;

	//Pivot Gizmo
	//public Transform pivotGizmo;
	//public Camera gizmoCam;
	//public int pivotGizmoDistance = 10;

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
	private const float InternalMoveSpeed = 8;
	private Vector3 _moveTarget;
	private Vector3 _moveDirection;
	bool isAccelerating = false;

	//Rotation variables
	private bool _rightMouseDown = false;
	private const float InternalRotationSpeed = 4;
	private Quaternion _rotationTarget;
	private Vector2 _mouseDelta;


	void Start()
	{
		Debug.Log("Screen Width : " + Screen.width + " Screen Height: " + Screen.height);
		//int xPos = -Screen.width, yPos = -Screen.height;
		//SetCursorPos(xPos, yPos);//Call this when you want to set the mouse position
		Cursor.lockState = CursorLockMode.Confined;
		if (hideSystemCursor == true)
		{
			Cursor.visible = false;
		}
		else
		{
			Cursor.visible = true;
		}


		Cursor.SetCursor(_cursorDefaultTexture, _hotSpot, _cursorMode);

		//Store a reference to the camera rig
		_actualCamera = GetComponentInChildren<Camera>();

		//Set the rotation of the camera based on the CameraAngle property
		_actualCamera.transform.rotation = Quaternion.AngleAxis(CameraAngle, Vector3.right);

		//Set the position of the camera based on the look offset, angle and default zoom properties. This will make sure we're focusing on the right focal point.
		CurrentZoom = DefaultZoom;
		_actualCamera.transform.position = _cameraPositionTarget;

		//Set the initial rotation value
		_rotationTarget = transform.rotation;
	}

	/// Sets the direction of movement based on the input provided by the player
	public void OnΧΥΖMove(InputAction.CallbackContext context)
	{
		//Read the input value that is being sent by the Input System
		Vector3 value = context.ReadValue<Vector3>();
		_moveDirection = new Vector3(value.x, value.y, value.z);
		_moveTarget += (transform.forward * _moveDirection.z +
						transform.right * _moveDirection.x +
						transform.up * _moveDirection.y) * Time.fixedDeltaTime * InternalMoveTargetSpeed;
	}

	/// Calculates a new position based on various properties

	private void UpdateCameraTarget()
	{
		//_cameraPositionTarget = (Vector3.up * LookOffset) + (Quaternion.AngleAxis(CameraAngle, Vector3.right) * Vector3.back) * _currentZoomAmount;
		//_cameraPositionTarget = _actualCamera.transform.rotation * Vector3.back * _currentZoomAmount;
	}


	/// Sets whether the player has the right mouse button down
	public void OnRotateToggle(InputAction.CallbackContext context)
	{
		_rightMouseDown = context.ReadValue<float>() == 1;
		if (_rightMouseDown)
		{
			Cursor.SetCursor(_cursorPanTexture, _hotSpot, _cursorMode);
		}
		else
		{
			Cursor.SetCursor(_cursorDefaultTexture, _hotSpot, _cursorMode);
		}
	}

	/// Sets the rotation target quaternion if the right mouse button is pushed when the player is moving the mouse
	public void OnRotate(InputAction.CallbackContext context)
	{
		// If the right mouse is down then we'll read the mouse delta value. If it is not, we'll clear it out.
		// Note: Clearing the mouse delta prevents a 'death spin' from occuring if the player flings the mouse really fast in a direction.
		_mouseDelta = _rightMouseDown ? context.ReadValue<Vector2>() : Vector2.zero;

		if (_rightMouseDown)
		{
			Cursor.SetCursor(_cursorPanTexture, _hotSpot, _cursorMode);
		}
		else
		{
			Cursor.SetCursor(_cursorDefaultTexture, _hotSpot, _cursorMode);
		}

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
				break;
			case InputActionPhase.Canceled:
				isAccelerating = false;
				break;
		}
	}

	private void Update()
	{


	}
	private void LateUpdate()
	{

		//Lerp  the camera to a new move target position
		transform.position = Vector3.Lerp(transform.position, _moveTarget, Time.deltaTime * InternalMoveSpeed);

		//Move the _actualCamera's local position based on the new zoom factor
		_actualCamera.transform.localPosition = Vector3.Lerp(_actualCamera.transform.localPosition, _cameraPositionTarget, Time.deltaTime * _internalZoomSpeed);

		//Set the target rotation based on the mouse delta position and our rotation speed
		//Pitch
		transform.rotation *= Quaternion.AngleAxis(_mouseDelta.y * Time.deltaTime * RotationSpeed, Vector3.left);
		//gizmoCam.transform.rotation *= Quaternion.AngleAxis(_mouseDelta.y * Time.deltaTime * RotationSpeed, Vector3.left);

		//Yaw
		transform.rotation = Quaternion.Euler(
			transform.eulerAngles.x,
			transform.eulerAngles.y + _mouseDelta.x * Time.deltaTime * RotationSpeed,
			transform.eulerAngles.z
		);
		/*
		gizmoCam.transform.rotation = Quaternion.Euler(
			transform.eulerAngles.x,
			transform.eulerAngles.y + _mouseDelta.x * Time.deltaTime * RotationSpeed,
			transform.eulerAngles.z
		);
		*/
		//pivotGizmo.position = new Vector3(6, 3, 7);
		//gizmoCam.transform.position = pivotGizmo.position - transform.forward* pivotGizmoDistance;
	}


	private void FixedUpdate()
	{
		//Sets the move target position based on the move direction.
		//Must be done here as there's no logic for the input system to calculate holding down an input
		if (isAccelerating == false)
		{
			InternalMoveTargetSpeed = 8;
			_moveTarget += (transform.forward * _moveDirection.z +
							transform.right * _moveDirection.x +
							transform.up * _moveDirection.y) * Time.fixedDeltaTime * InternalMoveTargetSpeed;
		}
		else
		{
			InternalMoveTargetSpeed = 40;
			_moveTarget += (transform.forward * _moveDirection.z +
							transform.right * _moveDirection.x +
							transform.up * _moveDirection.y) * Time.fixedDeltaTime * InternalMoveTargetSpeed;
		}
	}

	public void OnMouseMove(InputAction.CallbackContext context)
	{
		_mousePos = context.ReadValue<Vector2>();
		//Debug.Log("mousePos x: " + (int)_mousePos.x + " mousePos y: " + (int)_mousePos.y);

		if (_rightMouseDown)
		{
			if (buildForWebgl == false)
			{
				if ((int)_mousePos.x >= Screen.width - 2)
				{
					SetCursorPos(screenOffset_x, Screen.height + screenOffset_y - (int)_mousePos.y);
				}
				if ((int)_mousePos.x <= 1)
				{
					SetCursorPos(Screen.width + screenOffset_x - cursorSize, Screen.height + screenOffset_y - (int)_mousePos.y);
				}
				if ((int)_mousePos.y >= Screen.height - 1)
				{
					SetCursorPos((int)_mousePos.x + screenOffset_x, Screen.height + screenOffset_y - cursorSize);
				}
				if ((int)_mousePos.y <= 1)
				{
					SetCursorPos((int)_mousePos.x + screenOffset_x, screenOffset_y + cursorSize);
				}
			}

		}

		/*
		Ray ray = _actualCamera.ScreenPointToRay(_mousePos);
		if (Physics.Raycast(ray, out RaycastHit hitInfo))
		{

			if (hitInfo.collider.gameObject.GetComponent<Target>() != null)
			{
				Debug.Log("Hit sth!");
				hitInfo.collider.gameObject.GetComponent<Target>().HighlightColor();
			}
			else
			{
			}
		}
		*/
	}

}
