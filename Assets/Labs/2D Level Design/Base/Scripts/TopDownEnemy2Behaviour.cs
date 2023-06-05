using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEnemy2Behaviour : TopDownEnemyBehaviour
{   
    public float projectileTimer = 2f;
    public GameObject projectile;

    private float _currentTime;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        _currentTime = projectileTimer;
    }

    // Update is called once per frame
    override public void FixedUpdate()
    {
        base.FixedUpdate();
        _currentTime -= Time.deltaTime;
        
        // shoot a bullet if the time is right
        if (_currentTime <= 0){
            GameObject p = Instantiate(projectile) as GameObject;
            TopDownEnemyProjectileBehaviour projScript = (TopDownEnemyProjectileBehaviour)p.GetComponent(typeof(TopDownEnemyProjectileBehaviour));
            projScript.setDirection((int)_currDir); 
            p.transform.position = transform.position;
            _currentTime = projectileTimer;
        }
    }
}
