using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomInfo : MonoBehaviour
{   
    private float _startTime;
    private float _elapsedTime;
    string[] _dialogue = {};
    [Header("Puzzle Panel and Note")]
    public GameObject pannel;
    public GameObject note;
    private const string _lastHorizontal = "LastHorizontal";
    // private void Awake()
    // {
    //     PlayerPrefs.SetInt("Bedroom", 0);
    //     PlayerPrefs.SetInt("BedroomCam", 0); // player has camera?
    //     PlayerPrefs.SetInt("Fishdock", 0);
    //     PlayerPrefs.SetInt("Campsite", 0);
    //     PlayerPrefs.SetInt("Store", 0);
    //     PlayerPrefs.SetInt("Darkroom", 0);
    //     PlayerPrefs.SetString("FromScene", "");
        
    //     PlayerPrefs.SetInt("BedroomPuzzle", 0); // player did the puzzle from the trash can?
    //     PlayerPrefs.SetInt("StorePuzzle", 0);   // player took the pic from store?
    //     PlayerPrefs.SetInt("CampsitePuzzle", 0);    // player took the pic from campsite?
    //     PlayerPrefs.SetInt("DarkroomPuzzle", 0);    // player did the puzzle from darkroom?
    //     PlayerPrefs.SetInt("LeaveBedroom", 0);    // player did the puzzle from fishdock?

    //     PlayerPrefs.SetInt("MotelPosterPuzzle", 0); // for testing
    //     PlayerPrefs.SetInt("MotelPoster", 0);
    //     PlayerPrefs.SetInt("PercyCamPuzzle", 0); // for testing
    //     PlayerPrefs.SetInt("PercyCam", 0);
    //     PlayerPrefs.SetInt("FishshopPuzzle", 0); // for testing
    //     PlayerPrefs.SetInt("Fishshop", 0);
    // }
    private void Start()
    {
        _startTime = Time.time; // Store the time when the player enters the campsite
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);
        if (PlayerPrefs.HasKey("FromScene"))
        {
            if (PlayerPrefs.GetString("FromScene") == "BedroomCam")
            {
                this.transform.position = new Vector3(-33.54f, 14.55f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "Fishdock")
            {
                this.transform.position = new Vector3(-68.65f, 14.55f, 0f);
                // TryGetComponent(out Animator _playerAnimator);
                // _playerAnimator.SetFloat(_lastHorizontal, -1);
                _playerAnimator.SetFloat("LastHorizontal", 1);
            }
        }
    }

    private void Update()
    {
        
        int roomPuzzle = PlayerPrefs.GetInt("BedroomPuzzle");
        int camPuzzle = PlayerPrefs.GetInt("BedroomCam");
        if (roomPuzzle != 1 || camPuzzle != 1)
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false && note.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (_elapsedTime > 10) // If more than 10 seconds have passed
                {
                    
                    _dialogue = new string[] {"Why do you take this much time? Bedroom"};
                    print("You have been in the campsite for more than 10 seconds");
                    DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                    _startTime = Time.time;
                }
            }
            else _startTime = Time.time;

        }
        else if (roomPuzzle == 1 && camPuzzle == 1)
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false && note.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (_elapsedTime > 20) // If more than 10 seconds have passed
                {
                    
                    _dialogue = new string[] {"Gotta find Percy. Bedroom"};
                    print("You have been in the campsite for more than 20 seconds");
                    DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                    _startTime = Time.time;
                }
            }
            else _startTime = Time.time;
        }
    }
}
