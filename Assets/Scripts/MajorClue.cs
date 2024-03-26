using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MajorClue : MonoBehaviour
{
    private static MajorClue instance; // Singleton instance 

    private bool playerIsNearby = false;

    public Button exitButton;

    public bool playerFoundMajorClue;

    public string[] dialogue;


    // Singleton Instance Property
    public static MajorClue Instance
    {
        get { return instance; }
    }

    void Start(){
        exitButton.gameObject.SetActive(false);
        playerFoundMajorClue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.RightClickInput && !playerFoundMajorClue)
        {
            playerFoundMajorClue = true;

            exitButton.gameObject.SetActive(true);
            string fromScene = SceneManager.GetActiveScene().name + "Puzzle";
            PlayerPrefs.SetInt(fromScene, 1);
            exitButton.interactable = true;

            if (dialogue.Length != 0){
                DialogueManager.Instance.playBlockingDialogue("", dialogue);
            }
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

    public bool GetPlayerIsNearby()
    {
        return playerIsNearby;
    }
}
