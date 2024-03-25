using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHotspot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            CursorManager.Instance.SetClickableCursor();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            CursorManager.Instance.SetDefaultCursor();
        }
    }
}
