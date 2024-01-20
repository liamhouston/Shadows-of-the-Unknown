using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Narrative
{
    public class DialogueSounds : MonoBehaviour
    {
        //Audio Source for playing sounds
        //  Singleton pattern
        private static AudioSource _audioSource;
        public static AudioSource AudioSource { get { return _audioSource; } }

        //Singleton pattern
        private static DialogueSounds _instance;
        public static DialogueSounds Instance { get { return _instance; } }
        /// <summary>
        /// Constructor, defines singeton instance.
        /// </summary>
        public DialogueSounds()
        {
            _instance = this;
        }

        /// <summary>
        /// Simple struct because you can't serialize a dictionary by default.
        /// </summary>
        [System.Serializable]
        struct SoundItem
        {
            [SerializeField] public string id;
            [SerializeField] public AudioClip clip;
        }

        [SerializeField] private List<SoundItem> soundsByID = new List<SoundItem>(); //list for serializing

        private Dictionary<string, AudioClip> soundsDictionary = new Dictionary<string, AudioClip>(); //actual portrait dictionary

        // Awake is called before everything
        void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            BuildSoundDictionary();
        }

        /// <summary>
        /// Converts the serializable list into a dictionary.
        /// </summary>
        private void BuildSoundDictionary()
        {
            foreach (SoundItem item in soundsByID)
            {
                //Catch duplicates
                if (soundsDictionary.ContainsKey(item.id)){
                    Debug.LogWarning("Duplicate sound id: " + item.id);
                }
                //Add sound name in uppercase for non-case-sensitivity
                soundsDictionary.Add(item.id.ToUpper().Trim(), item.clip);
            }
        }

        /// <summary>
            /// Gets the portrait sprite. Throws KeyNotFoundException if the portrait doesn't exist
            /// </summary>
            /// <param name="id">Name of the portrait, non-case-sesitive</param>
            /// <returns>The sprite object of given id</returns>
            public static AudioClip GetSound(string id)
            {
                string nonCaseSensitiveID = id.ToUpper().Trim();
                if (!Instance.soundsDictionary.ContainsKey(nonCaseSensitiveID))
                {
                    throw new KeyNotFoundException("Sound does not exist with id " + id);
                }
                return Instance.soundsDictionary[nonCaseSensitiveID];
            }

            /// <summary>
            /// Returns whether the database contains a sound matching the given id.
            /// </summary>
            public static bool IsValidSoundName(string name)
            {
                return Instance.soundsDictionary.ContainsKey(name.ToUpper().Trim());
            }
    }
}
