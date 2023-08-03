using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class TopDownComponentScript : MonoBehaviour
{
    public bool topLeftOkay;
    public bool topRightOkay;
    public bool bottomLeftOkay;
    public bool bottomRightOkay;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveToFile(string filename){
        StreamWriter writer = new StreamWriter("Assets/Labs/3 2D Level Design/3-5 PCG/Scenes/Components/" + filename);

        // first row, 4 bools denoting the 4 public variables
        writer.WriteLine(topLeftOkay.ToString() + " " + topRightOkay.ToString() + " " + bottomLeftOkay.ToString() + " " + bottomRightOkay.ToString());
        
        // next, the 5x9 grid of tiles
        Tilemap tiles = gameObject.GetComponentInChildren<Tilemap>();
        
        // 0, 0 is top left, and unity has up as positive, so negative
        for (int i = 0; i > -5; i--){
            string rowString = "";
            for (int j = 0; j < 9; j++){
                Vector3Int tilePosition = new Vector3Int(j, i, 0);
                if (tiles.HasTile(tilePosition)){
                    switch (tiles.GetTile(tilePosition).name){
                        case "CaveRuleTile":
                            rowString += "w";
                            break;
                        case "GroundTile":
                            rowString += ".";
                            break;
                        case "IceRuleTile":
                            rowString += "i";
                            break;
                        case "PitRuleTile":
                            rowString += "p";
                            break;
                        default:
                            rowString += "h";
                            break;
                    }
                }
            }
            writer.WriteLine(rowString);
        }

        // n x 4 list of game objects in the scene
        foreach (Transform child in transform){
            string line = "";
            switch (child.tag){
                case "Enemy":
                    // enemy 2
                    if (child.gameObject.GetComponent<TopDownEnemy2Behaviour>() != null){
                        line = "2 " + child.position.x + " " + child.position.y + " 0";
                    }
                    // enemy 1
                    else{
                        line = "1 " + child.position.x + " " + child.position.y + " 0";
                    }
                    break;
    
                case "Goal":
                    line = "g " + child.position.x + " " + child.position.y + " 0";
                    break;

                case "Crate":
                    line = "c " + child.position.x + " " + child.position.y + " 0";
                    break;
                
                default:
                    // key
                    if (child.gameObject.GetComponent<TopDownKeyBehaviour>() != null){
                        line = "k " + child.position.x + " " + child.position.y + " 0";
                    }
                    // door
                    else if (child.gameObject.GetComponent<TopDownDoorBehaviour>() != null){
                        int openCond = (int)child.gameObject.GetComponent<TopDownDoorBehaviour>().openCondition;
                        line = "d " + child.position.x + " " + child.position.y + " " + openCond;
                    }
                    break;
            }
            if (line != ""){
                writer.WriteLine(line);
            }
        }
        
        writer.Close();
    }
}
