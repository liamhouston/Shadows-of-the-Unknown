using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilmPickUp : MonoBehaviour
{
    public Button exitButton;
    public GameObject film;
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
        if (!playerFoundMajorClue)
        {
            if (!DialogueManager.Instance.DialogueIsActive()) 
            {
                _elapsedTime = Time.time - _startTime;
                if (_elapsedTime > 10) // If more than 10 seconds have passed
                {
                    
                    BedroomCamdialogue = new string[] {"Why do you take this much time? BedroomCam"};
                    print("You have been in the campsite for more than 10 seconds");
                    DialogueManager.Instance.playBlockingDialogue("Jay", BedroomCamdialogue);
                    _startTime = Time.time;
                }
            }
        }
        if (playerIsNearby && InputManager.Instance.ClickInput && !playerFoundMajorClue)
        {
            playerFoundMajorClue = true;
            exitButton.gameObject.SetActive(true);
            exitButton.interactable = true; 
            film.SetActive(false); // make film invisible
            InputManager.PlayerInput.actions.FindAction("RightClick").Enable();
            SoundManager.Instance.PlaySound2D("MajorClue");
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
