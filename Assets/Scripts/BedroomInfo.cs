using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomInfo : MonoBehaviour
{   
    [Header("Hints after interval")]
    public float delay;
    public string findCameraDialogue;
    public string findPuzzleDialogue;
    public string leaveBedroomDialogue;

    private bool findCameraDialoguePlayed = false;
    private bool findPuzzleDialoguePlayed = false;
    private bool leaveBedroomDialoguePlayed = false;
    private bool timeAdjust = false;


    private float _startTime;
    private float _elapsedTime;

    [Header("Puzzle Panel and Note")]
    public GameObject pannel;
    public GameObject note;
    private const string _lastHorizontal = "LastHorizontal";

    private void Start()
    {
        _startTime = Time.time; // Store the time when the player enters the campsite
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);
        if (PlayerPrefs.HasKey("FromScene"))
        {
            if (PlayerPrefs.GetString("FromScene") == "BedroomCam")
            {
                this.transform.position = new Vector3(-33.54f, 14.30f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "Fishdock")
            {
                this.transform.position = new Vector3(-68.65f, 14.30f, 0f);
                _playerAnimator.SetFloat("LastHorizontal", 1);
            }
        }
    }

    private void Update()
    {
        
        int roomPuzzle = PlayerPrefs.GetInt("BedroomPuzzle");
        int camPuzzle = PlayerPrefs.GetInt("BedroomCam");
        bool DialogueIsActive = DialogueManager.Instance.DialogueIsActive();

        if (!DialogueIsActive && !pannel.activeSelf && !note.activeSelf) timeAdjust = false;

        if (camPuzzle != 1) {
            if (!DialogueIsActive && !pannel.activeSelf && !note.activeSelf) {
                _elapsedTime = Time.time - _startTime;
                if (!findCameraDialoguePlayed && _elapsedTime > delay) {
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {findCameraDialogue});
                    findCameraDialoguePlayed = true;
                    _startTime = Time.time;
                }
            }
            else if (!timeAdjust) StartCoroutine(WaitAndAdd());
        }
        else if (roomPuzzle != 1){
            if (!DialogueIsActive && !pannel.activeSelf && !note.activeSelf) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!findPuzzleDialoguePlayed && _elapsedTime > delay) {
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {findPuzzleDialogue});
                    findPuzzleDialoguePlayed = true;
                    _startTime = Time.time;
                }
            }
            else if (!timeAdjust) StartCoroutine(WaitAndAdd());
        }
        else if (roomPuzzle == 1 && camPuzzle == 1)
        {
            if (!DialogueIsActive && !pannel.activeSelf && !note.activeSelf) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!leaveBedroomDialoguePlayed && _elapsedTime > delay) {
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {leaveBedroomDialogue});
                    leaveBedroomDialoguePlayed = true;
                    _startTime = Time.time;
                }
            }
            else if (!timeAdjust) StartCoroutine(WaitAndAdd());
        }
    }
    private IEnumerator WaitAndAdd()
    {
        _startTime = Time.time + _elapsedTime;
        yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsActive() && !pannel.activeSelf && !note.activeSelf);
        if (!DialogueManager.Instance.DialogueIsActive() && !pannel.activeSelf && !note.activeSelf && !timeAdjust)
        {
            timeAdjust = true;
            _startTime += 5f;
            StopAllCoroutines();
            print("time adjusted");
        }
    }
}
