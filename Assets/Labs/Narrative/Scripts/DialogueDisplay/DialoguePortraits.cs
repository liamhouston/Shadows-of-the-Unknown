using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    /// <summary>
    /// Recieves commands from the dialogue sequencer to control portraits.
    /// </summary>
    public class DialoguePortraits : MonoBehaviour
    {

        [SerializeField] DialoguePortrait portrait;



        /// <summary>
        /// Sets the sprite of the portrait.
        /// If the portrait isn't sowing it will fade in.
        /// </summary>
        /// <param name="sprite">Sprite to set the portrait to</param>
        public void SetPortraitSprite(Sprite sprite)
        {
            portrait.SetSprite(sprite);

            //Fade if not already there
            if (!portrait.IsEntered)
            {
                portrait.EnterFade(0.5f);
            }
        }

        /// <summary>
        /// Fades out all portraits.
        /// </summary>
        public void ClosePortraits()
        {
            if (portrait.IsVisible)
            {
                portrait.ExitFade(0.5f);
            }
        }
    }
}