using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MajorPhoto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    // private bool isHovering = false;
    
    public Collider2D colliderclue;
    public GameObject xbutton;
    private void Start()
    {
        if (true)
        {
            // PlayerPrefs.SetInt("MotelPosterPuzzle", 0); // for testing
            // PlayerPrefs.SetInt("MotelPoster", 0);



            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
            pointerClickEntry.eventID = EventTriggerType.PointerClick;
            pointerClickEntry.callback.AddListener((eventData) => { OnPointerDown((PointerEventData)eventData); });
            eventTrigger.triggers.Add(pointerClickEntry);

            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
            pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
            pointerEnterEntry.callback.AddListener((eventData) => { OnPointerEnter((PointerEventData)eventData); });
            eventTrigger.triggers.Add(pointerEnterEntry);

            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => { OnPointerExit((PointerEventData)eventData); });
            eventTrigger.triggers.Add(pointerExitEntry);
        }        
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"Puzzle") == 1)
        {
            colliderclue.enabled = false;
        }
        else if (DialogueManager.Instance.DialogueIsActive())
        {
            colliderclue.enabled = false;
        }
        else
        {
            colliderclue.enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // isHovering = true;
        Debug.Log("Enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // isHovering = false;
        Debug.Log("Not Hovering");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // collider.enabled = true;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (SceneManager.GetActiveScene().name == "MotelPoster" && PlayerPrefs.GetInt("MotelPosterPuzzle") == 0)
            {
                string[] PosterDialogue = { "Sheesh, he was getting really serious about this, huh? Film cameras…", "I wonder if this camera was the, uh, subject of the expedition. Maybe I should take a picture." };
                
                DialogueManager.Instance.playBlockingDialogue("Jay", PosterDialogue);
                // collider.enabled = false;
                
            }
        }
        else
        {
            Debug.Log("Right click");
            if (SceneManager.GetActiveScene().name == "MotelPoster" && PlayerPrefs.GetInt("MotelPosterPuzzle") == 0)
            {
                PlayerPrefs.SetInt("MotelPosterPuzzle", 1);
                string[] PosterDialogue1 = {"That feels… Better, I think. I really do feel much less sick after I take a photo.", "I can kinda see how Percy got hooked on these things."};
                // TryGetComponent(out Collider2D collider);
                // collider.enabled = false;
                DialogueManager.Instance.playBlockingDialogue("Jay", PosterDialogue1);
                SoundManager.Instance.PlaySound2D("MajorClue");
        
                xbutton.SetActive(true);
            }
        }

        
        
    }   
}