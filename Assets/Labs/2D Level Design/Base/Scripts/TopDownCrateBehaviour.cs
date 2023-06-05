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

        VerifyCrates();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Just overlapped a collider 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crate")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void VerifyCrates()
    {
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");

        if (crates.Length != goals.Length)
        {
            Debug.LogError(("Number of crates and goals for Sokoban are not equal. \nNumber of Crates: " + crates.Length + " Number of Goals: " + goals.Length));
        }
    }
}
