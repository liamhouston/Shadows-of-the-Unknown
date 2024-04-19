using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Campsite : MonoBehaviour
{   
    public string hintDialogue;
    public int delay;
    private bool hintPlayed = false;


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
        }
        else
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false && sign.activeSelf == false && paperclue.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!hintPlayed && _elapsedTime > delay) {
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {hintDialogue});
                    hintPlayed = true;
                    _startTime = Time.time;
                }
            }
        }
        
        
    }
}
