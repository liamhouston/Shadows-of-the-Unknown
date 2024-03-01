using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool playerIsNearby = false;
    private float imageHeight = 400;

    public GameObject itemCanvas;

    public GameObject InteractwithText;

    public Sprite displayImage = null;
    //public AudioSource InteractwithSound;

    //private Color originalColor;
    //public Color highlightColor = new Color(1f, 0.5255f, 0.5255f);


    // Start is called before the first frame update
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer must exist on this object");
        InteractwithText.SetActive(false);
        //originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update(){
        if (InputManager.Instance.InteractInput && playerIsNearby){
            if (itemCanvas.activeSelf){
                itemCanvas.SetActive(false);
            }
            else {
                // Calculate the aspect ratio of the image
                float aspectRatio = (float) spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.bounds.size.y;
                float newWidth = imageHeight * aspectRatio;

                // Set the size of the Canvas based on the aspect ratio
                itemCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, imageHeight);

                // Set the image to the display image or the sprite
                if (displayImage != null){
                    itemCanvas.GetComponentInChildren<RawImage>().texture = displayImage.texture;
                }
                else {
                    itemCanvas.GetComponentInChildren<RawImage>().texture = spriteRenderer.sprite.texture;
                }
                itemCanvas.SetActive(true);
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
