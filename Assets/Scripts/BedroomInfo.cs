using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomInfo : MonoBehaviour
{   
    // private int _bedroomPuzzle;
    // private int _tookCamera;
    private const string _lastHorizontal = "LastHorizontal";
    private void Start()
    {
        // _bedroomPuzzle = PlayerPrefs.GetInt("BedroomPuzzle");
        // _tookCamera = PlayerPrefs.GetInt("BedroomCam");
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
        // while (_bedroomPuzzle == 1 || _tookCamera == 1)
        // {
        //     _bedroomPuzzle = PlayerPrefs.GetInt("BedroomPuzzle");
        //     _tookCamera = PlayerPrefs.GetInt("BedroomCam");
        //     if (_bedroomPuzzle == 1 && _tookCamera == 1)
        //     {
                
        //         string[] leavingBedroom = {"I don't like you"};  
        //         DialogueManager.Instance.playBlockingDialogue("Jay", leavingBedroom);
        //         break;
        //     }
        // }
    }
}
