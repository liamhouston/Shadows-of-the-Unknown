using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomInfo : MonoBehaviour
{
    private const string _lastHorizontal = "LastHorizontal";
    private void Start()
    {
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);
        if (PlayerPrefs.HasKey("FromScene"))
        {
            if (PlayerPrefs.GetString("FromScene") == "BedroomCam")
            {
                this.transform.position = new Vector3(-33.54f, 14.55f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "Fishdock")
            {
                this.transform.position = new Vector3(-68.65f, 14.55f, 0f);
                // TryGetComponent(out Animator _playerAnimator);
                // _playerAnimator.SetFloat(_lastHorizontal, -1);
                _playerAnimator.SetFloat("LastHorizontal", 1);
            }
        }
    }
}
