﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    // private AudioSource audioSource;
    private Animator _animator;
    private const string _horizontal = "Horizontal";

    private void Start()
    {
        Cursor.visible = true;
        InputManager.PlayerInput.actions.FindActionMap("Player").Enable();
        InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap("Player");
        InputManager.PlayerInput.SwitchCurrentActionMap("Player");
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

    }
    private void Update()
    {
        Debug.Log(InputManager.PlayerInput.currentActionMap);
        _movement.Set(InputManager.Instance.MoveInput.x, InputManager.Instance.MoveInput.y);

        _rb.velocity = _movement * _moveSpeed;
        _animator.SetFloat(_horizontal, _movement.x);

    }
    // Update is called once per frame
    // void Update(){
    //     if(InputManager.Instance.MenuOpenInput)
    //     {
    //         PauseManager.Instance.Pause();
    //         // PauseManager.Instance.PauseCheck();
    //     }
    //     else
    //     {
    //         AnimationUpdate();
    //         transform = 
    // Movement controls
    // if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
    //     setStateWalk();
    //     transform.position+= Vector3.left*Time.deltaTime*Speed;
    //     if (transform.localScale.x > 0){ // if facing right, flip sprite
    //         transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    //     }    
    // }
    // else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
    //     setStateWalk();
    //     transform.position+= Vector3.right*Time.deltaTime*Speed;
    //     if (transform.localScale.x < 0){ // if facing left, flip sprite
    //         transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    //     }
    // }
    // else {
    //     setStateIdle();
    // }
}


//     }
// }

