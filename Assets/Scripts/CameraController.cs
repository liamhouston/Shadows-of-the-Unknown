using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.InputSystem.Controls;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [Header("Camera Confinement")]
    // public Transform backgroundTransform;
    // public Transform camTransform;

   
    public Collider2D CamBoundary;
    // public Camera mainCamera;

    public CinemachineVirtualCamera vcam;
    private Vector3 _movement;

    // public void Awake()
    // {

    //     // cvc = GetComponentInChildren<CinemachineVirtualCamera>();
    //     // Debug.Assert(cvc != null, "Must have Cinemachine Virtual Camera component alongside Camera Controller object.");
    //     // Debug.Assert(mainCamera != null, "Must have main camera attached.");
    // }


    public void Update()
    {
        // _movement.Set(InputManager.Instance.MoveInput.x, InputManager.Instance.MoveInput.y);
        _movement.Set(InputManager.Instance.MoveInput.x, InputManager.Instance.MoveInput.y, 0f);
        Vector3 clampedPosition = transform.position + _movement * _moveSpeed * Time.deltaTime;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, CamBoundary.bounds.min.x, CamBoundary.bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, CamBoundary.bounds.min.y, CamBoundary.bounds.max.y);
        transform.position = clampedPosition;
        // Built in Unity input system uses arrows or wasd
        // float horizontalInput = Input.GetAxis("Horizontal"); // ranges from -1 left to 1 right
        // float verticalInput = Input.GetAxis("Vertical"); // ranges from -1 down to 1 up

        // Vector3 horzMovement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;
        // Vector3 vertMovement = new Vector3(0f, verticalInput, 0f) * speed * Time.deltaTime;

        // if not at screen bound, move horizontal
        // if (IsValid(transform.position + horzMovement))
        // {
        //     transform.position = transform.position + horzMovement;
        // }
        // // if not at screen bound, move vertical
        // if (IsValid(transform.position + vertMovement))
        // {
        //     transform.position = transform.position + vertMovement;
        // }
    }

    // Given a position, checks that this position is valid in the bounds of the camera
    // public bool IsValid(Vector3 pos)
    // {
    //     float horz_size = CamBoundary.bounds.size.x;
    //     float vert_size = CamBoundary.bounds.size.y;

    //     float left_bound = CamBoundary.position.x - (horz_size / 2.0f);
    //     float right_bound = CamBoundary.position.x + (horz_size / 2.0f);
    //     float top_bound = CamBoundary.position.y + (vert_size / 2.0f);
    //     float bottom_bound = CamBoundary.position.y - (vert_size / 2.0f);

    //     // We need to consider the size of the virtual camera when determining if a position is valid

    //     // Orthographic size determines the amount of units from the center of the screen to the top. For example, the virtual camera has an ortho size of 4 which means you can see a total of 8 units in the y direction.
    //     float aspectRatio = (float)mainCamera.pixelWidth / mainCamera.pixelHeight;

    //     float y_offset = cvc.m_Lens.OrthographicSize;
    //     float x_offset = y_offset * aspectRatio;

    //     bool validity = true;
    //     validity &= ((pos.x - x_offset) >= left_bound); // validity stays true if we can't see past left bound
    //     validity &= ((pos.x + x_offset) <= right_bound); // can't see past right bound
    //     validity &= ((pos.y - y_offset) >= bottom_bound); // can't see below bottom
    //     validity &= ((pos.y + y_offset) <= top_bound); // can't see above

    //     return validity;
    // }
}