using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCrateBehaviour : MonoBehaviour
{

    private TopDownSokobanBehaviour sokobanScript;
    private Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        sokobanScript = (TopDownSokobanBehaviour)player.gameObject.GetComponent(typeof(TopDownSokobanBehaviour));
    }


    //Just overlapped a collider 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            sokobanScript.DecrementGoals();
            sokobanScript.DecrementCrates();

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
