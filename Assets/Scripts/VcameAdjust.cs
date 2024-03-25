// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Cinemachine;

// public class VcamAdjust : MonoBehaviour
// {
//     [SerializeField]
//     private Transform _focusObjectTransform;
//     // Start is called before the first frame update
//     void Start()
//     {
//         _focusObjectTransform = Player.Instance.transform;
//         TryGetComponent(out CinemachineVirtualCamera _cinemachinevcam);
//         if (_cinemachinevcam)
//         {
//             _cinemachinevcam.Follow = _focusObjectTransform;
//         }
//         else
//         {
//             Debug.Log("_cinemachinevcam does't exist");
//         }
//     }
// }
