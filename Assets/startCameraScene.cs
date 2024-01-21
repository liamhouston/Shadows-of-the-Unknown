using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startCameraScene : MonoBehaviour
{
    private bool playerIsNearby = false;
    private string sceneToLoad = "CameraMiniGameScene";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        if (playerIsNearby && Input.GetKey(KeyCode.C)){
            Debug.LogError("Opening a new Scene!");
            //SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.LogError("Collision with bush");
        if (other.CompareTag("Player")) playerIsNearby = true;
    }

    void OnTriggerExit2D(Collider2D other){
        Debug.LogError("Exit collision with bush");
        if (other.CompareTag("Player")) playerIsNearby = false;
    }
}
