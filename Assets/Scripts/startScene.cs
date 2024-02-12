using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startScene : MonoBehaviour
{
    private bool playerIsNearby;
    public string sceneToLoad;
    public GameObject InteractwithText;

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
        if (playerIsNearby && Input.GetKeyDown(KeyCode.I)){
            InteractwithText.SetActive(false);
            LevelManager.Instance.LoadScene(sceneToLoad, "CrossFade");
            MusicManager.Instance.PlayMusic(sceneToLoad);

        }
    }

    
}
