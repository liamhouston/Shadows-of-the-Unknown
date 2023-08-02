using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        // if we've already been collected in a past life, don't spawn again!
        if (PlayerPrefs.HasKey(gameObject.scene.name + gameObject.name) && SceneManager.sceneCount != 1){
            Destroy(gameObject);
        }

        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        playerScript = (TopDownPlayerBehaviour)player.gameObject.GetComponent(typeof(TopDownPlayerBehaviour));
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && Mathf.Abs(Vector2.Distance(player.position, transform.position)) <= collectRadius){
            Collect();
        }
    }

    void Collect(){
        // todo: play sound / animation?
        playerScript.collectKey();
        // log that we've been collected
        PlayerPrefs.SetInt(gameObject.scene.name + gameObject.name, 1);
        Destroy(gameObject);
    }
}
