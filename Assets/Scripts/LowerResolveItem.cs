using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerResolveItem : MonoBehaviour
{
    [Header ("Damage Info")]
    public float delayOnFirstSight = 1f; // how long the player can look at item before taking damage the first time. (allow the player to actually realize what's damaging them before damaging)
    public float delayBetweenDamages = 1f; // wait before taking more damage to resolve meter
    public int damageAmount = 3; // amount of damage to resolve meter every delayBetweenDamages seconds


    private SpriteRenderer spriteRenderer;
    private GameController gameController;

    // bool indicators
    private bool playerIsNearby = false;
    private bool playerSeenItem = false;
    private bool readyDamage = true;


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
            // allow the player time to see the item for the first time
            if (playerSeenItem == false) {
                StartCoroutine(WaitForFirstSight());
            } 
            else {
                SoundManager.Instance.PlaySound2D("Heartbeating");
            }

            if (playerSeenItem){                
                // change the resolve meter
                gameController.ChangeResolve(-damageAmount);
                // shake camera with amplitude 3, frequency 5, and duration 2 seconds.
                gameController.StartShake(3,5,1);
                // wait until we can take damage again
                readyDamage = false;
                StartCoroutine(WaitForDamage());    
            }
            
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
        yield return new WaitForSeconds(delayBetweenDamages);
        readyDamage = true;
    }

    private IEnumerator WaitForFirstSight() {
        yield return new WaitForSeconds(delayOnFirstSight);
        if (playerIsNearby){
            playerSeenItem = true;
            SoundManager.Instance.PlaySound2D("Heartbeating");
        }
    }
}
