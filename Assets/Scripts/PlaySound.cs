using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private bool playerIsNearby = false;
    private bool currentlyPlaying = false;

    [Header("Clip information")]
    public string soundEffectName;
    public float soundVolume = 1; // between 0-1 representing the percent volume to play at
    public bool looping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && !currentlyPlaying){
            currentlyPlaying = true;

            SoundManager.Instance.SetVolume(soundVolume);

            if (looping){
                SoundManager.Instance.PlayLoopingSound2D(soundEffectName);
            }
            else{
                SoundManager.Instance.PlaySound2D(soundEffectName);
            }
        }
    }
    
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = false;
            currentlyPlaying = false;

            SoundManager.Instance.SetVolume(1);
            SoundManager.Instance.TurnOffSound();
        }    
    }
}
