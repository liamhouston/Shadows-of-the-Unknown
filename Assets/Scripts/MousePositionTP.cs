using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTP : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(InputManager.Instance.MouseInput);
        mousePosition.z = 0; // Ensure the z-position is 0, as we're in a 2D space
        transform.position = mousePosition;
    }
}
