using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Narrative
{
    /// <summary>
    /// A component class for the dialogue textbox display.
    /// </summary>
    public class DialogueBox : MonoBehaviour
    {
        //Event Callbacks
        public UnityEvent onAdvance;

        //Sub-object references
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private AdvanceArrow advanceArrow;
        [SerializeField] private Animator animator;

        //Object properties
        public float charactersPerSecond = 45.0f;//how many characters are displayed per second
        private float currentCharacter = 0f; //current position where
        private int textLength = 0;//cached content length

        //State tracking variables and getters, useful for timing
        //serialized so the animator can edit them but shouldn't be modified otherwise
        [SerializeField] [HideInInspector] private bool isOpen = false;
        [SerializeField] [HideInInspector] private bool isActive = false;
        public bool IsOpen { get { return isOpen; } }
        public bool IsActive { get { return isActive; } }

        // Start is called before the first frame update
        private void Start()
        {
            //Connect onAdvance event to hide the advancearrow object on invoke
            onAdvance.AddListener(() => advanceArrow.SetVisible(false));
        }

        // Update is called once per frame
        void Update()
        {
            if (isOpen)
            {
                //Update state
                CheckInput();
                UpdateText();
            }
        }

        /// <summary>
        /// Checks player input during frame update
        /// </summary>
        private void CheckInput()
        {
            //Input for advancing textbox
            if (Input.GetButtonDown("Submit"))
            {
                if (isEndOfText())
                {
                    AdvanceLine();
                }
            }
        }

        /// <summary>
        /// Advances per character text.
        /// </summary>
        private void UpdateText()
        {
            if (currentCharacter < textLength)
            {
                //Advance visible characters
                currentCharacter += Time.deltaTime * charactersPerSecond;
                textLabel.maxVisibleCharacters = Mathf.FloorToInt(currentCharacter);

                if (isEndOfText())
                {
                    advanceArrow.SetVisible(true);
                }
            }
        }

        /// <summary>
        /// Opens the textbox with playing the open animation
        /// </summary>
        public void OpenTextbox()
        {
            animator.SetBool("isOpen", true);
            advanceArrow.SetVisible(false);
            nameLabel.text = "";
            isActive = true;
        }

        /// <summary>
        /// Closes the textbox by playing the close animation
        /// </summary>
        public void CloseTextbox()
        {
            animator.SetBool("isOpen", false);
        }

        /// <summary>
        /// Sets the line and resets the per character scrolling
        /// </summary>
        /// <param name="sourceText"></param>
        public void SetLine(string sourceText)
        {
            textLabel.SetText(sourceText);

            //Reset scroll
            textLabel.maxVisibleCharacters = 0;
            currentCharacter = 0f;
            textLength = sourceText.Length;
        }

        /// <summary>
        /// Sets the speaker name.
        /// </summary>
        /// <param name="sourceText">The name to be displayed</param>
        public void SetName(string sourceText)
        {
            if (nameLabel.text != sourceText && nameLabel.text != "")
            {
                //Jostle the textbox when the speaker changes
                animator.SetTrigger("jostle");
            }

            //Apply text to label
            nameLabel.SetText(sourceText);
        }


        /// <summary>
        /// Checks if the per character scrolling reached the end of the text.
        /// </summary>
        /// <returns></returns>
        private bool isEndOfText()
        {
            return (currentCharacter >= textLength);
        }

        /// <summary>
        /// Called when the line advances. Invokes the onAdvance event.
        /// </summary>
        public void AdvanceLine()
        {
            advanceArrow.SetVisible(false);
            onAdvance.Invoke();
        }
    }
}