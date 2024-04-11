using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class RespondToDarkRoomPuzzle : MonoBehaviour
{
    [Header ("Camera Shake Info")]
    [SerializeField] private float max_amplitude;
    [SerializeField] private float max_frequency;
    [SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera;

    private float num_correct_pieces = 0;
    private bool startedCutscene = false;

    void Update(){
        // if player has solved a new piece. increase shake and play breathing
        if (num_correct_pieces != PuzzleObject.Instance.GetNumCorrectPieces()){
            num_correct_pieces = PuzzleObject.Instance.GetNumCorrectPieces();

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = (num_correct_pieces / PuzzleObject.Instance.GetNumPieces()) * max_amplitude;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = (num_correct_pieces / PuzzleObject.Instance.GetNumPieces()) * max_frequency;

            SoundManager.Instance.PlaySound2D("Heartbeating");
        }

        // if the puzzle is complete and not loaded cutscene yet
        if (!startedCutscene && PuzzleObject.Instance.puzzleComplete){
            startedCutscene = true;

            StartCoroutine(LoadCutsceneAfterSeconds(1));
        }
    }

    private IEnumerator LoadCutsceneAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        // load cutscene
        LevelManager.Instance.LoadScene("EndCutscene", "CrossFade");
    }
    /*
    public string[] puzzleCompletionDialogue;
    private bool puzzleCompletionDialoguePlayed;

    public GameObject puzzlePanel;

    [Header("Game Over Info")]
    public Image BlackoutBox;
    private bool gameOverFadeComplete;
    public string[] gameOverDialogue;
    public int  mainMenuLoadDelay = 7;
    private bool gameOverDialogueStarted;


    void Start(){
        puzzleCompletionDialoguePlayed = false;
        gameOverFadeComplete = false;
        gameOverDialogueStarted = false;
        PlayerPrefs.SetInt("DarkroomPuzzle", 0);
    }

    // Update is called once per frame
    void Update()
    {
        // ----------------- sequence of events
        // puzzle completes
        // play dialogue responding to puzzle
        // fade to black
        // play game over dialogue
        // quit to main menu


        // fade to black if dialogue played
        if (puzzleCompletionDialoguePlayed && !DialogueManager.Instance.DialogueIsActive()){
            StartCoroutine(FadeOutBlackOutSquare((float) 0.3));
        }        
        // // play game over dialogue if faded to black
        if (gameOverFadeComplete && !gameOverDialogueStarted && !DialogueManager.Instance.DialogueIsActive()){
            gameOverDialogueStarted = true;
            DialogueManager.Instance.playBlockingDialogue("Jay", gameOverDialogue);
            StartCoroutine(LoadMainMenuAfterSeconds(mainMenuLoadDelay));
        }
        
        // // play dialogue when puzzle complete
        if (!puzzleCompletionDialoguePlayed && PlayerPrefs.GetInt("DarkroomPuzzle") == 1 && !DialogueManager.Instance.DialogueIsActive() && !puzzlePanel.activeSelf){
            DialogueManager.Instance.playBlockingDialogue("Jay", puzzleCompletionDialogue);
            puzzleCompletionDialoguePlayed = true;
        }

    }

    public IEnumerator FadeOutBlackOutSquare(float fadeSpeed = 1)
    {
        Color objectColor = BlackoutBox.GetComponent<Image>().color;
        float fadeAmount;

        while (BlackoutBox.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = BlackoutBox.GetComponent<Image>().color.a + (fadeSpeed * Time.deltaTime);
            BlackoutBox.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }
        CursorManager.Instance.MouseColliderEnable(false);
        InputManager.PlayerInput.actions.FindAction("Move").Disable();
        gameOverFadeComplete = true;
    }
    
    private IEnumerator LoadMainMenuAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        // load main menu
        LevelManager.Instance.LoadScene("MainMenu", "CrossFade");
    }
    */
}
