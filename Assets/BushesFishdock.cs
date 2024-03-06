using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushesFishdock : MonoBehaviour
{
    private bool _isPic;
    // Update is called once per frame
    private void Start()
    {
        if (GameController.Instance != null)
        {
            _isPic = GameController.Instance.TentPic;
        }

    }
    private void Update()
    {
        if (_isPic)
        {
            Debug.Log("took the pic already");
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
