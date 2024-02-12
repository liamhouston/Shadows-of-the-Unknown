using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scre : MonoBehaviour
{
    public GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            print("space is typed");
            a.SetActive(true);
        }
    }
}
