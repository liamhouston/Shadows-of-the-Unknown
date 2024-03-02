using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{

    private bool playerIsNearby = false;
    public float imageHeight = 400;

    public GameObject itemCanvas;

    public GameObject InteractwithText;

    public Sprite displayImage = null;
    //public AudioSource InteractwithSound;

    //private Color originalColor;
    //public Color highlightColor = new Color(1f, 0.5255f, 0.5255f);


    // Start is called before the first frame update
    void Start(){
        InteractwithText.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if (InputManager.Instance.InteractInput && playerIsNearby){
            if (itemCanvas.activeSelf){
                itemCanvas.SetActive(false);
            }
            else {
                // Set the image to the display image or the sprite
                itemCanvas.SetActive(true);
                if (displayImage != null){
                    // Calculate the aspect ratio of the image
                    float aspectRatio = (float) displayImage.bounds.size.x / displayImage.bounds.size.y;
                    float newWidth = imageHeight * aspectRatio;
                    // Set the size of the Canvas based on the aspect ratio
                    itemCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, imageHeight);
                    itemCanvas.GetComponentInChildren<RawImage>().texture = displayImage.texture;
                }
                else {
                    // Calculate the aspect ratio of the image
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    Debug.Assert(spriteRenderer != null, "SpriteRenderer must exist on this object");
                    float aspectRatio = (float) spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.bounds.size.y;
                    float newWidth = imageHeight * aspectRatio;
                    // Set the size of the Canvas based on the aspect ratio
                    itemCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, imageHeight);
                    itemCanvas.GetComponentInChildren<RawImage>().texture = spriteRenderer.sprite.texture;
                }
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = true;
            InteractwithText.SetActive(true);
            //spriteRenderer.color = highlightColor;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = false;
            InteractwithText.SetActive(false);
            itemCanvas.SetActive(false);
            //spriteRenderer.color = originalColor;
        }    
    }
}
