using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [Header("Clip information")]
    public string soundEffectName;
    public float soundVolume = 1; // between 0-1 representing the percent volume to play at
    public bool looping = false;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PreloadSound(soundEffectName);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.SetVolume(soundVolume);
            StopAllCoroutines();
            StartCoroutine(SoundManager.Instance.FadeOut("Waves", false));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(SoundManager.Instance.FadeOut("Waves", true));
        }
    }
}
