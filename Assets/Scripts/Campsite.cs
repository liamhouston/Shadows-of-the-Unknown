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
    private float _startTime;
    private float _elapsedTime;
    private bool timeAdjust = false;
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
        bool DialogueIsActive = DialogueManager.Instance.DialogueIsActive();
        if (!DialogueIsActive && !sign.activeSelf && !paperclue.activeSelf) timeAdjust = false;

        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            TryGetComponent(out Collider2D campsiteCollider);
            campsiteCollider.enabled = false;
        }
        else
        {
            if (!DialogueIsActive && !sign.activeSelf && !paperclue.activeSelf) 
            {
                _elapsedTime = Time.time - _startTime;
                if (!hintPlayed && _elapsedTime > delay) {
                    DialogueManager.Instance.playBlockingDialogue("Jay", new string[] {hintDialogue});
                    hintPlayed = true;
                    _startTime = Time.time;
                }
            }
            else if (!timeAdjust) StartCoroutine(WaitAndAdd());
        }
    }
    private IEnumerator WaitAndAdd()
    {
        _startTime = Time.time + _elapsedTime;
        yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsActive() && !sign.activeSelf && !paperclue.activeSelf);
        if (!timeAdjust)
        {
            // print("time adjust is true");
        }
        if (!DialogueManager.Instance.DialogueIsActive() && !sign.activeSelf && !paperclue.activeSelf && !timeAdjust)
        {
            timeAdjust = true;
            _startTime += 5f;
            StopAllCoroutines();
            print("time adjusted");
        }
    }
}
