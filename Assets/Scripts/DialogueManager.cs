using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header ("Character Info")]
    public List<string> characterNames;
    public List<Image> characterProfile;

    [Header ("Dialogue Panel Info")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public float wordSpeed = 0.04f;

    private int line_index;
    private string[] dialogue;
    private string defaultActionMap;
    private bool stopTyping = false;
    private float secondsOnScreen;

    private int numClicks = 0;

    private void Awake() {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void Start(){
        dialoguePanel.SetActive(false);
        defaultActionMap = InputManager.PlayerInput.defaultActionMap;
    }

    public void Update(){
        if (InputManager.Instance.ClickInput){
            Debug.Log("Num Clicks " + ++numClicks);
        }
        if (dialoguePanel.activeSelf){
            // check whether user has clicked through to end this line of dialogue
            if (dialogueText.text == dialogue[line_index] && InputManager.Instance.ClickInput){
                // if there are still more lines of dialogue play them
                if (line_index < dialogue.Length - 1){
                    line_index++;
                    dialogueText.text = "";
                    stopTyping = true;
                    StartCoroutine(Typing(secondsOnScreen));
                }
                else{
                    // last line of dialogue
                    dialogueText.text = "";
                    dialoguePanel.SetActive(false);
                    InputManager.PlayerInput.SwitchCurrentActionMap(defaultActionMap);

                }
            }    
        }
    }

    // Play a dialogue line from characterName. Player must click to advance to next dialogue/exit.
    public void playBlockingDialogue(string characterName, string[] dialogueLines){
        stopTyping = true; // no one else should be typing
        if (dialoguePanel.activeSelf){
            dialoguePanel.SetActive(false);
            InputManager.PlayerInput.SwitchCurrentActionMap(defaultActionMap);
            return;
        }

        // disable input system
        InputManager.PlayerInput.SwitchCurrentActionMap("UI");

        // prep for typing
        dialogue = dialogueLines;
        dialogueText.text = "";
        dialoguePanel.SetActive(true);
        line_index = 0;
        secondsOnScreen = -1;
        StartCoroutine(Typing(secondsOnScreen)); // -1 indicates that dialogue should stay on screen until player clicks
    }

    // Play a dialogue line from characterName. Dialogue appears on screen for secondsOnScreen.
    public void playNonBlockingDialogue(string characterName, string[] dialogueLines, float seconds){
        stopTyping = true; // no one else should be typing
        if (dialoguePanel.activeSelf){
            dialoguePanel.SetActive(false);
            InputManager.PlayerInput.SwitchCurrentActionMap(defaultActionMap);
            return;
        }
        // prep for typing
        dialogue = dialogueLines;
        dialogueText.text = "";
        dialoguePanel.SetActive(true);
        line_index = 0;
        secondsOnScreen = seconds;
        StartCoroutine(Typing(secondsOnScreen));
    }

    // Seconds on screen represents how long the dialogue box will appear on screen. (-1 if BLOCKING and should stay on screen until player clicks)
    private IEnumerator Typing(float secondsOnScreen){
        stopTyping = false;
        // this function types out each individual letter of the dialogue
        foreach (char letter in dialogue[line_index].ToCharArray()){
            if (!stopTyping){
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }
        }
        if (secondsOnScreen > 0){
            yield return new WaitForSeconds(secondsOnScreen);
            dialoguePanel.SetActive(false);
        }

    }   

    public bool DialogueCheck(){
        return dialoguePanel.activeSelf;
    }
}