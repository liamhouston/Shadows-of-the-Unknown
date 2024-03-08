using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public Slider resolveBar;

    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    [Header("Virtual Camera Info")]
    private float default_amplitude;
    private float default_frequency;

    public CinemachineVirtualCamera CinemachineVirtualCamera;

    public NoiseSettings ShakeNoiseProfile;
    public NoiseSettings DefaultNoiseProfile;

    [Header("Player Info")]
    [SerializeField] private int _maxResolve = 100;
    [SerializeField] private int _currentResolve;

    [Header("Game Over Info")]
    public Image BlackoutBox;
    private bool gameOverFadeComplete = false;
    public string[] gameOverBark;


    void Awake()
    {
        // Debug.Assert(CinemachineVirtualCamera != null, "CinemachineVirtualCamera must exist in the scene");
        // Debug.Assert(ShakeNoiseProfile != null, "ShakeNoiseProfile must exist in the scene");
        // Debug.Assert(DefaultNoiseProfile != null, "DefaultNoiseProfile must exist in the scene");
        // Debug.Assert(BlackoutBox != null, "BlackoutBox must exist in scene");

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        default_amplitude = cinemachineBasicMultiChannelPerlin.m_AmplitudeGain;
        default_frequency = cinemachineBasicMultiChannelPerlin.m_FrequencyGain;

        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            _currentResolve = _maxResolve;
            this.SetMaxResolve(_maxResolve);
        }
    }

    public void Update()
    {
        // check for game over
        if (_currentResolve <= 0 && !gameOverFadeComplete)
        {
            StartCoroutine(GameOverFade((float)0.01));
        }
        else if (gameOverFadeComplete)
        {
            // play dialogue if fade complete
            LevelManager.Instance.LoadScene("MainMenu", "CrossFade");
        }
    }

    public void SetMaxResolve(int resolve)
    {
        resolveBar.maxValue = resolve;
        resolveBar.value = resolve;
    }

    public void SetResolve(int resolve)
    {
        _currentResolve = resolve;
        resolveBar.value = resolve;
    }

    public void ChangeResolve(int change)
    {
        _currentResolve += change;
        resolveBar.value += change;
    }

    public void StartShake(float amplitude, float frequency, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_NoiseProfile = ShakeNoiseProfile;

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        StartCoroutine(StopShake(time));
    }

    private IEnumerator StopShake(float time)
    {
        yield return new WaitForSeconds(time);

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_NoiseProfile = DefaultNoiseProfile;

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = default_amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = default_frequency;
    }

    public IEnumerator FadeOutBlackOutSquare(float fadeSpeed = 1)
    {
        Color objectColor = BlackoutBox.GetComponent<Image>().color;
        float fadeAmount;

        while (BlackoutBox.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = BlackoutBox.GetComponent<Image>().color.a + (fadeSpeed * Time.deltaTime);
            BlackoutBox.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }

        gameOverFadeComplete = true;
    }

    public IEnumerator GameOverFade(float fadeSpeed = 1)
    {
        Color objectColor = BlackoutBox.GetComponent<Image>().color;
        float fadeAmount;

        while (BlackoutBox.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = BlackoutBox.GetComponent<Image>().color.a + (fadeSpeed * Time.deltaTime);
            BlackoutBox.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }

        gameOverFadeComplete = true;
    }

    public IEnumerator FadeToAndFromBlack(float initialWait, float duringWait = 0.05f, float inBetweenWait = 0.25f)
    {
        Color objectColor = BlackoutBox.GetComponent<Image>().color;
        float fadeAmount;

        yield return new WaitForSeconds(initialWait);

        while (BlackoutBox.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = BlackoutBox.GetComponent<Image>().color.a + 10 * Time.deltaTime;
            BlackoutBox.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return new WaitForSeconds(duringWait);
        }

        yield return new WaitForSeconds(inBetweenWait);

        while (BlackoutBox.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = BlackoutBox.GetComponent<Image>().color.a - 10 * Time.deltaTime;
            BlackoutBox.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return new WaitForSeconds(duringWait);
        }
    }
}
