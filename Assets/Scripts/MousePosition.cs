using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    private Vector2 _mouse;

    public float sensitivity;
    public float delayInStart = 0; // how long after the scene starts to enable the light

    private void Start()
    {
        InputManager.PlayerInput.actions.FindActionMap("UI").Disable();
        InputManager.PlayerInput.actions.FindActionMap("Camera").Enable();
        InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap("Camera");
        InputManager.PlayerInput.SwitchCurrentActionMap("Camera");
        Debug.Log("In Start, Mouse input: " + InputManager.Instance.MouseCInput);
        InputManager.PlayerInput.actions.FindActionMap("Player").Disable();

        StartCoroutine(WaitToEnableLight());
    }
    // Update is called once per frame
    private void Update()
    {

        // Debug.Log(InputManager.PlayerInput.currentActionMap);
        if (DialogueManager.Instance.DialogueIsActive())
        {
            // we're in dialogue
            // if (InputManager.Instance.ClickUIInput)
            // {
            //     DialogueManager.Instance.wordSpeed = 0.01f;
            // }
        }

        else
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(InputManager.Instance.MouseCInput);
            // _mouse = _mainCamera.ScreenToWorldPoint(InputManager.Instance.MouseCInput);
            // Debug.Log("In update, Mouse input: " + mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

            // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
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

    private IEnumerator WaitToEnableLight(){
        Light2D light = GetComponent<Light2D>();
        light.enabled = false;
        yield return new WaitForSeconds(delayInStart);
        light.enabled = true;
    }
}