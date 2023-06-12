using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEnemyBehaviour : TopDownEntityBehaviour
{

    // public variables
    public float moveTime = 1.5f;
    public float damageRadius = 1f;

    // internal variables
    private float _remainingTime;

    // the player & doors
    private Rigidbody2D player;
    private TopDownDoorBehaviour[] doors = {};

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        _remainingTime = moveTime;
        _currDir = pickDirection();
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        doors = (TopDownDoorBehaviour[])GameObject.FindObjectsOfType(typeof(TopDownDoorBehaviour));
    }
    
    // Update is called once per frame
    override public void FixedUpdate()
    {
        base.FixedUpdate();

        // handle movement, change directions after elapsed time
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0){
            _currDir = pickDirection();
            _remainingTime = moveTime;
        }

        // handle player collision
        if (Mathf.Abs(Vector2.Distance(player.position, transform.position)) <= damageRadius){
            player.gameObject.SendMessage("takeDamage");
        }
    }

    // uses helper function to convert current direction into movement vector
    override public Vector2 getMovement(){
        return dirToVec();
    }

    // update our direction with a random new one
    Direction pickDirection(){
        int i = 0; // loop limiter for the uh. 0.00390625 chance 
        
        // pick a new direction that wasn't our old one
        Direction newDir = (Direction)Random.Range(0, 4);
        while (newDir == _currDir && i < 100){
            newDir = (Direction)Random.Range(0, 4);
            i++;
        }
        return newDir;
    }
    
    // on death, kill!!
    override public void handleDeath(){
        // sending a message to All Doors. I am dead
        for (int i = 0; i < doors.Length; i++){
            if (doors[i] != null){
                doors[i].SendMessage("enemyDeath");
            }
        }
        Destroy(gameObject);
    }
}
