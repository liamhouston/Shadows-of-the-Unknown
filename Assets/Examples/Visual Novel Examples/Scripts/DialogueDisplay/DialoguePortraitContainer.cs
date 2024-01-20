using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Narrative {
    /// <summary>
    /// Container class for portrait references to be retrieved by string ids.
    /// </summary>
    public class DialoguePortraitContainer : MonoBehaviour
    {
        //Singleton pattern
        private static DialoguePortraitContainer _instance;
        public static DialoguePortraitContainer Instance { get { return _instance; } }
        /// <summary>
        /// Constructor, defines singeton instance.
        /// </summary>
        public DialoguePortraitContainer()
        {
            _instance = this;
        }



        /// <summary>
        /// Simple struct because you can't serialize a dictionary by default.
        /// </summary>
        [System.Serializable]
        struct PortraitItem
        {
            [SerializeField] public string id;
            [SerializeField] public Sprite sprite;
        }

        [SerializeField] private List<PortraitItem> portraitsByID = new List<PortraitItem>(); //list for serializing

        private Dictionary<string, Sprite> portraitsDictionary = new Dictionary<string, Sprite>(); //actual portrait dictionary

        // Awake is called before everything
        void Awake()
        {
            BuildPortraitDictionary();
        }


        /// <summary>
        /// Converts the serializable list into a dictionary.
        /// </summary>
        private void BuildPortraitDictionary()
        {
            foreach (PortraitItem item in portraitsByID)
            {
                //Catch duplicates
                if (portraitsDictionary.ContainsKey(item.id)){
                    Debug.LogWarning("Duplicate portrait id: " + item.id);
                }
                //Add portrait name in uppercase for non-case-sensitivity
                portraitsDictionary.Add(item.id.ToUpper().Trim(), item.sprite);
            }
        }

        /// <summary>
        /// Gets the portrait sprite. Throws KeyNotFoundException if the portrait doesn't exist
        /// </summary>
        /// <param name="id">Name of the portrait, non-case-sesitive</param>
        /// <returns>The sprite object of given id</returns>
        public static Sprite GetPortrait(string id)
        {
            string nonCaseSensitiveID = id.ToUpper().Trim();
            if (!Instance.portraitsDictionary.ContainsKey(nonCaseSensitiveID))
            {
                throw new KeyNotFoundException("Portrait does not exist with id " + id);
            }
            return Instance.portraitsDictionary[nonCaseSensitiveID];
        }

        /// <summary>
        /// Returns whether the database contains a portrait matching the given id.
        /// </summary>
        public static bool IsValidPortraitName(string name)
        {
            return Instance.portraitsDictionary.ContainsKey(name.ToUpper().Trim());
        }
    }
}