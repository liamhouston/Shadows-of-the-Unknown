// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Cinemachine;
// using UnityEditor.VersionControl;
// using UnityEngine.UIElements;
// using Unity.Collections;
// using System.Numerics;
// [RequireComponent(typeof(CinemachineVirtualCamera))]
// public class CinemachinePanAndZoom : MonoBehaviour
// {
//     [SerializeField, Tooltip("Speed at which to pan screen")]
//     private float panSpeed = 2f;
//     [SerializeField, Tooltip("Max fov to zoom in")]
//     private float zoomInMax = 40f;
//     [SerializeField, Tooltip("Max fov to zoom out")]
//     private float zoomOutMax = 90f;
//     [SerializeField, Tooltip("How fast to zoom")]
//     private float zoomSpeed = 3f;
//     [SerializeField]
//     private Collider2D boundary;

//     private CinemachineVirtualCamera vcam;
//     private Transform camTransform;

//     private float startingZposition { get; set; }

//     private void Awake()
//     {
//         vcam = GetComponent<CinemachineVirtualCamera>();
//         camTransform = vcam.VirtualCameraGameObject.transform;
//     }
//     private void Start()
//     {
//         startingZposition = camTransform.position.z;

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // float x
//         // float y
//         // float z
//         // if (x != 0 || y != 0)
//         // {
//         //     PanScreen(x, y);
//         // }
//         // if (z != 0)
//         // {
//         //     ZoomScreen(z);
//         // }
        
//     }

//     public void ZoomScreen(float increment)
//     {
//         float fov = vcam.m_Lens.FieldOfView;
//         float target = Mathf.Clamp((fov + increment), zoomInMax, zoomOutMax);
//         vcam.m_Lens.FieldOfView = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime);
//     }

//     // public void PanDirection(float x, float y)
//     // {
//     //     Vector2 direction = Vector2.Zero;
//     //     Screen.height 
//     // }
//     public void PanScreen(float x, float y)
//     {
//         Vector3 targetPosition =
//     }
// }
