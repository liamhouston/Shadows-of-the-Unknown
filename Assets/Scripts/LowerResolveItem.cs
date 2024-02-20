using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerResolveItem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameController gameController;
    private bool playerIsNearby = false;
  
    private bool playerSeenItem = false;


    private bool readyDamage = true;
    public float damageDelay = 1f; // wait half a second before taking more damage to resolve meter
    public int damageAmount = 3; // amount of damage to resolve meter every damageDelay seconds


    // Start is called before the first frame update
    void Start(){
        gameController = GameController.Instance;
        Debug.Assert(gameController != null, "GameController must exist in the scene");
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "SpriteRenderer must exist on this object");

    }

    // Update is called once per frame
    void Update(){
        if (playerIsNearby && readyDamage){
            readyDamage = false;
            // change the resolve meter
            gameController.ChangeResolve(-damageAmount);
            
            // shake camera with amplitude 3, frequency 5, and duration 2 seconds.
            gameController.StartShake(3,5,1);

            // play audio depending if this is the first time the player has seen the item
            if (playerSeenItem == false) {
                playerSeenItem = true;
                AudioController.Instance.PlayFirstDamageSound();
            } else {
                AudioController.Instance.PlayDefaultDamageSound();
            }

            // wait until we can take damage again
            StartCoroutine(WaitForDamage());
        }
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

    private IEnumerator WaitForDamage() {
        yield return new WaitForSeconds(damageDelay);
        readyDamage = true;
    }
}
