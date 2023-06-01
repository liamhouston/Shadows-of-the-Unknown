using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownKeyBehaviour : MonoBehaviour
{

    // public variables
    public float collectRadius = 0.5f;

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
        if (Mathf.Abs(Vector2.Distance(player.position, transform.position)) <= collectRadius){
            Collect();
        }
    }

    void Collect(){
        // todo: play sound / animation?
        playerScript.collectKey();
        Destroy(gameObject);
    }
}
