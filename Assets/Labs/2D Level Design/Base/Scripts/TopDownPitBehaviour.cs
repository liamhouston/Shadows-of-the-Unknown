using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPitBehaviour : MonoBehaviour
{
    // public variables
    public float fallRadius = 0.9f;

    // the player
    private Rigidbody2D player;
    private TopDownPlayerBehaviour playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        playerScript = (TopDownPlayerBehaviour)player.gameObject.GetComponent(typeof(TopDownPlayerBehaviour));
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) <= fallRadius && Mathf.Abs(player.position.y - transform.position.y) <= fallRadius){
            Fall();
        }
    }

    void Fall(){
        playerScript.fallInPit(transform.position);
    }
}
