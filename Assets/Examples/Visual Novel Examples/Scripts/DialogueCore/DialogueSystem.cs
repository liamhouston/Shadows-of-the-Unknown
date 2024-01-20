using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Narrative
{

    
    /// <summary>
    /// The core dialogue system intterface for objects in the scene.
    /// </summary>
    public class DialogueSystem : MonoBehaviour
    {
        //Event Callbacks
        public UnityEvent onDialogueStarted;
        public UnityEvent onDialogueEnd;
        public static UnityEvent OnDialogueEnd { get { return Instance.onDialogueEnd; } }

        //Singleton pattern
        private static DialogueSystem _instance;
        public static DialogueSystem Instance { get { return _instance; } }


        [SerializeField] private DialogueSequencer dialogueSequencer;//reference to the sequencer
        [SerializeField] private Canvas dialogueCanvas; //reference to the ui canvas

        public DialogueSystem()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Add event listeners
            dialogueSequencer.onFinish.AddListener(OnFinish);
            dialogueCanvas.enabled = true;//Enable the canvas only on runtime so it doesn't get in the way of scene editing
        }


        /// <summary>
        /// Plays the dialogue sequence.
        /// </summary>
        /// <param name="asset">Text asset source to play</param>
        public static void PlaySequence(TextAsset asset)
        {
            PlaySequence(new DialogueSequence(asset));
        }

        /// <summary>
        /// Plays the dialogue sequence.
        /// </summary>
        /// <param name="dialogue">Dialogue sequence object to play</param>
        public static void PlaySequence(DialogueSequence dialogue)
        {
            Instance.dialogueSequencer.PlaySequence(dialogue);
            Instance.onDialogueStarted.Invoke();
        }

        /// <summary>
        /// Checks if the sequencer is currently playing something
        /// </summary>
        public static bool IsPlaying()
        {
            return Instance.dialogueSequencer.IsPlaying();
        }

        /// <summary>
        /// Callback reciever for when the sequence has ended.
        /// </summary>
        private static void OnFinish()
        {
            Instance.onDialogueEnd.Invoke();
        }
    }
}