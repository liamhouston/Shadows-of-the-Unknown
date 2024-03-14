using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentTrigger : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryGetComponent(out PlayerBarks playerbarks);
            if (playerbarks)
            {
                playerbarks.ApartmentDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TryGetComponent(out Collider collider);
        if (collider)
        {
            collider.enabled = false;
        }
    }
}