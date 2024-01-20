using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Narrative
{
    /// <summary>
    /// Display component for a dialogue portrait.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DialoguePortrait : MonoBehaviour
    {
        private static readonly Color CLEAR = new Color(1.0f, 1.0f, 1.0f, 0.0f);//Color class doesn't contain clear white const so define it ourselves

        private Image spriteRenderer; //reference to the Unity UI image, retrieved automatically
        private Coroutine fadeCoroutine; //the current coroutine for fading if it currently exists

        //State check variables and getters
        private bool isVisible = false;
        public bool IsVisible { get { return isVisible; } }
        private bool isEntered = false;
        public bool IsEntered { get { return isEntered; } }

        // Awake is called before Start
        void Awake()
        {
            //Obtain neccesary components
            spriteRenderer = GetComponent<Image>();
        }


        /// <summary>
        /// Sets the current portrait sprite
        /// </summary>
        /// <param name="sourceSprite">Sprite to set to</param>
        public void SetSprite(Sprite sourceSprite)
        {
            spriteRenderer.sprite = sourceSprite;
        }

        /// <summary>
        /// Starts a fade in animation via coroutine
        /// </summary>
        /// <param name="duration">How long to fade in seconds</param>
        public void EnterFade(float duration)
        {
            if(fadeCoroutine != null)
            {
                //Stop any other fades if they were playing
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(IEnterFade(duration));
        }

        /// <summary>
        /// Starts a fade out animation via coroutine
        /// </summary>
        /// <param name="duration">How long to fade out seconds</param>
        public void ExitFade(float duration)
        {
            if(fadeCoroutine != null)
            {
                //Stop any other fades if they were playing
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(IExitFade(duration));
        }

        /// <summary>
        /// Coroutine for fade in
        /// </summary>
        /// <seealso cref="EnterFade(float)"/>
        private IEnumerator IEnterFade(float duration)
        {
            //Set state
            isVisible = true;
            isEntered = true;

            //Loop frame by frame
            float t = 0;
            while (t <= duration)
            {
                //Increment timer
                t += Time.deltaTime;
                //Calculate transparency from time
                float a = Mathf.Clamp01(t / duration);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
                yield return null;//wait for next frame
            }
            spriteRenderer.color = Color.white;//snap to correct value at end
            fadeCoroutine = null;//mark as finished
        }

        /// <summary>
        /// Coroutine for fade out
        /// </summary>
        /// <seealso cref="ExitFade(float)"/>
        private IEnumerator IExitFade(float duration)
        {
            //Set State
            isEntered = false;

            //Loop frame by frame
            float t = duration;
            while (t >= 0)
            {
                //Increment timer
                t -= Time.deltaTime;
                //Calculate transparency from time
                float a = Mathf.Clamp01(t / duration);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
                yield return null;
            }

            spriteRenderer.color = CLEAR;//snap to correct value at end
            fadeCoroutine = null;//mark as finished
            isVisible = false;
        }

    }
}