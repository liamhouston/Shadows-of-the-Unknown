using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class FlashlightPosition : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    public float sensitivity;
    public float delayInStart = 0; // how long after the scene starts to enable the light
    private void Start()
    {
        // // InputManager.PlayerInput.actions.FindActionMap("UI").Disable();
        // InputManager.PlayerInput.actions.FindActionMap("Camera").Enable();
        // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap("Camera");
        // InputManager.PlayerInput.SwitchCurrentActionMap("Camera");
        // // InputManager.PlayerInput.actions.FindActionMap("Player").Disable();
        StartCoroutine(WaitToEnableLight());
    }
    // Update is called once per frame
    private void Update()
    {
        // if (!DialogueManager.Instance.DialogueIsActive())
        // {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(InputManager.Instance.MouseInput);
        mousePosition.z = 0; // Ensure the z-position is 0, as we're in a 2D space
        GetComponent<Rigidbody2D>().MovePosition(mousePosition);
        // }
    }

    private IEnumerator WaitToEnableLight()
    {
        Light2D light = GetComponent<Light2D>();
        light.enabled = false;
        yield return new WaitForSeconds(delayInStart);
        light.enabled = true;
    }

}