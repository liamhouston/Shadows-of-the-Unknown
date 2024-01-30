using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{
    private bool playerIsNearby = false;
    private float imageHeight = 400;

    public GameObject itemCanvas;

    public GameObject InteractwithText;
    //public AudioSource InteractwithSound;


    // Start is called before the first frame update
    void Start(){
        InteractwithText.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.I) && playerIsNearby){
            if (itemCanvas.activeSelf){
                itemCanvas.SetActive(false);
            }
            else {
                // get the sprite image from this object
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                
                // Calculate the aspect ratio of the image
                float aspectRatio = (float) spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.bounds.size.y;
                float newWidth = imageHeight * aspectRatio;

                // Set the size of the Canvas based on the aspect ratio
                itemCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, imageHeight);

                // Set the RawImage component of the Canvas to display the inspected image
                itemCanvas.GetComponentInChildren<Image>().sprite = spriteRenderer.sprite;

                itemCanvas.SetActive(true);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = true;
            InteractwithText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = false;
            InteractwithText.SetActive(false);
            itemCanvas.SetActive(false);
        }    
    }
}
