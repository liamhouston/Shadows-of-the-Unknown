using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fishdockinfo : MonoBehaviour
{
    private const string _lastHorizontal = "LastHorizontal";
    
    [Header("Puzzle Objects")]
    public GameObject MotelPoster;
    public GameObject PercyCam;
    public GameObject Fishshop;
    public GameObject Campsite;
    public GameObject Store;

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
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);

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
    }
}
