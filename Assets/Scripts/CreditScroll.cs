using UnityEngine;
using UnityEngine.UI;

public class CreditScroll : MonoBehaviour
{
    public float startYPos;
    public float endYPos; // The Y position where the scrolling should stop
    public float scrollTime; // The duration of the scrolling animation
    public RectTransform rectTrans; // Reference to the RectTransform component of the credits container
    public Button backButton;

    private float startTime; // The time when the scrolling started
    private Vector2 startPos; // The starting position of the credits container

    void Update()
    {
        // Calculate the current progress of the scrolling animation
        float elapsedTime = Time.time - startTime;
        float t = Mathf.Clamp01(elapsedTime / scrollTime);

        // Interpolate between the start and end positions based on the progress
        Vector2 newPos = Vector2.Lerp(new Vector2(rectTrans.anchoredPosition.x, startYPos), new Vector2(rectTrans.anchoredPosition.x, endYPos), t);

        // Update the position of the credits container
        rectTrans.anchoredPosition = newPos;

        // If the animation is complete, stop updating
        if (t >= 1.0f)
        {
            enabled = false;
            backButton.interactable = true;
        }
    }

    public void StartCredits(){
        enabled = true; 
        backButton.interactable = false;
        rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, startYPos);
        startTime = Time.time;
    }
}
