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

    public AudioClip backgroundShadowMovementSound;

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
    public void PlayBackgroundShadowMovementSound() {
        audioSource.clip = backgroundShadowMovementSound;
        audioSource.time = 1.0f;
        audioSource.Play();
        StartCoroutine(stopSoundAfterSeconds(1.5f));

    }
    IEnumerator stopSoundAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
    }  
}
