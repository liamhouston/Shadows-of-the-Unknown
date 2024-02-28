using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorClue : MonoBehaviour
{
    private bool playerIsNearby = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) {
            playerIsNearby = false;
        }    
    }
}
