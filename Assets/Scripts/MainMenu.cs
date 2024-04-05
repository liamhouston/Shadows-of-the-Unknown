using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Menu always plays the right music whenever a scene is loaded
    private void Start()
    {
        LoadVolume();
        Scene activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;
        MusicManager.Instance.PlayMusic(sceneName); 
    }

    public void Play()
    {
        if (PlayerPrefs.GetInt("Bedroom") == 1){
            LevelManager.Instance.LoadScene("Bedroom", "CrossFade");
        }
        else {
            // Load intro cutscene if player hasn't been to bedroom
            LevelManager.Instance.LoadScene("IntroCutscene", "CrossFade");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}