using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [Header ("Cutscene Commands (sound OR delay OR dialogue)")]
    public string nextScene;
    public string[] cutsceneCommands;
    

    private string delimiter = ":";
    private bool lastCommandFinished;
    private int commandIndex;

    [Header("Dialogue Panel Info")]
    public string dialogueLine;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float wordSpeed = 0.04f;
    public float secondsOnScreen = 4f; // how long should dialogue remain on screen once complete?


    // Start is called before the first frame update
    void Start()
    {
        lastCommandFinished = true;
        commandIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        // if executed all commands
        if (lastCommandFinished  && commandIndex + 1 >= cutsceneCommands.Length){
            LevelManager.Instance.LoadScene(nextScene, "CrossFade");
            lastCommandFinished = false; // prevent any more execution
        }
        // if there's more commands to play
        else if (lastCommandFinished){
            commandIndex++; // increment commandIndex
            lastCommandFinished = false;
            
            string command = cutsceneCommands[commandIndex];
            if (command.IndexOf(delimiter) == -1){
                throw new Exception("Invalid command at index " + commandIndex.ToString());
            }
            
            // do appropriate action depending on command type
            string command_type = command.Substring(0, command.IndexOf(delimiter));
            if(command_type == "delay"){
                // get length of delay
                string delayLengthStr = command.Substring(command.IndexOf(delimiter) + 1);
                float delayLength;
                if (!float.TryParse(delayLengthStr, out delayLength)){
                    throw new Exception("Invalid command at index " + commandIndex.ToString());
                }
                // start delay
                StartCoroutine(enterDelay(delayLength));
            }
            else if (command_type == "sound"){
                // get sound name
                string soundName = command.Substring(command.IndexOf(delimiter) + 1);

                if (!SoundManager.Instance.DoesSoundExist(soundName)){
                    throw new Exception("Cannot find sound " + soundName);
                }
                else{
                    SoundManager.Instance.PlaySound2D(soundName);
                    lastCommandFinished = true;
                }
            }
            else if (command_type == "soundLoop"){
                // get sound name
                string soundName = command.Substring(command.IndexOf(delimiter) + 1);

                if (!SoundManager.Instance.DoesSoundExist(soundName)){
                    throw new Exception("Cannot find sound " + soundName);
                }
                else{
                    SoundManager.Instance.PlayLoopingSound2D(soundName);
                    lastCommandFinished = true;
                }
            }
            else if (command_type == "dialogue"){
                // get dialogue
                dialogueLine = command.Substring(command.IndexOf(delimiter) + 1);
                // init dialogue panel
                dialogueText.text = "";
                dialoguePanel.SetActive(true);
                // start typing
                StartCoroutine(Typing(secondsOnScreen));
            }
            else{
                throw new Exception("Invalid command at index " + commandIndex.ToString());
            }
        }
    }

    private IEnumerator enterDelay(float seconds){
        yield return new WaitForSeconds(seconds);
        lastCommandFinished = true;
    }

    private IEnumerator Typing(float secondsOnScreen)
    {
        // this function types out each individual letter of the dialogue
        foreach (char letter in dialogueLine.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        if (secondsOnScreen > 0)
        {
            yield return new WaitForSeconds(secondsOnScreen);
        }
        lastCommandFinished = true;
    }
}
