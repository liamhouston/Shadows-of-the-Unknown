using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Narrative
{
    /// <summary>
    /// Component that controls the sequencing of a dialogue.
    /// </summary>
    public class DialogueSequencer : MonoBehaviour
    {
        //Event Callbacks
        public UnityEvent onFinish;

        //Sub-Object references
        [SerializeField] private DialogueBox textbox;
        [SerializeField] private DialoguePortraits portraits;
        [SerializeField] private DialogueChoices choices;

        //Object properties
        private DialogueSequence currentDialog;  //set when we are playing
        private int currentLine = 0;  //current line in the sequence we are playing
        private bool isPlaying = false;


        /// <summary>
        /// Starts playing the given sequence
        /// </summary>
        /// <param name="dialogue">Dialogue resource to play</param>
        public void PlaySequence(DialogueSequence dialogue)
        {
            if (dialogue.IsEmpty())
            {
                Debug.LogWarning("Playing sequence stopped because DialogueSequence was empty");
                return;
            }

            //Set our state
            currentDialog = dialogue;
            currentLine = 0;
            isPlaying = true;

            //Open and play
            textbox.OpenTextbox();
            ParseLine(currentLine);
        }

        /// <summary>
        /// Event reciever that advances the dialogue to the next line or closes if the dialogue is finished
        /// </summary>
        public void onSequenceAdvanced()
        {
            bool hasNext = currentDialog.HasLine(currentLine + 1);
            if (hasNext)
            {
                //Start next line
                currentLine++;
                ParseLine(currentLine);
            }
            else
            {
                //Finished, close textbox
                textbox.CloseTextbox();
                portraits.ClosePortraits();
                onFinish.Invoke();
            }
        }

        /// <summary>
        /// Applies the current line.
        /// </summary>
        /// <param name="lineNum">Line number</param>
        private void ParseLine(int lineNum)
        {
            //Apply to textbox
            textbox.SetLine(currentDialog.GetRowDialogue(lineNum));
            //Apply to textbox speaker name
            string name = currentDialog.GetRowName(lineNum);
            if (name != "")//Only apply if not empty
            {
                textbox.SetName(name);
            }
            //Apply to portrait
            string portrait = currentDialog.GetRowPortrait(lineNum);
            if (portrait != "")//Only apply if not empty
            {
                //textbox.SetPortrait(portrait);
                Sprite spr = DialoguePortraitContainer.GetPortrait(portrait);
                portraits.SetPortraitSprite(spr);
            }
        }

        /// <summary>
        /// Checks if the sequencer is playing a sequence
        /// </summary>
        public bool IsPlaying()
        {
            return textbox.IsActive;
        }

    }
}