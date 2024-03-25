using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilmPickUp : MonoBehaviour
{
    public Button exitButton;
    public SpriteRenderer filmSprite;

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
            filmSprite.color = new Color(filmSprite.color.r, filmSprite.color.g, filmSprite.color.b, 0); // make film sprite invisible
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
