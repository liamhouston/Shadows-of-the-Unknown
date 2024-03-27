using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{
    [SerializeField]
    private bool playerIsNearby;
    public float imageHeight = 400;

    public GameObject itemCanvas;

    public Sprite displayImage = null;

    void Start(){
        playerIsNearby = false;
    }

    // Update is called once per frame
    void Update()
    {
         // if open and click happens, close
        if (InputManager.Instance.ClickInput && itemCanvas.activeSelf){
            itemCanvas.SetActive(false);
            CursorManager.Instance.MouseColliderEnable(true);
        }

        // open if player nearby
        if (InputManager.Instance.ClickInput && playerIsNearby && !DialogueManager.Instance.DialogueIsActive())
        {
            // Set the image to the display image or the sprite
            itemCanvas.SetActive(true);
            CursorManager.Instance.MouseColliderEnable(false);
            playerIsNearby = false;

            if (displayImage != null)
            {
                // Calculate the aspect ratio of the image
                float aspectRatio = (float)displayImage.bounds.size.x / displayImage.bounds.size.y;
                float newWidth = imageHeight * aspectRatio;
                // Set the size of the Canvas based on the aspect ratio
                itemCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, imageHeight);
                itemCanvas.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2f, Screen.height / 2f);
                itemCanvas.GetComponentInChildren<RawImage>().texture = displayImage.texture;
            }
            else
            {
                // Calculate the aspect ratio of the image
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                Debug.Assert(spriteRenderer != null, "SpriteRenderer must exist on this object");
                float aspectRatio = (float)spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.bounds.size.y;
                float newWidth = imageHeight * aspectRatio;
                // Set the size of the Canvas based on the aspect ratio
                itemCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, imageHeight);
                itemCanvas.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2f + newWidth / 2f, Screen.height / 2f + imageHeight / 2f);
                itemCanvas.GetComponentInChildren<RawImage>().texture = spriteRenderer.sprite.texture;
            }
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
