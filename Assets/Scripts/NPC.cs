/* Code Acknowledgements
* From: diving_squid on YouTube 
* URL: https://www.youtube.com/watch?v=1nFNOyCalzo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour{

    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int line_index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsNearby;

    // Update is called once per frame
    void Update(){
        // if player within range and interacts
        if(Input.GetKeyDown(KeyCode.I) && playerIsNearby){
            if (dialoguePanel.activeInHierarchy){
                // clear display if already talking
                zeroText();
            }
            else {
                // otherwise start interaction
                zeroText(); // clear before we start interaction
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        Debug.LogError("This is the current dialogue text:" + dialogueText.text + "This is the full line of dialogue:" + dialogue[line_index]);
        if(dialogueText.text == dialogue[line_index]){
            Debug.LogError("Reached the end of this line of dialogue");
            contButton.SetActive(true);
        }
    }



    public void zeroText(){
        // this function clears the dialoguePanel and resets
        dialogueText.text = "";
        line_index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing(){
        // this function types out each individual letter of the dialogue
        foreach (char letter in dialogue[line_index].ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine(){
        // this function is called by the OnClick for the continueButton
        contButton.SetActive(false);

        if (line_index < dialogue.Length -1){
            line_index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else{
            // clear the display if we reached the last line of dialogue
            zeroText();
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
            zeroText();
        }
    }
}
