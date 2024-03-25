using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [Header("Scene names")]
    public string ToScene = "";
    private const string _lastHorizontal = "LastHorizontal";
    
    private void OnTriggerEnter2D(Collider2D other) {    
    if(other.tag == "Player") 
        {
            if (ToScene != "")
            {
                string fromScene = SceneManager.GetActiveScene().name;
                PlayerPrefs.SetInt(fromScene, 1);
                PlayerPrefs.SetString("FromScene", fromScene);
                LevelManager.Instance.LoadScene(ToScene, "CrossFade");
            }
        }
    }



}
