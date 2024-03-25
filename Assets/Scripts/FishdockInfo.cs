using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishdockinfo : MonoBehaviour
{
    private const string _lastHorizontal = "LastHorizontal";
    private void Start()
    {
        TryGetComponent(out Animator _playerAnimator);
        _playerAnimator.SetFloat(_lastHorizontal, -1);
        if (PlayerPrefs.HasKey("FromScene"))
        {
            if (PlayerPrefs.GetString("FromScene") == "Store")
            {
                this.transform.position = new Vector3(40.32f, -6.4f, 0f);
            }
            else if (PlayerPrefs.GetString("FromScene") == "Campsite")
            {
                this.transform.position = new Vector3(-59.46f, -6.4f, 0f);
                // TryGetComponent(out Animator _playerAnimator);
                // _playerAnimator.SetFloat(_lastHorizontal, -1);
                // _playerAnimator.SetFloat("LastHorizontal", 1);
            }
        }
    }
}