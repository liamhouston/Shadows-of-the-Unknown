using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startScene : MonoBehaviour
{
    private bool playerIsNearby;
    private static bool darkMode;
    public string sceneToLoad;
    public GameObject InteractwithText;

    public AudioSource InteractwithSound;

    // Start is called before the first frame update
    void Start()
    {
        InteractwithText.SetActive(false);
        playerIsNearby = false;
    }
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) playerIsNearby = true;
        InteractwithText.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) playerIsNearby = false;
        InteractwithText.SetActive(false);
    }
    // Update is called once per frame
    void Update(){
        if (playerIsNearby && Input.GetKey(KeyCode.I)){
            InteractwithText.SetActive(false);
            if (this.CompareTag("Bush") && darkMode == false)
            {
                darkMode = true;
                sceneToLoad = SceneManager.GetActiveScene().name + "_cam";
                print("this is not working");
                print(sceneToLoad);
                SceneManager.LoadScene(sceneToLoad);
            }
            else if(this.CompareTag("Bush") && darkMode == true)
            {
                darkMode = false;
                // sceneToLoad = SceneManager.GetActiveScene().ToString().PadRight(-4);
                SceneManager.LoadScene(("PrototypeScene"));
            }
            //SceneManager.LoadScene(sceneToLoad);
        }
    }

    
}
