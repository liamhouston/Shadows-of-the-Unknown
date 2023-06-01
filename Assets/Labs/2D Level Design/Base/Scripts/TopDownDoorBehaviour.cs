using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownDoorBehaviour : MonoBehaviour
{

    // Types of Door
    public enum Condition {KillAllEnemies, Key};
    public Condition openCondition;

    [Header("Key Options")]
    public float openRadius = 1f;
    

    // internal variables
    private int _remainingEnemies = 0;

    // the player
    private Rigidbody2D player;
    private TopDownPlayerBehaviour playerScript;

    // Start is called before the first frame update
    void Start()
    {
        if (openCondition == Condition.KillAllEnemies){
            _remainingEnemies = GameObject.FindObjectsOfType(typeof(TopDownEnemyBehaviour)).Length;
        }

        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        playerScript = (TopDownPlayerBehaviour)player.gameObject.GetComponent(typeof(TopDownPlayerBehaviour));
    }

    // Update is called once per frame
    void Update()
    {
        if (openCondition == Condition.KillAllEnemies){
            if (_remainingEnemies <= 0){
                Unlock();
            }
        }

        else if (openCondition == Condition.Key){
            if (playerScript.getKeys() > 0 && Mathf.Abs(Vector2.Distance(player.position, transform.position)) <= openRadius){
                Unlock();
            }
        }
        
    }

    void enemyDeath(){
        if (openCondition == Condition.KillAllEnemies){
            _remainingEnemies--;
        }
    }

    void Unlock(){
        // todo: play sound, play animation

        // if we're opening with a key, decrement key from player
        if (openCondition == Condition.Key){
            playerScript.unlockDoor();
        }

        Destroy(gameObject);
    }
}
