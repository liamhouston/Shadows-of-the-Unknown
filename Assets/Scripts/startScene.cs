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
    private void Start()
    {
        InteractwithText.SetActive(false);
        playerIsNearby = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerIsNearby && InputManager.Instance.ClickInput)
        {
            NextLevel();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerIsNearby = true;
        InteractwithText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerIsNearby = false;
        if (InteractwithText != null)
        {
            InteractwithText.SetActive(false);
        }

    }
    public void NextLevel()
    {
        InteractwithText.SetActive(false);
        LevelManager.Instance.LoadScene(sceneToLoad, "CrossFade");
        MusicManager.Instance.PlayMusic(sceneToLoad);
    }


}
