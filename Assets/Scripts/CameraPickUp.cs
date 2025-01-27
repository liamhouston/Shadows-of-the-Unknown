using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPickUp : MonoBehaviour
{
    // [SerializeField] private SpriteRenderer cameraCover;
    private int _bedroomPuzzle;
    private int _tookCamera;
    [SerializeField] private GameObject cameraObj;
    public string[] barkList;

    private bool playerIsNearby;
    private bool triggeredCameraDialogue;
    private string isCameraPickedUp = "IsCameraPickedUp";
    
    public Image PuzzlePanel;
    private void Start(){
        triggeredCameraDialogue = false;
        cameraObj.SetActive(true);
        PlayerPrefs.SetInt(isCameraPickedUp, 0);

        // cover the camera if it's already been picked up
        if (PlayerPrefs.HasKey(isCameraPickedUp) && PlayerPrefs.GetInt("BedroomCam") == 1){
            // cameraCover.color = new Color (cameraCover.color.r, cameraCover.color.g, cameraCover.color.b, 1); // cover the camera
            cameraObj.SetActive(false); // turn off tutorial
            // Destroy(cameraObj);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _bedroomPuzzle = PlayerPrefs.GetInt("BedroomPuzzle");
        _tookCamera = PlayerPrefs.GetInt("BedroomCam");
        // if player has clicked through all of dialogue, then start the tutorial scene
        if (triggeredCameraDialogue && !DialogueManager.Instance.DialogueIsActive() && PlayerPrefs.GetInt("IsCameraPickedUp") != 1){
            PlayerPrefs.SetInt(isCameraPickedUp, 1); // camera picked up in player prefs
            // Debug.Log(PlayerPrefs.GetInt(isCameraPickedUp));
            // MusicManager.Instance.PlayMusic("Bedroom");
            LevelManager.Instance.LoadScene("BedroomCam", "CrossFade");
            
        }

        // if player within range and clicks
        if (playerIsNearby && InputManager.Instance.ClickInput && !DialogueManager.Instance.DialogueIsActive()){
            DialogueManager.Instance.playBlockingDialogue("Jay", barkList);
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