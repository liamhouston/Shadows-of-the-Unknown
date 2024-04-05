using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialBox : MonoBehaviour
{
    public GameObject tutorialBox;
    private bool isHovering = false;

    private void Start()
    {
        tutorialBox.SetActive(false);
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name) == 0)
        {
            CursorManager.Instance.MouseColliderSwitch();
            InputManager.PlayerInput.actions.FindAction("Move").Disable();
            tutorialBox.SetActive(true);

            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
            pointerClickEntry.eventID = EventTriggerType.PointerClick;
            pointerClickEntry.callback.AddListener((eventData) => { OnPointerClick((PointerEventData)eventData); });
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        if (isHovering)
        {
            onClicked();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Debug.Log("Not Hovering");
    }

    void onClicked()
    {
        // Time.timeScale = 1f;
        CursorManager.Instance.MouseColliderSwitch();
        InputManager.PlayerInput.actions.FindAction("Move").Enable();
        tutorialBox.SetActive(false);
    }
}
