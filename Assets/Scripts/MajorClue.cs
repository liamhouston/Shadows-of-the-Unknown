using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MajorClue : MonoBehaviour
{
    private static MajorClue instance; // Singleton instance 

    private bool playerIsNearby = false;

    public float waitToPlaySound = 2.5f;

    public Button exitButton;

    public bool playMajorClueSound = false; // this will communicate with CameraFlash when to play the major clue sound
    public bool playerFoundMajorClue = false;


    // Singleton Instance Property
    public static MajorClue Instance
    {
        get { return instance; }
    }

    public string[] dialogue;
    private PlayerBarks playerBarks;

    void Awake()
    {
        // Ensure only one instance of MajorClue exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        // keep exit button hidden until player interacts
        exitButton.gameObject.SetActive(false);
        playerBarks = GetComponent<PlayerBarks>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.RightClickInput && !playerFoundMajorClue)
        {
            playMajorClueSound = true; // communicate to the CameraFlash script that we don't want the default camera shutter noise
            playerFoundMajorClue = true;
            // Player.Instance.TentPic = true;
            exitButton.gameObject.SetActive(true);
            exitButton.interactable = true;
            // dialogue = playerBarks.barkList;
            // InputManager.PlayerInput.actions.FindActionMap("UI").Enable();
            // InputManager.PlayerInput.actions.FindActionMap("Camera").Enable();
            dialogue = new string[] { "I think I got the picture. I might as well leave." };
            DialogueManager.Instance.playBlockingDialogue("", dialogue);

            // Fade In and Out From Black quickly
            // StartCoroutine(GameController.Instance.FadeToAndFromBlack((float)0.5, (float)0, (float)0.1));

            // enable and make visible the exit button

            // buttonText.color = buttonTextColor;
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

    public bool GetPlayerIsNearby()
    {
        return playerIsNearby;
    }
}
