using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange_ref : MonoBehaviour
{
    public string sceneName;
    
    private void OnTriggerEnter2D(Collider2D other) {
    print("Trigger Entered");
    
    if(other.tag == "Player") 
        {
            LevelManager.Instance.LoadScene(sceneName, "CrossFade");
        }
    }
}
