using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campsite : MonoBehaviour
{
    public GameObject xbutton;
    private void Start()
    {
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            TryGetComponent(out Collider2D campsiteCollider);
            campsiteCollider.enabled = false;
            xbutton.SetActive(true);
        }
    }
}
