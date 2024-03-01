/* Code Acknowledgements
* From: diving_squid on YouTube 
* URL: https://www.youtube.com/watch?v=1nFNOyCalzo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour{

    public string[] dialogue;

    public bool playerIsNearby;
    // Update is called once per frame
    void Update(){
        // if player within range and interacts
        if(InputManager.Instance.InteractInput && playerIsNearby){
            DialogueManager.Instance.playBlockingDialogue("Mr. NPC", dialogue);   
        }      
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            playerIsNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            playerIsNearby = false;
        }
    }
}