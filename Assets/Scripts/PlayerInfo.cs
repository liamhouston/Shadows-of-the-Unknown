using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("Bedroom", 0);
        PlayerPrefs.SetInt("BedroomCam", 0); // player has camera?
        PlayerPrefs.SetInt("Fishdock", 0);
        PlayerPrefs.SetInt("Campsite", 0);
        PlayerPrefs.SetInt("Store", 0);
        PlayerPrefs.SetInt("Darkroom", 0);
        PlayerPrefs.SetString("FromScene", "");
        
        PlayerPrefs.SetInt("BedroomPuzzle", 0); // player did the puzzle from the trash can?
        PlayerPrefs.SetInt("StorePuzzle", 0);   // player took the pic from store?
        PlayerPrefs.SetInt("CampsitePuzzle", 0);    // player took the pic from campsite?
        PlayerPrefs.SetInt("DarkroomPuzzle", 0);    // player did the puzzle from darkroom?
        PlayerPrefs.SetInt("LeaveBedroom", 0);    // player did the puzzle from fishdock?

        PlayerPrefs.SetInt("MotelPosterPuzzle", 0); // for testing
        PlayerPrefs.SetInt("MotelPoster", 0);
        PlayerPrefs.SetInt("PercyCamPuzzle", 0); // for testing
        PlayerPrefs.SetInt("PercyCam", 0);
        PlayerPrefs.SetInt("FishshopPuzzle", 0); // for testing
        PlayerPrefs.SetInt("Fishshop", 0);
    }
}
