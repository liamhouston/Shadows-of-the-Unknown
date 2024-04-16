using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // if puzzle is done, disable the puzzle
    public GameObject xbutton;
    private float _startTime;
    private float _elapsedTime;
    public GameObject pannel;
    string[] _dialogue = {};
    private void Start()
    {
         _startTime = Time.time; // Store the time when the player enters the campsite
        if (PlayerPrefs.GetInt("StorePuzzle") == 1)
        {
            TryGetComponent(out Collider2D storeCollider);
            storeCollider.enabled = false;
            xbutton.SetActive(true);
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("StorePuzzle") == 1)
        {
            TryGetComponent(out Collider2D storeCollider);
            storeCollider.enabled = false;
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (_elapsedTime > 10) // If more than 10 seconds have passed
                {
                    
                    _dialogue = new string[] {"Gotta leave store to find Percy. Store"};
                    print("You have been in the campsite for more than 20 seconds");
                    DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                    _startTime = Time.time;
                }
            }
            else _startTime = Time.time;
        }
        else
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (_elapsedTime > 10) // If more than 10 seconds have passed
                {
                    
                    _dialogue = new string[] {"Why do you take this much time? Store"};
                    print("You have been in the campsite for more than 10 seconds");
                    DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                    _startTime = Time.time;
                }
            }
            else _startTime = Time.time;
        }
    }
}
