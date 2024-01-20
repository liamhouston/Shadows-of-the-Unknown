using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Narrative
{
    /// <summary>
    /// A component that recieves clicks and broadcasts a callback.
    /// Can set reciever methods in the editor.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ClickableObject : MonoBehaviour
    {
        //Event Callbacks
        public UnityEvent onClick;


        /// <summary>
        /// Called when this object is clicked on
        /// </summary>
        private void OnMouseDown()
        {
            onClick.Invoke();
        }
    }
}