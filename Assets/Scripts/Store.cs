using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public string hintDialogue;
    public int delay;
    private bool hintPlayed = false;
    private bool timeAdjust = false;

    // if puzzle is done, disable the puzzle
    public GameObject xbutton;
    private float _startTime;
    private float _elapsedTime;
    // public GameObject pannel;
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
        bool DialogueIsActive = DialogueManager.Instance.DialogueIsActive();
        if (!DialogueIsActive) timeAdjust = false;
        if (PlayerPrefs.GetInt("StorePuzzle") == 1)
        {
            TryGetComponent(out Collider2D storeCollider);
            storeCollider.enabled = false;
        }
        else
        {
            if (!DialogueIsActive) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!hintPlayed && _elapsedTime > delay) {
                    hintPlayed = true;
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {hintDialogue});
                    _startTime = Time.time;
                }
            }
            else if (!timeAdjust) StartCoroutine(WaitAndAdd());
        }
    }
    private IEnumerator WaitAndAdd()
    {
        _startTime = Time.time + _elapsedTime;
        yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsActive());
        if (!DialogueManager.Instance.DialogueIsActive() && !timeAdjust)
        {
            timeAdjust = true;
            _startTime += 5f;
            StopAllCoroutines();
            print("time adjusted");
            
        }
    }
}
