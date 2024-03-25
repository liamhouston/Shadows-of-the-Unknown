using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("Bedroom", 0);
        PlayerPrefs.SetInt("BedroomCam", 0);
        PlayerPrefs.SetInt("Fishdock", 0);
        PlayerPrefs.SetInt("Campsite", 0);
        PlayerPrefs.SetInt("Store", 0);
        PlayerPrefs.SetInt("Darkroom", 0);
        PlayerPrefs.SetString("FromScene", "");
    }
}
