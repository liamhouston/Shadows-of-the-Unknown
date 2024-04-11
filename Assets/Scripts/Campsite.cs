using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campsite : MonoBehaviour
{
    public GameObject xbutton;
    // private float startTime;
    // string[] CampsiteDialogue = {};
    private void Start()
    {
        // startTime = Time.time; // Store the time when the player enters the campsite
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            TryGetComponent(out Collider2D campsiteCollider);
            campsiteCollider.enabled = false;
            xbutton.SetActive(true);
        }
    }
    // private void Update()
    // {
    //     // float elapsedTime = Time.time - startTime; // Calculate elapsed time

    //     // if (elapsedTime > 10) // If more than 10 seconds have passed
    //     // {
    //     //     CampsiteDialogue = new string[] {"Sheesh, Why do you take this much time?"};
    //     //     print("You have been in the campsite for more than 10 seconds");
    //     //     DialogueManager.Instance.playBlockingDialogue("Jay", CampsiteDialogue);
    //     //     startTime = Time.time;
    //     // }
        
    // }
}
