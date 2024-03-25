using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilmPickUp : MonoBehaviour
{
    public Button exitButton;
    public GameObject film;

    private bool playerIsNearby = false;
    public bool playerFoundMajorClue = false;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.ClickInput && !playerFoundMajorClue){
            playerFoundMajorClue = true;
            exitButton.gameObject.SetActive(true);
            exitButton.interactable = true; 
            film.SetActive(false); // make film invisible
            SoundManager.Instance.PlaySound2D("MajorClue");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = false;
        }
    }
}
