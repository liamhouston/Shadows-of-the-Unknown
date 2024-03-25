using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [Header("Scene names")]
    public string ToScene = "";
    // private const string _lastHorizontal = "LastHorizontal";
    private bool playerIsNearby;
    private void Update()
    {
        if (playerIsNearby && InputManager.Instance.ClickInput)
        {
            LevelManager.Instance.LoadScene(ToScene, "CrossFade");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerIsNearby = true;
        // InteractwithText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerIsNearby = false;
        // if (InteractwithText != null)
        // {
        //     // InteractwithText.SetActive(false);
        // }

    }
}
