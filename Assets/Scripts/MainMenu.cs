﻿using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // InputManager.PlayerInput.actions.FindActionMap("UI").Enable();
        LoadVolume();
        Scene activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;
        MusicManager.Instance.PlayMusic(sceneName);

        // PlayerPrefs.SetInt("IsCameraPickedUp", 0); // player has not picked up camera yet
    }

    public void Play()
    {
        LevelManager.Instance.LoadScene("Bedroom", "CrossFade");
        // MusicManager.Instance.PlayMusic("Bedroom");
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