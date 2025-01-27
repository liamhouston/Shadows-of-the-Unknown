using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarkroomTriggerCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Bedroom puzzle: " + PlayerPrefs.GetInt("BedroomPuzzle"));
        Debug.Log("Store puzzle: " + PlayerPrefs.GetInt("StorePuzzle"));
        Debug.Log("Campsite puzzle: " + PlayerPrefs.GetInt("CampsitePuzzle"));
        Debug.Log("Fishshop puzzle: " + PlayerPrefs.GetInt("FishshopPuzzle"));
        Debug.Log("PercyCam puzzle: " + PlayerPrefs.GetInt("PercyCamPuzzle"));
        Debug.Log("MotelPoster puzzle: " + PlayerPrefs.GetInt("MotelPosterPuzzle"));

        int puzzle1 = PlayerPrefs.GetInt("BedroomPuzzle");
        int puzzle2 = PlayerPrefs.GetInt("StorePuzzle");
        int puzzle3 = PlayerPrefs.GetInt("CampsitePuzzle");
        int puzzle4 = PlayerPrefs.GetInt("FishshopPuzzle");
        int puzzle5 = PlayerPrefs.GetInt("PercyCamPuzzle");
        int puzzle6 = PlayerPrefs.GetInt("MotelPosterPuzzle");

        SoundManager.Instance.PreloadSound("DarkroomOpen");

        // check the condition to open Darkroom
        if (puzzle1 == 1 && puzzle2 == 1 && puzzle3 == 1 && puzzle4 == 1 && puzzle5 == 1 && puzzle6 == 1)
        {
            TryGetComponent(out SpriteRenderer openDoor);
            TryGetComponent(out Light2D redLight);
            TryGetComponent(out Collider2D collider);
            redLight.enabled = true;
            collider.enabled = true;
            openDoor.enabled = true;
            SoundManager.Instance.PlaySound2D("DarkroomOpen");
        }
    }
}
