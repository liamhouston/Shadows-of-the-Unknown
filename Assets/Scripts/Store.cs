using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // if puzzle is done, disable the puzzle
    public GameObject xbutton;
    private void Start()
    {
        if (PlayerPrefs.GetInt("CampsitePuzzle") == 1)
        {
            TryGetComponent(out Collider2D storeCollider);
            storeCollider.enabled = false;
            xbutton.SetActive(true);
        }
    }
}
