using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MajorClue : MonoBehaviour
{
    private bool playerIsNearby = false;

    public float waitToPlaySound = 2.5f;

    public Button exitButton;
    public Text buttonText;

    private Color buttonTextColor;
    private Color invisible = new Color(1,1,1,0);

    void Start(){
        // keep exit button hidden until player interacts
        exitButton.interactable = false;
        buttonTextColor = buttonText.color;
        buttonText.color = invisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.RightClickInput){
            // player took a picture of the major clue
            StartCoroutine(WaitToPlaySound());

            // enable and make visible the exit button
            exitButton.interactable = true;
            buttonText.color = buttonTextColor;
        }
    }

    IEnumerator WaitToPlaySound() {     
        yield return new WaitForSeconds(waitToPlaySound);
        AudioController.Instance.PlayFoundMajorClueSound();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = false;
        }    
    }
}
