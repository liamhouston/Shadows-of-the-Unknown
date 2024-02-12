using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameController : MonoBehaviour {
    public Slider resolveBar;

    private static GameController _instance;
    public static GameController Instance {get{return _instance;}}

    public CinemachineVirtualCamera CinemachineVirtualCamera;

    [Header("Player Info")]
    [SerializeField] private int _maxResolve = 100;
    [SerializeField] private int _currentResolve;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            _currentResolve = _maxResolve;
            this.SetMaxResolve(_maxResolve);
            Debug.Assert(CinemachineVirtualCamera != null, "CinemachineVirtualCamera must exist in the scene");
        }
    }

    public void SetMaxResolve(int resolve)
    {
        resolveBar.maxValue = resolve;
        resolveBar.value = resolve;
    }

    public void SetResolve(int resolve)
    {
        resolveBar.value = resolve;
    }

    public void ChangeResolve(int change) {
        resolveBar.value += change;
    } 

    public void StartShake(float intensity, float time) {
        Debug.Log("starting shake");
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Debug.Assert(cinemachineBasicMultiChannelPerlin != null, "CinemachineBasicMultiChannelPerlin must exist on the virtual camera");
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        StartCoroutine(StopShake(time));
    }

    private IEnumerator StopShake(float time) {
        yield return new WaitForSeconds(time);
        Debug.Log("stopping shake");
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

}
