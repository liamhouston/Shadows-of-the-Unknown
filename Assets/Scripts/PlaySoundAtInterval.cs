using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtInterval : MonoBehaviour
{
    public int minDelay;
    public int maxDelay;
    public string soundName;

    private bool waitingForSound = false;

    // Update is called once per frame
    void Update()
    {   
        if (!waitingForSound){
            waitingForSound = true;
            System.Random rnd = new System.Random();
            int interval = rnd.Next(minDelay, maxDelay);
            Debug.Log("waiting for " + interval + " before playing " + soundName);
            StartCoroutine(playSoundAtInterval(soundName, interval));
        }
    }

    private IEnumerator playSoundAtInterval(string soundName, int interval){
        yield return new WaitForSeconds(interval);
        SoundManager.Instance.PlaySound2D(soundName);
        waitingForSound = false;
    }
}
