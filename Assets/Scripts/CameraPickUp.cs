using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPickUp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cameraCover;
    [SerializeField] private GameObject cameraObj;
    public string[] barkList;

    private bool playerIsNearby;
    private bool triggeredCameraDialogue;
    private string isCameraPickedUp = "IsCameraPickedUp";

    private void Start(){
        triggeredCameraDialogue = false;

        // cover the camera if it's already been picked up
        if (PlayerPrefs.HasKey(isCameraPickedUp) && PlayerPrefs.GetInt(isCameraPickedUp) == 1){
            cameraCover.color = new Color (cameraCover.color.r, cameraCover.color.g, cameraCover.color.b, 1); // cover the camera
            cameraObj.SetActive(false); // turn off tutorial
        }
        else {
            cameraCover.color = new Color (cameraCover.color.r, cameraCover.color.g, cameraCover.color.b, 0); // uncover the camera
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // if player has clicked through all of dialogue, then start the tutorial scene
        if (triggeredCameraDialogue && !DialogueManager.Instance.DialogueIsActive()){
            PlayerPrefs.SetInt(isCameraPickedUp, 1); // camera picked up in player prefs
            LevelManager.Instance.LoadScene("BedroomCam", "CrossFade");
        }

        // if player within range and clicks
        if (playerIsNearby && InputManager.Instance.ClickInput && !DialogueManager.Instance.DialogueIsActive()){
            DialogueManager.Instance.playBlockingDialogue("Jau", barkList);
            triggeredCameraDialogue = true;
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
}