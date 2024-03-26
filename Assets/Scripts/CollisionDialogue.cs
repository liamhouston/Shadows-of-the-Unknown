using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDialogue : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryGetComponent(out PlayerBarks playerBarks);
            DialogueManager.Instance.playBlockingDialogue("Jay", playerBarks.barkList);
        }
    }
}
