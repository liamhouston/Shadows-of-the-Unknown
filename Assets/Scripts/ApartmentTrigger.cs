using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentTrigger : MonoBehaviour
{
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
