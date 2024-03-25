using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarks : MonoBehaviour
{

    private bool playerIsNearby;
    private int barkIndex; // keep track of what bark we're playing

    public bool playTogether = true; // whether all dialogue should be played at once
    public string[] barkList;

    private void Start(){
        barkIndex = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        // if player within range and clicks
        if (playerIsNearby && InputManager.Instance.ClickInput && !DialogueManager.Instance.DialogueIsActive()){
            if (this.CompareTag("Enemy")){
                // play non blocking on enemy
                string[] element = new string[1];
                element[0] = barkList[barkIndex];
                DialogueManager.Instance.playNonBlockingDialogue("Mr. NPC", element, 0.01f);
                IncrementBarkIndex();
            }    
            else {
                if (playTogether){
                    // if we want to play all dialogue lines at once
                    DialogueManager.Instance.playBlockingDialogue("Mr. NPC", barkList);
                }
                else{
                    string[] element = new string[1];
                    element[0] = barkList[barkIndex];
                    DialogueManager.Instance.playBlockingDialogue("Mr. NPC", element);
                    IncrementBarkIndex();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = false;
        }
    }

    public void ApartmentDialogue()
    {
        DialogueManager.Instance.playBlockingDialogue("Mr. NPC", barkList);
        playerIsNearby = false; // need to come back;
    }

    private void IncrementBarkIndex(){
        barkIndex = (barkIndex + 1) % barkList.Length;
    }

    public void TestFunction(){
        Debug.Log("Please please please");
    }
}