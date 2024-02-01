﻿using System.Collections;
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
        if (Input.GetMouseButtonDown(1) && readyFlash)
        {
            readyFlash = false;
            StartCoroutine(WaitForFlash());
        }
    }
    
    // void TriggerCameraShutter()
    // {
    //     if (Input.GetMouseButtonDown(1) && !flashLight.activeSelf)
    //     {
    //         StartCoroutine(WaitForFlash());
    //     }
    // }
    private IEnumerator WaitForFlash()
    {
        yield return new WaitForSeconds(flashDelay);

        AudioController.Instance.PlayCameraShutter();
        flashLight.SetActive(true);
        yield return new WaitForSeconds(flashLength);
        flashLight.SetActive(false);
        yield return new WaitForSeconds(afterFlashDelay);
        Debug.Log("5seconds");
        readyFlash = true;
    }
}
