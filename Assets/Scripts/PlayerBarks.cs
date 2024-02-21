/* Code Acknowledgements
* From: diving_squid on YouTube 
* URL: https://www.youtube.com/watch?v=1nFNOyCalzo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarks : MonoBehaviour{

    public GameObject dialoguePanel;
    public Text dialogueText;

    public float wordSpeed = 0.04f;
    private bool playerIsNearby;

    public List<string> barkList = new List<string>();
    private int bark_index;

    public float remainOnScreen = 3f;
    public bool repeatBarks = false;

    // Update is called once per frame
    void Update(){
        // if player within range and clicks
        if(playerIsNearby && Input.GetMouseButtonDown(0) && !dialoguePanel.activeInHierarchy && bark_index < barkList.Count){
                // start dialogue
                dialogueText.text = "";
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
                bark_index++;

                // if we want to repeat the barks
                if (bark_index == barkList.Count && repeatBarks){
                    bark_index = 0;
                }
        }
    }

    IEnumerator Typing(){
        // this function types out each individual letter of the dialogue
        foreach (char letter in barkList[bark_index].ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
              
        }
        yield return new WaitForSeconds(remainOnScreen);
        dialoguePanel.SetActive(false);
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