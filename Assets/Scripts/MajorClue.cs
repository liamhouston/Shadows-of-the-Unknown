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
    private Color invisible = new Color(1, 1, 1, 0);

    void Start()
    {
        // keep exit button hidden until player interacts
        exitButton.gameObject.SetActive(false);
        // exitButton.interactable = false;
        // buttonTextColor = buttonText.color;
        // buttonText.color = invisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.RightClickInput)
        {
            // player took a picture of the major clue
            StartCoroutine(WaitToPlaySound());
            InputManager.PlayerInput.actions.FindActionMap("UI").Enable();
            // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap("Camera");
            // InputManager.PlayerInput.SwitchCurrentActionMap("Camera");
            // enable and make visible the exit button
            exitButton.interactable = true;
            exitButton.gameObject.SetActive(true);
        }
    }

    IEnumerator WaitToPlaySound()
    {
        yield return new WaitForSeconds(waitToPlaySound);
        SoundManager.Instance.PlaySound2D("ScaryAmbientWind");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
        }
    }
}
