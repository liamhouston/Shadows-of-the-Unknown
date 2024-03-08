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
    }
    void Update()
    {
        if (InputManager.Instance.RightClickInput && readyFlash){
            readyFlash = false;
            StartCoroutine(WaitForFlash());
        }
    }
    
    private IEnumerator WaitForFlash()
    {
        yield return new WaitForSeconds(flashDelay);
        SoundManager.Instance.PlaySound2D("CameraFlash");
        flashLight.transform.position = this.transform.position;
        flashLight.SetActive(true);
        yield return new WaitForSeconds(flashLength);

        flashLight.SetActive(false);
        yield return new WaitForSeconds(afterFlashDelay);

        readyFlash = true;
    }
}
