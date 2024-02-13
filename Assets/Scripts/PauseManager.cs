using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public static bool IsPaused = false;
    public GameObject pauseMenuUI;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void EscapceCheck()
    {
        if(IsPaused)
        {
            Cursor.visible = false;
            Resume();
        }
        else
        {
            Cursor.visible = true;
            Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        LevelManager.Instance.LoadScene("MainMenu", "CrossFade");
    }
    public void Quid()
    {
        Application.Quit();
    } 
}