/* Code Acknowledgements
* From: diving_squid on YouTube 
* URL: https://www.youtube.com/watch?v=1nFNOyCalzo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{

    public string[] dialogue;

    public bool playerIsNearby;
    // Update is called once per frame
    void Update()
    {
        // if player within range and interacts
        if (InputManager.Instance.ClickInput && playerIsNearby)
        {
            DialogueManager.Instance.playBlockingDialogue("Mr. NPC", dialogue);
            playerIsNearby = false; // need to come back if they want to talk to npc again
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
        }
    }
}