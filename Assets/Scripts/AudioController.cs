using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance {get{return _instance;}}

    public AudioSource audioSource;
    public AudioClip cameraShutter;

    public AudioClip firstDamageSound;
    public AudioClip defaultDamageSound;

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
    public void PlayFirstDamageSound() {
        audioSource.PlayOneShot(firstDamageSound);
    }
    public void PlayDefaultDamageSound() {
        audioSource.PlayOneShot(defaultDamageSound);
    }
}
