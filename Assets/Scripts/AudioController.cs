using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance {get{return _instance;}}

    public AudioSource audioSource;
    public AudioClip cameraShutter;

    private AudioController audioController;


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    public void PlayCameraShutter()
    {
        audioSource.PlayOneShot(cameraShutter);
    }
}
