using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    public bool ShowCursor = false;
    public float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        if(ShowCursor == false)
        {
            Cursor.visible = false;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PauseManager.Instance.PauseCheck();
        }
        else
        {
            // Get the mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate the movement in world space
            Vector3 mouseDelta = new Vector3(mouseX, mouseY, 0f) * sensitivity * Time.deltaTime;

            // Convert the mouse position to world position
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;

            // Update the object's position based on the mouse movement
            transform.position = mouseWorldPosition + mouseDelta;
        }
        
        
    }
}