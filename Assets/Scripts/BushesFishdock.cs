using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BushesFishdock : MonoBehaviour
{
    [SerializeField]
    private bool _isPic;
    // Update is called once per frame
    private void Start()
    {
        if (GameController.Instance != null)
        {
            _isPic = Player.Instance.TentPic;
        }

    }
    private void Update()
    {
        if (_isPic)
        {
            Debug.Log("took the pic already");
            GetComponent<BoxCollider2D>().enabled = false;
            TryGetComponent(out Light2D lightpath);
            if (lightpath)
            {
                lightpath.enabled = true;
            }
        }
    }
}
