using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public string hintDialogue;
    public int delay;
    private bool hintPlayed = false;

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
        }
        else
        {
            if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!DialogueManager.Instance.DialogueIsActive() && pannel.activeSelf == false) {
                    _elapsedTime = Time.time - _startTime;
                    if (!hintPlayed && _elapsedTime > delay) {
                        hintPlayed = true;
                        DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {hintDialogue});
                        _startTime = Time.time;
                    }
                }
            }
        }
    }
}
