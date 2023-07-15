using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;   //used to set position of mouse

public class CustomCursor : MonoBehaviour
{
    Rigidbody2D selectedRigidBody;
    Vector2 selectedOffset;
    Transform _3dCursor;    //the world position of the virtual cursor
    Transform _virtualCursor;   //the ui cursor
    Transform _operationalCursor;   //the system operational cursor

    private bool isRotating = false;
    //public InteractablesManager interactablesManager;
    private Transform _interactable;

    //Cursor
    public Texture2D _virtualCursorPanTexture;
    public Texture2D _virtualCursorDefaultTexture;
    public GameObject _3dCursorObject;
    public CursorMode _cursorMode;

    public Camera cam;
    bool is3DCursorBound = false;
    public CameraController camController;

    public Image virtualCursor;
    public Sprite virtualCursorDefaultMode;
    public Sprite virtualCursorRotateMode;
    Vector2 _virtualCursorPosLast;
    Vector2 _virtualCursorPos;
    Vector3 _3dCursorPos;
    Vector2 _operationalCursorDelta;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    private void Start()
    {

    }

    public void OSCursorVisible(bool value) { }
    public static void CursorPoint(bool enable) { }

    public void OnOperationalCursorPositionToCenter(InputAction.CallbackContext context)
    {
        CenterOperationalCursorPosition();
    }
    public void CenterOperationalCursorPosition()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Debug.Log("CameraPositionToCenter!");
    }

    public void OnCursorContinueFromCenter(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("CameraContinueFromCenter!");
    }

    public void OnCursorSpecificPosition(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetCursorPos(513, 514);
        Debug.Log("Cursor on specific position!");
    }
    public void MoveCursor(Vector3 delta) {
        
    }

    public void OnVirtualCursorMove(InputAction.CallbackContext context)
    {
        _virtualCursorPos = context.ReadValue<Vector2>();
        _3dCursorObject.transform.position = new Vector3(0, 0, 0);
        //Debug.Log("virtualCursorPos x: " + (int)_virtualCursorPos.x + " virtualCursorPos y: " + (int)_virtualCursorPos.y);
    }

    public void GetMouseDelta(InputAction.CallbackContext context)
    {
        _operationalCursorDelta= context.ReadValue<Vector2>();
        UpdateVirtualCursorMovement();
        RepositionMouseOnScreen();
        Update3dCursorMovement();
    }

    public void UpdateVirtualCursorMovement()
    {
        _virtualCursorPos += _operationalCursorDelta;
        
        virtualCursor.GetComponent<RectTransform>().anchoredPosition = _virtualCursorPos;
        CustomCursorInteraction();
    }

    public void CustomCursorInteraction()
    {
        if (_interactable != null)
        {
            _interactable.GetComponent<Target>().DefaultColor();
        }

        Vector2 customCursorScreenPos = new Vector2(virtualCursor.transform.position.x, virtualCursor.transform.position.y);
        Ray ray = cam.ScreenPointToRay(customCursorScreenPos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject.GetComponent<Target>() != null)
            {
                hitInfo.collider.gameObject.GetComponent<Target>().HighlightColor();
                _interactable = hitInfo.collider.gameObject.transform;
            }
        }
    }

    public void OnRotateToggle(InputAction.CallbackContext context)
    {
        isRotating = context.ReadValue<float>() == 1;
        if (isRotating)
        {
            virtualCursor.GetComponent<Image>().color = Color.white;
            virtualCursor.GetComponent<Image>().sprite = virtualCursorRotateMode;
        }
        else
        {
            virtualCursor.GetComponent<Image>().color = Color.black;
            virtualCursor.GetComponent<Image>().sprite = virtualCursorDefaultMode;
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (isRotating)
        {
            virtualCursor.GetComponent<Image>().color = Color.white;
            virtualCursor.GetComponent<Image>().sprite = virtualCursorRotateMode;
        }
        else
        {
            virtualCursor.GetComponent<Image>().color = Color.black;
            virtualCursor.GetComponent<Image>().sprite = virtualCursorDefaultMode;
        }
    }

    public void RepositionMouseOnScreen()
    {
       if(virtualCursor.GetComponent<RectTransform>().anchoredPosition.x > Screen.width / 2)
        {
            virtualCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width / 2, virtualCursor.GetComponent<RectTransform>().anchoredPosition.y);
            _virtualCursorPos = new Vector2(-Screen.width / 2, virtualCursor.GetComponent<RectTransform>().anchoredPosition.y);
        }
        if (virtualCursor.GetComponent<RectTransform>().anchoredPosition.x < -Screen.width / 2)
        {
            virtualCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width / 2, virtualCursor.GetComponent<RectTransform>().anchoredPosition.y);
            _virtualCursorPos = new Vector2(Screen.width / 2, virtualCursor.GetComponent<RectTransform>().anchoredPosition.y);
        }
        if (virtualCursor.GetComponent<RectTransform>().anchoredPosition.y > Screen.height / 2)
        {
            virtualCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(virtualCursor.GetComponent<RectTransform>().anchoredPosition.x, -Screen.height / 2);
            _virtualCursorPos = new Vector2(virtualCursor.GetComponent<RectTransform>().anchoredPosition.x, -Screen.height / 2);
        }
        if (virtualCursor.GetComponent<RectTransform>().anchoredPosition.y < -Screen.height / 2)
        {
            virtualCursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(virtualCursor.GetComponent<RectTransform>().anchoredPosition.x, Screen.height / 2);
            _virtualCursorPos = new Vector2(virtualCursor.GetComponent<RectTransform>().anchoredPosition.x, Screen.height / 2);
        }
    }

    public void Update3dCursorMovement()
    {
        _3dCursorPos = cam.ScreenToWorldPoint(new Vector3(_virtualCursorPos.x+Screen.width/2, _virtualCursorPos.y + Screen.height/2, cam.nearClipPlane+1));
        _3dCursorObject.transform.position = _3dCursorPos;
    }

    ///<summary>
    ///Cursor follows the rigidbody with offset. To stop following pass null
    ///</summary>
    public void Follow(Rigidbody2D body)
    {
        selectedRigidBody = body;
        if (body)
        {
            selectedOffset = body.position;
            selectedOffset -= new Vector2(_3dCursor.position.x, _3dCursor.position.y);
        }
    }

    /*
    public void SetCursorPosition(Vector3 pos, bool normalized) { }
    public Vector2 GetCursorPositionWorld() { }
    public Vector2 GetCursorPositionScreen(bool normalized) { }
    public Vector2 GetScreenPosition(Vector2 worldPos) { }
    public void SetMode(CursorMode mode) { }
    public void LateUpdate()
    {
        
    }
    

    public void UpdateCursorMovement()
    {
        //Get all input delta from controllers and mouse
        Vector3 delta = GetInputDelta();

        //Bind the OS Cursor to the game window
       // if (BindOSCursor())
       //     mousePosLast = Vector3.zero;

        mousePos += delta;
        Vector2 cursorDelta = Update3dCursor();

        if (selectedRigidBody)
        {
            //Get the rigidbody's position with the offset
            Vector3 bodyPos = selectedRigidBody.position*selectedOffset;
            //bodyPos.z = LevelImproved.CurrentLevel.depth;

            //Grab the scene pixel that the body is at
            Vector3 pos = cam.WorldToScreenPoint(bodyPos);

            UpdateVirtualCursor(pos);
        }
        else
        {
            if (is3DCursorBound)
                Bind3DCursor();

            //Snap our cursor back to the UICursor's position
            if (selectedOffset != Vector2.zero)
            {
                Vector3 pos = cam.WorldToScreenPoint(_virtualCursor.position);
                SetVirtualCursorPosition(pos, false);
                selectedOffset = Vector2.zero;
            }
            else
            {
                UpdateVirtualCursor();
            }
        }
    }


   
    public Vector3 GetInputDelta()
    {
        if (mousePosLast == Vector3.zero)
            mousePosLast = Input.mousePosition;

        //Mouse input
        Vector3 delta = Input.mousePosition - mousePosLast;
        mousePosLast = Input.mousePosition;

        
        ////Controller input
        //float sens = Mathf.Lerp(gamepadSens_High, gamepadSens_Low, InputManager.GetAxis("CursorSpeed"));
        //delta.x += InputManager.GetAxis("Cursor Horizontal") * sens * Time.deltaTime;
        //delta.y += InputManager.GetAxis("Cursor Vertical") * sens * Time.deltaTime;
        //delta.z = 0;

        ////Add overall sensitivity
        //delta *= sensitivity;
        

        return delta;
    }

    ///<summary>
    ///Updates the position of the cursor transform (at the puzzle depth) and also returns the delta movement
    ///</summary>
    public Vector3 UpdateWorldCursor()
    {
        //Get the new cursor position based off of the *mouse* pixel position
        mousePos.z = LevelImproved.current.LevelDepth - cam.transform.position.z;
        Vector3 cursorPos = cam.ScreenToWorldPoint(mousePos);
        cursorPos.z = LevelImproved.current.LevelDepth;

        //Get the delta movement
        Vector3 delta = cursorPos - worldCursor.position;

        //Update the cursor position
        worldCursor.position = cursorPos;

        return delta;
    }

    public void UpdateVirtualCursor(Vector3 screenPos)
    {
        screenPos.z = hudDepth;
        Vector3 cursorPos = cam.ScreenToWorldPoint(screenPos);
        uiCursor.position = cursorPos;
    }

    public void UpdateVirtualCursor()
    {
        mousePos.z = hudDepth;
        Vector3 cursorPos = cam.ScreenToWorldPoint(mousePos);
        uiCursor.positon = cursorPos;
    }
    /*
    public void UpdateReticle()
    {
        reticleTrans.position = cam.transform.TransformPoint(Vector3.forward * hudDepth);
        reticleTrans.rotation = cam.transform.rotation;
    }
*/

}
