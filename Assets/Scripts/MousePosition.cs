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
        InputManager.PlayerInput.actions.FindActionMap("Player").Disable();

        StartCoroutine(WaitToEnableLight());
    }
    // Update is called once per frame
    private void Update()
    {
        if (!DialogueManager.Instance.DialogueIsActive())
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(InputManager.Instance.MouseCInput);

            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }

    private IEnumerator WaitToEnableLight(){
        Light2D light = GetComponent<Light2D>();
        light.enabled = false;
        yield return new WaitForSeconds(delayInStart);
        light.enabled = true;
    }
}