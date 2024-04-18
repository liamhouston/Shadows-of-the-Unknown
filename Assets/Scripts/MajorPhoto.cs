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
            // PlayerPrefs.SetInt("PercyCamPuzzle", 0); // for testing
            // PlayerPrefs.SetInt("PercyCam", 0);
            // PlayerPrefs.SetInt("FishshopPuzzle", 0); // for testing
            // PlayerPrefs.SetInt("Fishshop", 0);

            print("MajorPhoto script is running");
            // print("current scene: " + SceneManager.GetActiveScene().name);
            // print("MotelPosterPuzzle: " + PlayerPrefs.GetInt("MotelPosterPuzzle"));



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
        // Debug.Log("Enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // isHovering = false;
        // Debug.Log("Not Hovering");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // collider.enabled = true;
        string[] PosterDialogue = {};
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            if (SceneManager.GetActiveScene().name == "MotelPoster" && PlayerPrefs.GetInt("MotelPosterPuzzle") == 0)
            {
                PosterDialogue = new string[] {"Sheesh, he was getting really serious about this, huh? Film cameras…", 
                                            "I wonder if this camera was the, uh, subject of the expedition. Maybe I should take a picture."
                                        };
                
                
                // collider.enabled = false;
                
            }
            else if (SceneManager.GetActiveScene().name == "PercyCam" && PlayerPrefs.GetInt("PercyCamPuzzle") == 0)
            {
                PosterDialogue = new string[] {"Wow, weird place to leave this. It doesn’t look like there’s a memory card inside either, but it’s definitely Percy’s.", 
                                                "Maybe he didn’t need it anymore after he found the camera he lent me.", 
                                                " I’ll take a picture and hopefully it’ll start to make more sense later."
                                                };
            }
            else if (SceneManager.GetActiveScene().name == "Fishshop" && PlayerPrefs.GetInt("FishshopPuzzle") == 0)
            {
                PosterDialogue = new string[] {"Jeez, how the hell did he even get in here? He's not the type to break-and-enter just—just randomly like that.",
                                                "Too bad I can’t see further into the store. I’ll take a picture and bring it up to him whenever I find him."
                                                }; 
            }

            DialogueManager.Instance.playBlockingDialogue("Jay", PosterDialogue);
            
        }
        else
        {
            Debug.Log("Right click");
            if (SceneManager.GetActiveScene().name == "MotelPoster" && PlayerPrefs.GetInt("MotelPosterPuzzle") == 0)
            {
                PlayerPrefs.SetInt("MotelPosterPuzzle", 1);
                PosterDialogue = new string[] {"So Percy was using this camera for his work. It’s strange that he’d leave it to me?…",
                                               "Or maybe the camera wanted to be left to me."
                                            };        
                SoundManager.Instance.PlaySound2D("MajorClue");
                DialogueManager.Instance.playBlockingDialogue("Jay", PosterDialogue);
                xbutton.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().name == "PercyCam" && PlayerPrefs.GetInt("PercyCamPuzzle") == 0)
            {
                PlayerPrefs.SetInt("PercyCamPuzzle", 1);
                PosterDialogue = new string[] {"It’s so weird that he’d abandon his other cameras. This one was expensive, too. I suppose when the camera is this good at picking up things, you don’t really need others anymore."
                                            };
                SoundManager.Instance.PlaySound2D("MajorClue");
                DialogueManager.Instance.playBlockingDialogue("Jay", PosterDialogue);
                xbutton.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().name == "Fishshop" && PlayerPrefs.GetInt("FishshopPuzzle") == 0)
            {
                PlayerPrefs.SetInt("FishshopPuzzle", 1);
                PosterDialogue = new string[] {"Damn, I don’t think I can get in there. That blows. What was Percy doing in there? And how did he even get in?"};
                SoundManager.Instance.PlaySound2D("MajorClue");
                DialogueManager.Instance.playBlockingDialogue("Jay", PosterDialogue);
                xbutton.SetActive(true);
            }
            {
                
            }
            
        }

        
        
    }   
}