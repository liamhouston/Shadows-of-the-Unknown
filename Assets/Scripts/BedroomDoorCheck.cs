using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomDoorCheck : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("Bedroom") == 0)
        {
            TryGetComponent(out Collider2D doorCollider);
            TryGetComponent(out PlayerBarks playerBarks);
            doorCollider.enabled = true;
            playerBarks.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryGetComponent(out PlayerBarks playerBarks);
            TryGetComponent(out NextScene nextScene);
            if (PlayerPrefs.GetInt("BedroomCam") == 0)
            {
                nextScene.enabled = false;
                playerBarks.enabled = true;
                playerBarks.barkList = new string[] { "I don’t really have a reason to go outside right now.",  "I’ll keep looking around his gross place." };
                DialogueManager.Instance.playBlockingDialogue("Jay", playerBarks.barkList); 
            }
            else if (PlayerPrefs.GetInt("BedroomCam") == 1)
            {
                nextScene.enabled = true;
                playerBarks.enabled = false;
            }
        }
    }
}
