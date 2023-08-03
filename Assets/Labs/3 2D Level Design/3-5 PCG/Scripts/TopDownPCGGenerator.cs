using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class TopDownPCGGenerator : MonoBehaviour
{   
    // game components
    public TileBase groundTile;
    public TileBase wallTile;
    public TileBase iceTile;
    public TileBase pitTile;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject key;
    public GameObject door;
    public GameObject goal;
    public GameObject crate;

    private List<string[]> _componentStrings = new List<string[]>();
    private Tilemap tiles;

    

    // Start is called before the first frame update
    void Start()
    {
        tiles = GameObject.Find("Grid").GetComponentInChildren<Tilemap>();
        foreach (string f in Directory.EnumerateFiles("Assets/Labs/3 2D Level Design/3-5 PCG/Scenes/Components/", "*.txt")){
            StreamReader reader = new StreamReader(f);
            _componentStrings.Add(reader.ReadToEnd().Split('\n'));
        }

        buildScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void buildScene(){
        // go through each component and select four to build the scene, respecting their limitations
        List<List<int>> allowedChunks = new List<List<int>>();
        
        for (int i = 0; i < 4; i++){
            allowedChunks.Add(new List<int>());
        }

        for (int i = 0; i < _componentStrings.Count; i++){
            
            for (int j = 0; j < 4; j++){
                // thank you C#
                if (_componentStrings[i][0].Split(' ')[j].TrimEnd('\r') == "True"){
                    allowedChunks[j].Add(i);
                }
            }
        }

        for (int i = 0; i < 4; i++){
            string o = "";
            for (int j = 0; j < allowedChunks[i].Count; j++){
                o += allowedChunks[i][j].ToString() + " ";
            }
            print(o);
        }

        for (int i = 0; i < 4; i++){
            if (allowedChunks[i].Count == 0){
                print("0 possibilities for a corner! Please add at least one component which works for each corner.");
            }
        }

        // top left, top right, bottom left, bottom right
        List<int> chosenChunks = new List<int>();
        for (int timeout = 0; timeout < 10000; timeout++){
            bool success = true;
            for (int i = 0; i < 4; i++){
                int possibleChoice = allowedChunks[i][Random.Range(0, allowedChunks[i].Count)];
                chosenChunks.Add(possibleChoice);
            }

            for (int i = 0; i < 4; i++){
                for (int j = 0; j < 4; j++){
                    if (chosenChunks[i] == chosenChunks[j] && i != j){
                        success = false;
                    }
                }
            }

            if (success){
                break;
            }

            if (timeout == 9999){
                print("timed out! going with a match with duplicates");
            }
            else{
                chosenChunks.Clear();
            }
            
        }

        string oo = "";
        for (int i = 0; i < 4; i++){
            oo += chosenChunks[i].ToString() + " ";
        }
        print(oo);

        // with our chosen chunks, build the level!
        for (int i = 0; i < 4; i++){
            addComponentsToScene(_componentStrings[chosenChunks[i]], i);
        }
        return;
    }

    void addComponentsToScene(string[] componentString, int side){
        // set the bounds according to what side the component will be on
        int x1 = -1;
        int x2 = -1;
        int y1 = -1;
        int y2 = -1;
        if (side == 0){
            x1 = -9;
            x2 = -1;
            y1 = 4;
            y2 = 0;
        }
        else if (side == 1){
            x1 = 0;
            x2 = 8;
            y1 = 4;
            y2 = 0;
        }
        else if (side == 2){
            x1 = -9;
            x2 = -1;
            y1 = -1;
            y2 = -5;
        }
        else if (side == 3){
            x1 = 0;
            x2 = 8;
            y1 = -1;
            y2 = -5;
        }

        
        // set the tiles accordingly
        int currY = y1;
        for (int i = 0; i < 5; i++){
            int currX = x1;
            for (int j = 0; j < 9; j++){
                Vector3Int tilePos = new Vector3Int(currX, currY, 0);
                switch (componentString[i + 1][j]){
                    case 'w':
                        tiles.SetTile(tilePos, wallTile);
                        break;
                    case '.':
                        tiles.SetTile(tilePos, groundTile);
                        break;
                    case 'i':
                        tiles.SetTile(tilePos, iceTile);
                        break;
                    case 'p':
                        tiles.SetTile(tilePos, pitTile);
                        break;
                    default:
                        break;
                }
                currX++;
            }
            currY--;
        }

        // now, place each of the objects in the scene
        // x pos is from left -> right, y pos is from top -> down

        for (int i = 6; i < componentString.Length; i++){
            string[] objectInfo = componentString[i].TrimEnd('\r').Split(' ');
            if (objectInfo.Length < 4){
                continue;
            }
            Vector3 objectPosition = new Vector3(x1 + float.Parse(objectInfo[1]), y1 + float.Parse(objectInfo[2]), 0);
            switch (objectInfo[0]){
                case "1":
                    GameObject e1 = Instantiate(enemy1) as GameObject;
                    e1.transform.position = objectPosition;
                    break;
                case "2":
                    GameObject e2 = Instantiate(enemy2) as GameObject;
                    e2.transform.position = objectPosition;
                    break;
                case "c":
                    GameObject c = Instantiate(crate) as GameObject;
                    c.transform.position = objectPosition;
                    break;
                case "k":
                    GameObject k = Instantiate(key) as GameObject;
                    k.transform.position = objectPosition;
                    break;
                case "g":
                    GameObject g = Instantiate(goal) as GameObject;
                    g.transform.position = objectPosition;
                    break;
                case "d":
                    GameObject d = Instantiate(door) as GameObject;
                    d.transform.position = objectPosition;
                    TopDownDoorBehaviour doorScript = d.GetComponent<TopDownDoorBehaviour>();
                    doorScript.openCondition = (TopDownDoorBehaviour.Condition)int.Parse(objectInfo[3]);
                    break;
                default:
                    break;
            }
        }
    }

}
