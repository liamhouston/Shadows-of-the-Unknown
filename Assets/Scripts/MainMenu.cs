// using UnityEngine;
// using UnityEngine.Audio;
// using UnityEngine.UI;
 
// public class MainMenu : MonoBehaviour
// {
//     public AudioMixer audioMixer;
//     public Slider musicSlider;
//     public Slider sfxSlider;
 
//     private void Start()
//     {
//         LoadVolume();
//         MusicManager.Instance.PlayMusic("MainMenu");
//     }
 
//     public void Play()
//     {
//         LevelManager.Instance.LoadScene("Game", "CrossFade");
//         MusicManager.Instance.PlayMusic("Game");
//     }
 
//     public void Quit()
//     {
//         Application.Quit();
//     }
 
//     public void UpdateMusicVolume(float volume)
//     {
//         audioMixer.SetFloat("MusicVolume", volume);
//     }
 
//     public void UpdateSoundVolume(float volume)
//     {
//         audioMixer.SetFloat("SFXVolume", volume);
//     }
 
//     public void SaveVolume()
//     {
//         audioMixer.GetFloat("MusicVolume", out float musicVolume);
//         PlayerPrefs.SetFloat("MusicVolume", musicVolume);
 
//         audioMixer.GetFloat("SFXVolume", out float sfxVolume);
//         PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
//     }
 
//     public void LoadVolume()
//     {
//         musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
//         sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
//     }
// }