using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    public void SetVolume(float volume){
        // Clamp the volume between 0 and 1
        volume = Mathf.Clamp01(volume);
        // Set the volume of the audio source
        sfx2DSource.volume = volume;
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound2D(string soundName)
    {
        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        sfx2DSource.PlayOneShot(clip);
    }

    public void PlayLoopingSound2D(string soundName)
    {
        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        sfx2DSource.clip = clip;
        sfx2DSource.loop = true;
        sfx2DSource.Play();
    }

    public void TurnOffSound(){
        sfx2DSource.Stop();
    }
}