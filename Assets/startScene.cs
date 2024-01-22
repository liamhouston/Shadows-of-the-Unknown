using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startScene : MonoBehaviour
{
    private bool playerIsNearby = false;
    public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        if (playerIsNearby && Input.GetKey(KeyCode.I)){
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
