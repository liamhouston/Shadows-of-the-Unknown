using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownDoorBehaviour : MonoBehaviour
{
    // Types of Door
    public enum Condition {KillAllEnemies, Key, Puzzle};
    public Condition openCondition;

    [Header("Key Options")]
    public float openRadius = 1f;

    // internal variables
    private int _remainingEnemies = 0;

    // the player
    private Rigidbody2D player;
    private TopDownPlayerBehaviour playerScript;
    private TopDownSokobanBehaviour sokobanScript;

    // Start is called before the first frame update
    void Start()
    {
        // if we've already been opened in a past life, don't open again!
        if (PlayerPrefs.HasKey(gameObject.scene.name + gameObject.name) && SceneManager.sceneCount != 1){
            Destroy(gameObject);
        }

        if (openCondition == Condition.KillAllEnemies){
            _remainingEnemies = GameObject.FindObjectsOfType(typeof(TopDownEnemyBehaviour)).Length;
        }

        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        playerScript = (TopDownPlayerBehaviour)player.gameObject.GetComponent(typeof(TopDownPlayerBehaviour));

        // Manually getting sokoban script. Might be better to get the player and call sokoban script when needed
        sokobanScript = (TopDownSokobanBehaviour)player.gameObject.GetComponent(typeof(TopDownSokobanBehaviour));

        
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

        else if (openCondition == Condition.Puzzle)
        {
            if (sokobanScript.puzzleComplete)
            {
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
        // if we're opening with a key, decrement key from player
        if (openCondition == Condition.Key){
            playerScript.unlockDoor(true);
        }
        else{
            playerScript.unlockDoor(false);
        }

        // log that this door has been opened
        PlayerPrefs.SetInt(gameObject.scene.name + gameObject.name, 1);

        Destroy(gameObject);
    }
}
