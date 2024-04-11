using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.PlayerLoop;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public string openingDialogueCharacterName = "Jay";
    public string[] openingDialogue;

    [Header("Character Info")]
    public List<string> characterNames;
    public List<Texture2D> characterProfiles;

    [Header("Dialogue Panel Info")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI profileName;
    public RawImage profileImage;
    public float wordSpeed = 0.04f;

    private int line_index;
    private string[] dialogue;
    private bool stopTyping = false;
    private bool _isTyping = false;
    private bool _playingAudio = false;
    private float secondsOnScreen;

    [Header("Talking Clips")]
    public AudioClip[] talkingClips;
    private int clip_index = 0;
    private AudioSource talkingSource;


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
        _playingAudio = false;
        talkingSource = GetComponentInChildren<AudioSource>();

        string currentSceneName = SceneManager.GetActiveScene().name;

        // // play opening dialogue if this is our first time in the scene
        if (openingDialogue.Length != 0 && PlayerPrefs.GetInt(currentSceneName) == 0)
        {
            playBlockingDialogue(openingDialogueCharacterName, openingDialogue);
        }
    }

    private void Update()
    {
        // play talking sound if typing
        if (talkingClips.Length != 0){
            if (_isTyping && !_playingAudio){
                _playingAudio = true;
                talkingSource.clip = talkingClips[clip_index];
                clip_index = (clip_index + 1) % talkingClips.Length;
                talkingSource.loop = true;
                talkingSource.Play();
            }
            // stop audio when not typing
            if (!_isTyping && _playingAudio){
                _playingAudio = false;
                talkingSource.Stop();
            }
        }

        if (dialoguePanel.activeSelf)
        {
            if (_isTyping == true && InputManager.Instance.ClickInput)
            {
                wordSpeed = 0.01f;
            }
            // check whether user has clicked through to end this line of dialogue
            if (dialogueText.text == dialogue[line_index] && InputManager.Instance.ClickInput)
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
                    CursorManager.Instance.MouseColliderSwitch();
                    InputManager.PlayerInput.actions.FindAction("Move").Enable();
                    // InputManager.PlayerInput.actions.FindAction("Point").Enable();
                    InputManager.PlayerInput.actions.FindAction("RightClick").Enable();
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
            return;
        }
        SetCharacterPanel(characterName);

        // prep for typing
        dialogue = dialogueLines;
        dialogueText.text = "";
        dialoguePanel.SetActive(true);
        CursorManager.Instance.MouseColliderSwitch();
        InputManager.PlayerInput.actions.FindAction("Move").Disable();
        InputManager.PlayerInput.actions.FindAction("RightClick").Disable();
        // InputManager.PlayerInput.actions.FindAction("RightClick").Disable();
        // InputManager.PlayerInput.actions.FindAction("Point").Disable();
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
            return;
        }
        SetCharacterPanel(characterName);

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

    public void DisableDialoguePanel(){
        dialoguePanel.SetActive(false);
    }

    public void SetCharacterPanel(string characterName){
        // if in first person don't try to change anything
        if (profileImage == null || profileName == null){
            return;
        }

        // if specified invalid name, assume it was supposed to be jay
        if (!characterNames.Contains(characterName)){
            characterName = "Jay";
        }
        // set character name
        profileName.text = characterName;

        // set character profile
        int index = characterNames.IndexOf(characterName);
        if (characterProfiles[index] == null){
            // name does not have associated texture
            profileImage.color = new Color(profileImage.color.r, profileImage.color.g, profileImage.color.b, 0);
        }
        else {
            // name does have associated texture
            profileImage.color = new Color(profileImage.color.r, profileImage.color.g, profileImage.color.b, 1);
            profileImage.texture = characterProfiles[index];
        }
        
    }
}
