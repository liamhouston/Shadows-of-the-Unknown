using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Campsite : MonoBehaviour
{
    public GameObject xbutton;
    public GameObject sign;
    public GameObject paperclue;
    public GameObject pannel;
    private float _startTime;
    private float _elapsedTime;
    string[] _dialogue = {};
    private void Start()
    {
        _startTime = Time.time; // Store the time when the player enters the campsite
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            TryGetComponent(out Collider2D campsiteCollider);
            campsiteCollider.enabled = false;
            xbutton.SetActive(true);
        }
    }
    
    private void Update()
    {
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            TryGetComponent(out Collider2D campsiteCollider);
            campsiteCollider.enabled = false;
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false && sign.activeSelf == false && paperclue.activeSelf == false) 
                {
                    _elapsedTime = Time.time - _startTime;
                    if (_elapsedTime > 10) // If more than 10 seconds have passed
                    {
                        
                        _dialogue = new string[] {"Gotta leave store to find Percy. Campsite"};
                        print("You have been in the campsite for more than 10 seconds");
                        DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                        _startTime = Time.time;
                    }
                }
                else _startTime = Time.time;
        }
        else
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false && sign.activeSelf == false && paperclue.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (_elapsedTime > 10) // If more than 10 seconds have passed
                {
                    
                    _dialogue = new string[] {"Why do you take this much time? Campsite"};
                    print("You have been in the campsite for more than 10 seconds");
                    DialogueManager.Instance.playBlockingDialogue("Jay", _dialogue);
                    _startTime = Time.time;
                }
            }
            else _startTime = Time.time;
        }
        
        
    }
}
