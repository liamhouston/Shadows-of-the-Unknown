using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CursorHotspot : MonoBehaviour
{
    private Light2D _light;

    public float TargetingIntensity;

    private void Start(){

        TryGetComponent(out Light2D light);
        if (light)
        {
            _light = light;
            _light.intensity = 0f;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            CursorManager.Instance.SetClickableCursor();
            if (_light)
            {
                _light.intensity = TargetingIntensity;
            }
            SoundManager.Instance.PlaySound2D("Hover");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            CursorManager.Instance.SetDefaultCursor();
            if (_light)
            {
                _light.intensity = 0f;
            }
        }
        
    }
}
