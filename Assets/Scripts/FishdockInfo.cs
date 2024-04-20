using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class Fishdockinfo : MonoBehaviour
{
    private const string _lastHorizontal = "LastHorizontal";

    [Header("Hints after interval")]
    public float delay;
    public string missingMotelPoster;
    public string missingStore;
    public string missingCampsite;
    public string missingFishshop;
    public string missingPercyCam;
    public string missingDarkRoom;
    public string toDarkroom;
    
    [Header("Puzzle Objects")]
    public GameObject MotelPoster;
    public GameObject PercyCam;
    public GameObject Fishshop;
    public GameObject Campsite;
    public GameObject Store;
    public GameObject Bedroom;

    // public GameObject pannel;
    public GameObject sign;
    private float _startTime;
    private float _elapsedTime;
    private bool timeAdjust = false;
    string[] _dialogue = {};

    private int puzzle1;
    private int puzzle2;
    private int puzzle3;
    private int puzzle4;
    private int puzzle5;
    private int puzzle6;

    private void Start()
    {
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);

        puzzle1 = PlayerPrefs.GetInt("BedroomPuzzle");
        puzzle2 = PlayerPrefs.GetInt("StorePuzzle");
        puzzle3 = PlayerPrefs.GetInt("CampsitePuzzle");
        puzzle4 = PlayerPrefs.GetInt("FishshopPuzzle");
        puzzle5 = PlayerPrefs.GetInt("PercyCamPuzzle");
        puzzle6 = PlayerPrefs.GetInt("MotelPosterPuzzle");

        _startTime = Time.time;
        // Depending on the scene the player is coming from, set the player's position
        if (PlayerPrefs.HasKey("FromScene"))
        {
            if (PlayerPrefs.GetString("FromScene") == "Store")
            {
                this.transform.position = new Vector3(40.32f, -6.4f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "Campsite")
            {
                this.transform.position = new Vector3(-59.46f, -6.4f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "MotelPoster")
            {
                this.transform.position = new Vector3(118.6f, -6.4f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "PercyCam")
            {
                _playerAnimator.SetFloat(_lastHorizontal, 1);
                this.transform.position = new Vector3(-129f, -6.4f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "Fishshop")
            {
                this.transform.position = new Vector3(-129f, -6.4f, 0f);
            }
        }
        // Disable colliders if the player has already solved the puzzle from extra scenes
        if (PlayerPrefs.GetInt("MotelPosterPuzzle") == 1)
        {
            MotelPoster.GetComponent<Collider2D>().enabled = false;
        }
        if (PlayerPrefs.GetInt("PercyCamPuzzle") == 1)
        {
            PercyCam.GetComponent<Collider2D>().enabled = false;
        }
        if (PlayerPrefs.GetInt("FishshopPuzzle") == 1)
        {
            Fishshop.GetComponent<Collider2D>().enabled = false;
        }

        // Change the color of the lights if the player has already solved the puzzle from old scenes
        if (PlayerPrefs.GetInt("StorePuzzle") == 1)
        {
            Store.GetComponent<Light2D>().color = new Color(255f/255f, 255f/255f, 255f/255f);
        }
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            Campsite.GetComponent<Light2D>().color = new Color(255f/255f, 255f/255f, 255f/255f);
        }
        if (puzzle1 == 1 && puzzle2 == 1 && puzzle3 == 1 && puzzle4 == 1 && puzzle5 == 1 && puzzle6 == 1)
        {
            _dialogue = new string[] {toDarkroom};
            
            Bedroom.SetActive(false);
            MotelPoster.SetActive(false);
            PercyCam.SetActive(false);
            Fishshop.SetActive(false);
            Campsite.SetActive(false);
            Store.SetActive(false);
            DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
            DialogueManager.Instance.dialoguePanel.SetActive(true);
        }
    }
    private void Update()
    {
        bool DialogueIsActive = DialogueManager.Instance.DialogueIsActive();
        if (!DialogueIsActive && !sign.activeSelf) timeAdjust = false;

        if (puzzle1 == 0 || puzzle2 == 0 || puzzle3 == 0 || puzzle4 == 0 || puzzle5 == 0 || puzzle6 == 0)
        {
            if (!DialogueIsActive && !sign.activeSelf)
                {
                    _elapsedTime = Time.time - _startTime;
                    if (_elapsedTime > delay){ // If more than delay seconds have passed since last hint
                        _startTime = Time.time;

                        if (PlayerPrefs.GetInt("MotelPosterPuzzle") == 0){
                            _dialogue = new string[] {missingMotelPoster};
                        }
                        else if (PlayerPrefs.GetInt("StorePuzzle") == 0) {
                            _dialogue = new string[] {missingStore};
                        }
                        else if (PlayerPrefs.GetInt("CampsitePuzzle") == 0){
                            _dialogue = new string[] {missingCampsite};
                        }
                        else if (PlayerPrefs.GetInt("FishshopPuzzle") == 0){
                            _dialogue = new string[] {missingFishshop};
                        } 
                        else if (PlayerPrefs.GetInt("PercyCamPuzzle") == 0){
                            _dialogue = new string[] {missingPercyCam};
                        }
                        else{
                            _dialogue = new string[] {missingDarkRoom};
                        }
                        DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                    }
                }
            else if (!timeAdjust) StartCoroutine(WaitAndAdd());
            
        }
    }

    private IEnumerator WaitAndAdd()
    {
        _startTime = Time.time + _elapsedTime;
        yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsActive() && !sign.activeSelf);
        if (!DialogueManager.Instance.DialogueIsActive() && !sign.activeSelf && !timeAdjust)
        {
            timeAdjust = true;
            _startTime += 5f;
            StopAllCoroutines();
            print("time adjusted");
        }
    }
}

    