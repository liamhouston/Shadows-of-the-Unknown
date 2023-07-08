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
        public const int NUMFLAGS = 8;//Predefine the number of binary flags

        private static DialogueFlags _instance;//Singleton instance reference

        //Flag data
        [SerializeField] private bool[] flags = new bool[NUMFLAGS];

        /// <summary>
        /// Class constructor. Assigns itself as th esingleton instance.
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
        /// <param name="num">The flag ID number</param>
        /// <param name="value">Value to set the flag to.</param>
        public static void SetFlag(int num, bool value)
        {
            _instance.flags[num] = value;
        }

        /// <summary>
        /// Obtains the flag value. Doesn't error check, leave to users to catch exceptions.
        /// </summary>
        /// <param name="flagNum">Which flag to retrieve.</param>
        /// <returns>The contents of the given flag.</returns>
        public static bool GetFlagValue(int flagNum)
        {
            return _instance.flags[flagNum];
        }
    }
}