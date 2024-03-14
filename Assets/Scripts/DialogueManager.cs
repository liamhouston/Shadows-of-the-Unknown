using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.PlayerLoop;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public string[] openingDialogue;

    [Header("Character Info")]
    public List<string> characterNames;
    public List<Image> characterProfile;

    [Header("Dialogue Panel Info")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public float wordSpeed = 0.04f;

    private int line_index;
    private string[] dialogue;
    private string defaultActionMap;
    private bool stopTyping = false;
    private bool _isTyping = false;
    private float secondsOnScreen;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        dialoguePanel.SetActive(false);
        // defaultActionMap = InputManager.PlayerInput.currentActionMap?.name;

        if (!Player.Instance.playedOpeningDialogue() && openingDialogue.Length != 0)
        {
            dialogue = openingDialogue;
            playBlockingDialogue("Jay", openingDialogue);
        }
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            
            if (_isTyping == true && InputManager.Instance.ClickCInput)
            {
                wordSpeed = 0.01f;
            }
            // check whether user has clicked through to end this line of dialogue
            if (dialogueText.text == dialogue[line_index] && InputManager.Instance.ClickCInput)
            {
                // if there are still more lines of dialogue play them

                if (line_index < dialogue.Length - 1)
                {
                    line_index++;
                    dialogueText.text = "";
                    stopTyping = true;
                    StartCoroutine(Typing(secondsOnScreen));
                }
                else
                {
                    // last line of dialogue
                    dialogueText.text = "";
                    dialoguePanel.SetActive(false);
                    // InputManager.PlayerInput.actions.FindActionMap(defaultActionMap).Enable();
                    // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap(defaultActionMap);
                    // InputManager.PlayerInput.SwitchCurrentActionMap(defaultActionMap);
                    // InputManager.PlayerInput.actions.FindActionMap("UI").Disable();
                }
                wordSpeed = 0.04f;
            }
        }
    }

    // Play a dialogue line from characterName. Player must click to advance to next dialogue/exit.
    public void playBlockingDialogue(string characterName, string[] dialogueLines)
    {
        stopTyping = true; // no one else should be typing
        if (dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);
            // InputManager.PlayerInput.actions.FindActionMap("UI").Disable();
            // InputManager.PlayerInput.actions.FindActionMap(defaultActionMap).Enable();
            // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap(defaultActionMap);
            // InputManager.PlayerInput.SwitchCurrentActionMap(defaultActionMap);
            return;
        }

        // disable input system
        // InputManager.PlayerInput.actions.FindActionMap("UI").Enable();
        // InputManager.PlayerInput.actions.FindActionMap("Player").Disable();
        // InputManager.PlayerInput.actions.FindActionMap("Camera").Disable();
        // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap("UI");
        // // InputManager.PlayerInput.actions.FindActionMap("UI").Enable();
        // InputManager.PlayerInput.SwitchCurrentActionMap("UI");


        // prep for typing
        dialogue = dialogueLines;
        dialogueText.text = "";
        dialoguePanel.SetActive(true);
        line_index = 0;
        secondsOnScreen = -1;
        StartCoroutine(Typing(secondsOnScreen)); // -1 indicates that dialogue should stay on screen until player clicks
    }

    // Play a dialogue line from characterName. Dialogue appears on screen for secondsOnScreen.
    public void playNonBlockingDialogue(string characterName, string[] dialogueLines, float seconds)
    {
        stopTyping = true; // no one else should be typing
        if (dialoguePanel.activeSelf)
        {
            dialoguePanel.SetActive(false);


            // InputManager.PlayerInput.actions.FindActionMap("UI").Enable();
            // InputManager.PlayerInput.actions.FindActionMap("Camera").Enable();
            // InputManager.PlayerInput.SwitchCurrentActionMap("Camera");
            // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap(defaultActionMap);
            // InputManager.PlayerInput.SwitchCurrentActionMap(defaultActionMap);
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
    private IEnumerator Typing(float secondsOnScreen)
    {
        yield return new WaitForSeconds(0.2f);
        _isTyping = true;
        stopTyping = false;
        // this function types out each individual letter of the dialogue
        foreach (char letter in dialogue[line_index].ToCharArray())
        {
            if (!stopTyping)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }
        }
        if (secondsOnScreen > 0)
        {
            yield return new WaitForSeconds(secondsOnScreen);
            dialoguePanel.SetActive(false);
        }
        _isTyping = false;
    }

    public bool DialogueIsActive()
    {
        return dialoguePanel.activeSelf;
    }

    // public void DialogueSkip()
    // {
    //     wordSpeed = 0.01f;
    // }
}