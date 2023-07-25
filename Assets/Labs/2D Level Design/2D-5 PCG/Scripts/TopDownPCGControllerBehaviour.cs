using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TopDownPCGControllerBehaviour : MonoBehaviour
{
    private bool _generating = true;
    private AsyncOperation deletion = null;
    public GameObject player;

    private GameObject connectionController;
    private Transform[] connections = {null, null, null, null}; // 0 = N, 1 = E, 2 = S, 3 = W
    
    private float _playerSpawnNudge = 2f;

    private int _scenesToLoad = 0;


    // Start is called before the first frame update
    void Start()
    {
        // reset health and key info
        PlayerPrefs.DeleteAll();

        // grab the connection controller for setting up player positioning
        connectionController = GameObject.Find("ConnectionController");
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

        // load all chunks to pick between them
        DirectoryInfo dir = new DirectoryInfo("Assets/Labs/2D Level Design/2D-5 PCG/Scenes/Components");
        FileInfo[] fileInfo = dir.GetFiles("*?.unity");
        for (int i = 0; i < fileInfo.Length; i++){
            SceneManager.LoadScene(Path.GetFileNameWithoutExtension(fileInfo[i].Name), LoadSceneMode.Additive);
        }
        _scenesToLoad = fileInfo.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // all chunks are loaded and we haven't generated a level yet, so it's time to generate
        if (_generating){
            if (SceneManager.sceneCount == _scenesToLoad + 1 && SceneManager.GetSceneAt(SceneManager.sceneCount - 1).isLoaded){
                _generating = false;
                bool done = false;
                int leftInd = -1;
                int rightInd = -1;
                while (!done){
                    leftInd = Random.Range(1, SceneManager.sceneCount);
                    rightInd = Random.Range(1, SceneManager.sceneCount);
                    
                    List<GameObject> leftSceneObjects = new List<GameObject>();
                    List<GameObject> rightSceneObjects = new List<GameObject>();
                    SceneManager.GetSceneAt(leftInd).GetRootGameObjects(leftSceneObjects);
                    SceneManager.GetSceneAt(rightInd).GetRootGameObjects(rightSceneObjects);

                    done = true;

                    TopDownPCGChunkControllerBehaviour leftPCGController = null;
                    TopDownPCGChunkControllerBehaviour rightPCGController = null;

                    // check that these chunks are allowed to be left and right respectively, and that they aren't the same chunk
                    for (int i = 0; i < leftSceneObjects.Count; i++){
                        if (leftSceneObjects[i].tag == "PCGController"){
                            leftPCGController = leftSceneObjects[i].GetComponent<TopDownPCGChunkControllerBehaviour>();
                            if (!leftPCGController.getLeftOkay()){
                                done = false;
                            }
                        }
                        
                    }

                    for (int i = 0; i < rightSceneObjects.Count; i++){
                        if (rightSceneObjects[i].tag == "PCGController"){
                            rightPCGController = rightSceneObjects[i].GetComponent<TopDownPCGChunkControllerBehaviour>();
                            if (!rightSceneObjects[i].GetComponent<TopDownPCGChunkControllerBehaviour>().getRightOkay()){
                                done = false;
                            }
                        }
                    }

                    if (leftInd == rightInd){
                        done = false;
                    }

                    // handle flips, if they are randomly chosen to occur
                    if (done){
                        if (leftPCGController.getVerticalFlipOkay() && Random.Range(0, 2) == 1){
                            
                        }
                    }
                }

                


                // merge the scenes from the chosen left and right index

                for (int i = SceneManager.sceneCount - 1; i > 0; i--){
                    List<GameObject> sceneObjects = new List<GameObject>();
                    SceneManager.GetSceneAt(i).GetRootGameObjects(sceneObjects);
                    for (int j = 0; j < sceneObjects.Count; j++){
                        if (sceneObjects[j].tag == "MainCamera"){
                            GameObject.Destroy(sceneObjects[j]);
                        }
                        else if (sceneObjects[j].tag == "Player"){
                            // ensure the player isn't destroyed in the object moving process
                            SceneManager.MoveGameObjectToScene(sceneObjects[j], SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                            DontDestroyOnLoad(sceneObjects[j]);

                            // randomly set the position of the player to one of the four entry points
                            int dir = Random.Range(0, 4);
                            Vector3 newPlayerPos = Vector3.zero;
                            switch (dir){
                                case 0:
                                    newPlayerPos = connections[0].transform.position;
                                    newPlayerPos.y -= _playerSpawnNudge;
                                    print("position 0");
                                    break;
                                case 1:
                                    newPlayerPos = connections[1].transform.position;
                                    newPlayerPos.x -= _playerSpawnNudge;
                                    print("position 1");
                                    break;
                                case 2:
                                    newPlayerPos = connections[2].transform.position;
                                    newPlayerPos.y += _playerSpawnNudge;
                                    print("position 2");
                                    break;
                                case 3:
                                    newPlayerPos = connections[3].transform.position;
                                    newPlayerPos.x += _playerSpawnNudge;
                                    print("position 3");
                                    break;
                                default:
                                    break;
                            }
                            sceneObjects[j].transform.position = newPlayerPos;
                        }
                        else if (i == rightInd){
                            Vector3 newPos = sceneObjects[j].transform.position;
                            newPos.x += 8;
                            sceneObjects[j].transform.position = newPos;
                            }
                        }
                        if (i == leftInd || i == rightInd){
                            SceneManager.MergeScenes(SceneManager.GetSceneAt(i), SceneManager.GetSceneAt(0));
                        }
                }

            }
        }
        else{
            if (SceneManager.sceneCount > 1){
                if (deletion == null || deletion.isDone){
                    List<GameObject> sceneObjects = new List<GameObject>();
                    SceneManager.GetSceneAt(SceneManager.sceneCount - 1).GetRootGameObjects(sceneObjects);
                    deletion = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                }
            }
            
        }
        
        
    }
}
