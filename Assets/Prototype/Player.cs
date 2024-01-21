﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : AnimatedEntity
{  
    public float Speed=5;
    private AudioSource audioSource;

    void Start(){
        AnimationSetup();
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update(){
        AnimationUpdate();
        
        // Movement controls
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            setStateWalk();
            transform.position+= Vector3.left*Time.deltaTime*Speed;
            if (transform.localScale.x > 0){ // if facing right, flip sprite
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }    
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            setStateWalk();
            transform.position+= Vector3.right*Time.deltaTime*Speed;
            if (transform.localScale.x < 0){ // if facing left, flip sprite
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else {
            setStateIdle();
        }

    }
}

