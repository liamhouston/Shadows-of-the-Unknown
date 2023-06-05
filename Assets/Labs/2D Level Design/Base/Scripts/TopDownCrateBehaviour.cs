using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCrateBehaviour : MonoBehaviour
{
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
        
    }

    //Just overlapped a collider 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Don't hardcode the name and potentially use tags here
        if (collision.gameObject.name == "Crate")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
