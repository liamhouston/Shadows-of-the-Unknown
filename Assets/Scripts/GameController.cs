using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameController : MonoBehaviour
{
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
    public GameObject XButton; // need x button to disable it on game over
    private bool gameOverFadeComplete;
    private bool gameOverFadeStarted;
    public string[] gameOverBark;
    public float mainMenuLoadDelay = 10; // how long the dialogue remains on screen before loading main menu

    [Header("Damage Effect")]
    public Image DamageEffectImage;
    public float damage_intensity = (float) 1.25;
    public float min_alpha = (float) 100;
    public float max_alpha = (float) 200;
    private bool currently_fading;
    private float desired_alpha;


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
        }
        gameOverFadeStarted = false;
        gameOverFadeComplete = false;
    }

    void Start(){

    }

    public void Update() {
        // update red tint
        float resolvePercent = (_maxResolve - _currentResolve + 0.0f) / _maxResolve;
        float alpha = resolvePercent * (max_alpha - min_alpha) + min_alpha;
        if (resolvePercent == 0)
            alpha = 0; // if player has not taken damage, don't show damage effect yet
        else {
            desired_alpha = alpha * damage_intensity;
            if (!currently_fading && DamageEffectImage.color.a < desired_alpha/255){
                // start fade 
                currently_fading = true;
                StartCoroutine(startDamageFade((float) 0.01f));
            }            
        }

        // check for game over
        if (_currentResolve <= 0 && !gameOverFadeStarted && !gameOverFadeComplete)
        {
            gameOverFadeStarted = true;
            // disable on screen elements
            DialogueManager.Instance.DisableDialoguePanel();
            InputManager.PlayerInput.actions.FindAction("Move").Disable();
            InputManager.PlayerInput.actions.FindAction("Click").Disable();
            InputManager.PlayerInput.actions.FindAction("RightClick").Disable();
            
            if (XButton != null)
                XButton.SetActive(false);

            StartCoroutine(GameOverFade((float)0.3));
        }
        else if (gameOverFadeComplete)
        {
            gameOverFadeComplete = false;
            
            // play dialogue if fade complete
            DialogueManager.Instance.DisableDialoguePanel();
            DialogueManager.Instance.playBlockingDialogue("Jay", gameOverBark);

            StartCoroutine(LoadMainMenuAfterSeconds(mainMenuLoadDelay));
        }
    }

    public void SetResolve(int resolve)
    {
        _currentResolve = resolve;
    }

    public void ChangeResolve(int change)
    {
        _currentResolve += change;
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
        CursorManager.Instance.MouseColliderEnable(false);
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

    private IEnumerator startDamageFade(float secondInBetween){
        while (DamageEffectImage.color.a < desired_alpha/255 && DamageEffectImage.color.a < 1){
            float fadeAmount = DamageEffectImage.color.a + Time.deltaTime;
            DamageEffectImage.color = new Color(DamageEffectImage.color.r, DamageEffectImage.color.g, DamageEffectImage.color.b, fadeAmount);
            yield return new WaitForSeconds(secondInBetween);
        }
        currently_fading = false;
    }

    private IEnumerator LoadMainMenuAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        // load main menu
        LevelManager.Instance.LoadScene("MainMenu", "CrossFade");
    }
}
