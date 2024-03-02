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
        _movement.Set(InputManager.Instance.MoveCInput.x, InputManager.Instance.MoveCInput.y, 0f);
        Vector3 clampedPosition = transform.position + _movement * _moveSpeed * Time.deltaTime;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, CamBoundary.bounds.min.x, CamBoundary.bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, CamBoundary.bounds.min.y, CamBoundary.bounds.max.y);
        transform.position = clampedPosition;
    }
}