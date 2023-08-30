using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    /// <summary>
    /// A script that will activate a dialogue when triggered.
    /// Must be triggered manually but it will attempt to automatically connect to a ClickableObject script.
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        /// <summary> The csv file containing the dialogue to be played. </summary>
        [SerializeField] private TextAsset dialogueCSV;

        /// <summary>
        /// An inline struct to contain dialogue activation conditions
        /// </summary>
        [System.Serializable]
        public struct Condition
        {
            [Tooltip("Which flag to check, if set to -1 the dialogue will always play when triggered.")]
            [Range(-1, DialogueFlags.NUMFLAGS)] public int flagID;

            [Tooltip("If the flag matches this value this condition will succeed.")]
            public bool expectedValue;
        }

        [Header("Conditions")]
        [SerializeField] private List<Condition> conditions = new List<Condition>();


        [Header("Set Dialogue Flag After Finishing Dialogue")]
        [Tooltip("Which flag to assign, if set to -1 no flag will be set.")]
        [SerializeField]
        [Range(-1, DialogueFlags.NUMFLAGS)]
        private int writeToFlagId = -1;
        [SerializeField] private bool writeToFlagValue = true;

        // Start is called before the first frame update
        void Start()
        {
            AttemptSubscribeToClickable();
        }

        /// <summary>
        /// Tries to subscribe to any clickable components on this gameobject or it's direct parent object.
        /// </summary>
        void AttemptSubscribeToClickable()
        {
            //Find component on this gameobject
            ClickableObject clickable = GetComponent<ClickableObject>();
            if (clickable)
            {
                clickable.onClick.AddListener(Trigger);
                return;
            }

            //Find component on parent object
            clickable = GetComponentInParent<ClickableObject>();
            if (clickable)
            {
                clickable.onClick.AddListener(Trigger);
                return;
            }

            Debug.LogWarning("Couldn't find ClickableObject to connect to in object " + name + ". Ensure it or it's parent has a ClickableObject component.");
        }

        /// <summary>
        /// Call this to activate the dialogue. If condition are set they must all be satisfied.
        /// </summary>
        private void Trigger()
        {
            if (DialogueSystem.IsPlaying())
            {
                return;//Don't activate if already playing something
            }

            //Check conditions
            if (!AreConditionsTrue())
            {
                return; //Cancel activation if any conditions fail
            }

            //Activate Dialogue
            DialogueSystem.OnDialogueEnd.AddListener(OnDialogueEnd);
            DialogueSystem.PlaySequence(dialogueCSV);
        }

        /// <summary>
        /// Evaluates whether all conditions are satisfied.
        /// </summary>
        private bool AreConditionsTrue()
        {
            foreach (Condition condition in conditions)
            {
                if (DialogueFlags.GetFlagValue(condition.flagID) != condition.expectedValue)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Callback reciever for ehrn dialogue ends.
        /// Writes to flags if it is set
        /// </summary>
        private void OnDialogueEnd()
        {
            if (writeToFlagId >= 0)
            {
                Debug.Log(name + " assigning flag " + writeToFlagId + " = " + writeToFlagValue);
                DialogueFlags.SetFlag(writeToFlagId, writeToFlagValue);
            }
            DialogueSystem.OnDialogueEnd.RemoveListener(OnDialogueEnd);//We shouldn't recieve this if we aren't playing something.
        }
    }
}