using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerSequence : MonoBehaviour
{
    public enum Direction {North, East, South, West};

    // the names of the scenes in the sequence
    public string firstSceneName;
    public string secondSceneName;
    public string thirdSceneName;

    public Direction spawnLocation; 
    public Direction connectionFromOneToTwo;
    public Direction connectionFromTwoToThree;

    // internal variables
    private int currentRoom = 0;
    private bool init = true;
    private int frames = 0;
    private float warpRadius = 0.5f;
    private float playerSpawnNudge = 1f;

    // the doors of the room
    private GameObject connectionController;
    private Transform[] connections = {null, null, null, null}; // 0 = N, 1 = E, 2 = S, 3 = W
    AsyncOperation unloaded = null;

    // scene traversal information
    private string next = "";
    
    // the player
    private Rigidbody2D player;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll(); // wiping health and keys back to their defaults
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // wait two frames to do the initialization due to bugs with loading objects!
        if (init){
            if (frames > 0){
                onInit();
            }
            frames++;
        }
        // otherwise, check if we're waiting to transition to a new scene
        else if (unloaded != null){
            if (unloaded.isDone){
                SceneManager.LoadScene(next, LoadSceneMode.Additive);
                init = true;
                unloaded = null;
            }
        }
        // handle doors of current scene
        else{
            Scene currentScene = SceneManager.GetSceneAt(1);
            for (int i = 0; i < connections.Length; i++){
                if (connections[i] != null && Mathf.Abs(Vector2.Distance(player.position, connections[i].position)) < warpRadius){
                    handleSwitch(currentScene, (Direction)i);
                }
            }
        }
    }

    // initialize proper connections once scene is fully loaded
    void onInit(){
        init = false;

        Scene gameplayScene = SceneManager.GetSceneAt(1);
        List<GameObject> test = new List<GameObject>();
        gameplayScene.GetRootGameObjects(test);
        for (int i = 0; i < test.Count; i++){
            if (test[i].name == "ConnectionController"){
                connectionController = test[i];
            }
        }

        // grab the connections for the current room
        foreach (Transform child in connectionController.transform){
            switch (child.name){
                case "North":
                    connections[0] = child;
                    break;
                case "East":
                    connections[1] = child;
                    break;
                case "South":
                    connections[2] = child;
                    break;
                case "West":
                    connections[3] = child;
                    break;
                default:
                    break;
            }
        }

        // logging connections
        for (int i = 0; i < connections.Length; i++){
            Debug.Log(connections[i]);
        }

        // put the player in the appropriate location
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        Vector3 newPlayerPos = connections[(int)spawnLocation].position;

        // nudge the position so we don't immediately warp again
        switch (spawnLocation){
            case Direction.North:
                newPlayerPos.y -= playerSpawnNudge;
                break;
            case Direction.South:
                newPlayerPos.y += playerSpawnNudge;
                break;
            case Direction.East:
                newPlayerPos.x -= playerSpawnNudge;
                break;
            case Direction.West:
                newPlayerPos.x += playerSpawnNudge;
                break;
            default:
                break;
        }
        player.position = newPlayerPos;
    }

    // for sequence, switch is purely handled based on sequence of three levels. for PCG, there will be an internal "map" this is based on
    void handleSwitch(Scene currentScene, Direction door){ 
        
        if (currentScene.name == firstSceneName){
            if (door != connectionFromOneToTwo){ return; }
            next = secondSceneName;
            spawnLocation = getInverseDoor(connectionFromOneToTwo);
        }
        else if (currentScene.name == secondSceneName){
            // N <-> S, E <-> W
            Direction door1 = getInverseDoor(connectionFromOneToTwo);
            Direction door2 = connectionFromTwoToThree;
            if (door == door1){
                next = firstSceneName;
                spawnLocation = connectionFromOneToTwo;
            }
            else if (door == door2){
                next = thirdSceneName;
                spawnLocation = getInverseDoor(door2);
            }
            else{
                return;
            }
        }
        else if (currentScene.name == thirdSceneName){
            if (door != getInverseDoor(connectionFromTwoToThree)){ return; }
            next = secondSceneName;
            spawnLocation = connectionFromTwoToThree;
        }

        // unload the current scene
        unloaded = SceneManager.UnloadSceneAsync(currentScene);    
        return; 
    }

    // helper function to get opposite door
    Direction getInverseDoor(Direction door){
        return (Direction)(((int)door + 2) % 4); 
    }
}
