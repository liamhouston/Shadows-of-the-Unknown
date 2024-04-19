using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilmPickUp : MonoBehaviour
{
    public Button exitButton;
    public GameObject film;
    public string[] pickUpDialogue;

    public int delay;
    public string hintDialogue;
    private bool hintDialoguePlayed = false;
    private float _startTime;
    private float _elapsedTime;

    private bool playerIsNearby = false;
    public bool playerFoundMajorClue = false;
    string[] BedroomCamdialogue = {};

    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time; // Store the time when the player enters the campsite
        exitButton.gameObject.SetActive(false);
        InputManager.PlayerInput.actions.FindAction("RightClick").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // play hint dialogue
        if (!playerFoundMajorClue)
        {
            if (!DialogueManager.Instance.DialogueIsActive()) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!hintDialoguePlayed && _elapsedTime > delay) {
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {hintDialogue});
                    hintDialoguePlayed = true;
                    _startTime = Time.time;
                }
            }
        }

        // pick up film
        if (playerIsNearby && InputManager.Instance.ClickInput && !playerFoundMajorClue)
        {
            playerFoundMajorClue = true;
            exitButton.gameObject.SetActive(true);
            exitButton.interactable = true; 
            film.SetActive(false); // make film invisible
            InputManager.PlayerInput.actions.FindAction("RightClick").Enable();
            SoundManager.Instance.PlaySound2D("MajorClue");
            
            DialogueManager.Instance.playBlockingDialogue("Jay", pickUpDialogue);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = false;
        }
    }
}
