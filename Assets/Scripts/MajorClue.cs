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
    public Text buttonText;

    public bool playMajorClueSound = false; // this will communicate with CameraFlash when to play the major clue sound
    public bool playerFoundMajorClue = false;

    private Color buttonTextColor;
    private Color invisible = new Color(1,1,1,0);

        // Singleton Instance Property
    public static MajorClue Instance
    {
        get { return instance; }
    }

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


    void Start(){
        // keep exit button hidden until player interacts
        exitButton.interactable = false;
        buttonTextColor = buttonText.color;
        buttonText.color = invisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.RightClickInput && !playerFoundMajorClue){
            playMajorClueSound = true; // communicate to the CameraFlash script that we don't want the default camera shutter noise
            playerFoundMajorClue = true;

            // enable and make visible the exit button
            exitButton.interactable = true;
            buttonText.color = buttonTextColor;
        }
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

    public bool GetPlayerIsNearby(){
        return playerIsNearby;
    }
}
