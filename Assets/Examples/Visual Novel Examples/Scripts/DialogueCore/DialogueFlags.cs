using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    /// <summary>
    /// A container class for persistent dialogue flag data using a singleton pattern.
    /// Attach to a persistent GameObject
    /// </summary>
    public class DialogueFlags : MonoBehaviour
    {
        private static DialogueFlags _instance;//Singleton instance reference

        //Flag data
        [SerializeField] private Dictionary<string, bool> flags = new Dictionary<string, bool>();

        /// <summary>
        /// Class constructor. Assigns itself as the singleton instance.
        /// </summary>
        public DialogueFlags()
        {
            if (_instance)
            {
                Debug.LogWarning("Duplicate DialogueFlags object created, may have undefined behaviour.");
            }
            _instance = this;
        }


        /// Static fuctions to shorten calls to the singleton instance
        /// ie. DialogueFlags.SetFlag(...) is better than DialogueFlags.Instance.SetFlag(...)

        /// <summary>
        /// Sets the flag.
        /// </summary>
        /// <param name="flag">The flag ID</param>
        /// <param name="value">Value to set the flag to.</param>
        public static void SetFlag(string flag, bool value)
        {
            _instance.flags[flag] = value;
        }

        /// <summary>
        /// Obtains the flag value. Doesn't error check, leave to users to catch exceptions.
        /// </summary>
        /// <param name="flag">Which flag to retrieve.</param>
        /// <returns>The contents of the given flag.</returns>
        public static bool GetFlagValue(string flag)
        {
            if (_instance.flags!=null){
                if (_instance.flags.ContainsKey(flag)){
                    return _instance.flags[flag];    
                }
            }
            return false;
            
        }
    }
}