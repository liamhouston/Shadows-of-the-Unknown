using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JayResponse : MonoBehaviour
{
    public string[] jayResponseDialogue;
    public string[] MrDResponseDialogue;
    
    private bool playedJayResponse;
    private bool playedMrDResponse;

    void Start(){
        playedJayResponse = false; 
        playedMrDResponse = false;
    }

    // Update is called once per frame
    void Update()
    {
            
        // go through simulated conversation by checking if dialogue active
        if (!DialogueManager.Instance.DialogueIsActive() && !playedJayResponse){
            playedJayResponse = true;
            DialogueManager.Instance.playBlockingDialogue("Jay", jayResponseDialogue);
        }
        else if (!DialogueManager.Instance.DialogueIsActive() && playedJayResponse && !playedMrDResponse){
            playedMrDResponse = true;
            DialogueManager.Instance.playBlockingDialogue("Mr. D", MrDResponseDialogue);
        }
    }
}
