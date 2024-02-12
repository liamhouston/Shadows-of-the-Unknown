using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
 
public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
 
    private void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
        // var anim = GetComponent<Animator>();
        // var state = anim.GetCurrentAnimatorStateInfo(layerIndex:0);
        // anim.Play(state.fullPathHash, layer:0, normalizedTime:Random.Range(0f, 1f));
    }
 
    public void Play()
    {
        LevelManager.Instance.LoadScene("BasicScene", "CrossFade");
        MusicManager.Instance.PlayMusic("FirstPerson");
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