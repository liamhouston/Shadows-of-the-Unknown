using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TopDownComponentHandler : MonoBehaviour
{
    GameObject[] components;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // space the components evenly in a grid
        components = GameObject.FindGameObjectsWithTag("PCGComponent");
        
        int pos_x = 0;
        int pos_y = 0;
        int row = 0;

        foreach (GameObject c in components){
            c.transform.position = new Vector3(pos_x, pos_y, 0);
            pos_x = (pos_x + 15) % 60;
            if (row == 3){
                pos_y -= 10;
            }
            row = (row + 1) % 4;
        }
    }

    public void save(){
        // get rid of any existing components
        foreach (string f in Directory.EnumerateFiles("Assets/Labs/3 2D Level Design/3-5 PCG/Scenes/Components/")){
            File.Delete(f);
        }

        components = GameObject.FindGameObjectsWithTag("PCGComponent");

        for (int i = 0; i < components.Length; i++){
            TopDownComponentScript t = components[i].GetComponent<TopDownComponentScript>();
            t.saveToFile("component" + i.ToString() + ".txt");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor (typeof(TopDownComponentHandler))]
public class PCGEditor : Editor{
    
    public override void OnInspectorGUI (){
        TopDownComponentHandler compHandler = (TopDownComponentHandler)target;
        if (GUILayout.Button("Save Components")){
            compHandler.save();
        }
        DrawDefaultInspector();
    }
}
#endif