using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    // [SerializeField] private CinemachineVirtualCamera cvc;

    // [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _mouse;
    // private Rigidbody2D _rb;

    // public bool ShowCursor = false;
    public float sensitivity;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        // _rb = GetComponent<Rigidbody2D>();
        // if (ShowCursor == false)
        // {

        // }
    }
    // Update is called once per frame
    private void Update()
    {
        if (InputManager.Instance.MenuOpenInput)
        {
            PauseManager.Instance.PauseCheck();
        }
        else
        {

            // _mouse.Set(InputManager.Instance.MouseInput.x, InputManager.Instance.MouseInput.y);
            // Debug.Log(InputManager.Instance.MouseInput);
            _mouse = _mainCamera.ScreenToWorldPoint(InputManager.Instance.MouseInput);
            transform.position = _mouse;

            // // Get the mouse input
            // float mouseX = Input.GetAxis("Mouse X");
            // float mouseY = Input.GetAxis("Mouse Y");

            // // Calculate the movement in world space
            // Vector3 mouseDelta = new Vector3(mouseX, mouseY, 0f) * sensitivity * Time.deltaTime;

            // // Convert the mouse position to world position
            // Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // mouseWorldPosition.z = 0f;

            // // Update the object's position based on the mouse movement
            // Vector3 newPos = mouseWorldPosition + mouseDelta;


            // float aspectRatio = (float) mainCamera.pixelWidth / mainCamera.pixelHeight;    
            // float y_offset = cvc.m_Lens.OrthographicSize;
            // float x_offset = y_offset * aspectRatio;

            // // check that the mouse is within screen bounds
            // bool leftBounds  = newPos.x <= (mainCamera.transform.position.x - x_offset); // true if left of screen
            // bool rightBounds = newPos.x >= (mainCamera.transform.position.x + x_offset); // true if right of screen
            // bool belowBounds = newPos.y <= (mainCamera.transform.position.y - y_offset); // true if below screen
            // bool aboveBounds = newPos.y >= (mainCamera.transform.position.y + y_offset); // true if above screen

            // if (!leftBounds && !rightBounds && !belowBounds && !aboveBounds){ // if in screen, move to new position
            //     transform.position = newPos;
            // }

        }


    }
}