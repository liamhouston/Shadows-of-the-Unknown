using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class Fishdockinfo : MonoBehaviour
{
    private const string _lastHorizontal = "LastHorizontal";
    
    [Header("Puzzle Objects")]
    public GameObject MotelPoster;
    public GameObject PercyCam;
    public GameObject Fishshop;
    public GameObject Campsite;
    public GameObject Store;

    public GameObject pannel;
    public GameObject sign;
    private float _startTime;
    private float _elapsedTime;
    string[] _dialogue = {};

    private int puzzle1;
    private int puzzle2;
    private int puzzle3;
    private int puzzle4;
    private int puzzle5;
    private int puzzle6;


    // private void Awake()
    // {
    //     // PlayerPrefs.SetInt("Bedroom", 0);
    //     // PlayerPrefs.SetInt("BedroomCam", 0); // player has camera?
    //     // PlayerPrefs.SetInt("Fishdock", 0);
    //     // PlayerPrefs.SetInt("Campsite", 0);
    //     // PlayerPrefs.SetInt("Store", 0);
    //     // PlayerPrefs.SetInt("Darkroom", 0);
    //     // PlayerPrefs.SetString("FromScene", "");
        
    //     // PlayerPrefs.SetInt("BedroomPuzzle", 0); // player did the puzzle from the trash can?
    //     // PlayerPrefs.SetInt("StorePuzzle", 0);   // player took the pic from store?
    //     // PlayerPrefs.SetInt("CampsitePuzzle", 0);    // player took the pic from campsite?
    //     // PlayerPrefs.SetInt("DarkroomPuzzle", 0);    // player did the puzzle from darkroom?
    //     // PlayerPrefs.SetInt("LeaveBedroom", 0);    // player did the puzzle from fishdock?

    //     // PlayerPrefs.SetInt("MotelPosterPuzzle", 0); // for testing
    //     // PlayerPrefs.SetInt("MotelPoster", 0);
    //     // PlayerPrefs.SetInt("PercyCamPuzzle", 0); // for testing
    //     // PlayerPrefs.SetInt("PercyCam", 0);
    //     // PlayerPrefs.SetInt("FishshopPuzzle", 0); // for testing
    //     // PlayerPrefs.SetInt("Fishshop", 0);

    //     // PlayerPrefs.SetInt("BedroomPuzzle", 1); // player did the puzzle from the trash can?
    //     // PlayerPrefs.SetInt("StorePuzzle", 1);   // player took the pic from store?
    //     // PlayerPrefs.SetInt("CampsitePuzzle", 1);    // player took the pic from campsite?
    //     // PlayerPrefs.SetInt("DarkroomPuzzle", 1);    // player did the puzzle from darkroom?
    //     // PlayerPrefs.SetInt("FishshopPuzzle", 1); // for testing
    //     // PlayerPrefs.SetInt("PercyCamPuzzle", 1); // for testing
    //     // PlayerPrefs.SetInt("MotelPosterPuzzle", 1); // for testing
    // }
    private void Start()
    {
        // string[] puzzles = new string[] { "BedroomPuzzle", "StorePuzzle", "CampsitePuzzle", "FishshopPuzzle", "PercyCamPuzzle", "MotelPosterPuzzle" };
        // List<string> unsolvedPuzzles = new List<string>();

        // foreach (string puzzle in puzzles)
        // {
        //     if (PlayerPrefs.GetInt(puzzle) == 0)
        //     {
        //         unsolvedPuzzles.Add(puzzle);
        //     }
        // }
        // string value = "";
        // int n = unsolvedPuzzles.Count;
        // if (n != 0)
        // {
        //     int k = Random.Range(0, n-1);
        //     value = unsolvedPuzzles[k];
        // }
        
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);

        puzzle1 = PlayerPrefs.GetInt("BedroomPuzzle");
        puzzle2 = PlayerPrefs.GetInt("StorePuzzle");
        puzzle3 = PlayerPrefs.GetInt("CampsitePuzzle");
        puzzle4 = PlayerPrefs.GetInt("FishshopPuzzle");
        puzzle5 = PlayerPrefs.GetInt("PercyCamPuzzle");
        puzzle6 = PlayerPrefs.GetInt("MotelPosterPuzzle");

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
            DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {"What was that?"});
            MotelPoster.SetActive(false);
            PercyCam.SetActive(false);
            Fishshop.SetActive(false);
            Campsite.SetActive(false);
            Store.SetActive(false);
        }
    }
    private void Update()
    {
        if (puzzle1 == 0 || puzzle2 == 0 || puzzle3 == 0 || puzzle4 == 0 || puzzle5 == 0 || puzzle6 == 0)
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false && sign.activeSelf == false)
                {
                    _elapsedTime = Time.time - _startTime;
                    if (_elapsedTime > 10) // If more than 10 seconds have passed
                    { 
                        _dialogue = new string[] {"I still need to check some other place here. Fishdock"};
                        print("You have been in the campsite for more than 10 seconds");
                        DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                        _startTime = Time.time;
                    }
                }
                else _startTime = Time.time;
        }
    }
}

    