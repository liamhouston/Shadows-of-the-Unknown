using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Narrative
{
    public class DialogueChoices : MonoBehaviour
    {
        public UnityEvent onChoiceSelected;

        [SerializeField] private Animator animator;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void ChoiceSelected(int choice)
        {
            print("Chose:" + choice);
            onChoiceSelected.Invoke();
        }
    }
}