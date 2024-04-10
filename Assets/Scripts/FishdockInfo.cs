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
            Campsite.GetComponent<Light2D>().color = new Color(255f/255f, 255f/255f, 255f/255f);
        }
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            Campsite.GetComponent<Light2D>().color = new Color(255f/255f, 255f/255f, 255f/255f);
        }
    }
}
