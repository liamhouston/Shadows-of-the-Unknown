using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomInfo : MonoBehaviour
{   
    // private int _bedroomPuzzle;
    // private int _tookCamera;
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
}
