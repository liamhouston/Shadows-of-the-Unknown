using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlash : MonoBehaviour
{
    public float flashDelay = 0.2f;
    public float flashLength = 0.1f;
    public float afterFlashDelay = 5.0f;

    private bool readyFlash = true;

    public GameObject flashLight;

    void Start()
    {
        flashLight.SetActive(false);
        SoundManager.Instance.PreloadSound("CameraFlash");
    }
    void Update()
    {
        if (InputManager.Instance.RightClickInput && readyFlash){
            readyFlash = false;
            SoundManager.Instance.PlaySound2D("CameraFlash");
            StartCoroutine(WaitForFlash());
        }
    }
    
    private IEnumerator WaitForFlash()
    {
        flashLight.transform.position = this.transform.position;
        yield return new WaitForSeconds(flashDelay);
        flashLight.SetActive(true);
        yield return new WaitForSeconds(flashLength);

        flashLight.SetActive(false);
        yield return new WaitForSeconds(afterFlashDelay);

        readyFlash = true;
    }
}
