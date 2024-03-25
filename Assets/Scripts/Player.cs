﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Runtime.InteropServices;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    public bool TentPic;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    // keep track of whether we've played the opening dialogue for this scene
    public string currentSceneName = "Fishdock";
    Dictionary<string, bool> enteredScenes = new Dictionary<string, bool>();

    private Animator _animator;
    private const string _horizontal = "Horizontal";
    private const string _lastHorizontal = "LastHorizontal";
    public static Player Instance;

    private AudioSource footstepsAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Cursor.visible = true;
        // InputManager.PlayerInput.actions.FindActionMap("Player").Enable();
        // InputManager.PlayerInput.currentActionMap = InputManager.PlayerInput.actions.FindActionMap("Player");
        // InputManager.PlayerInput.SwitchCurrentActionMap("Player");
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        footstepsAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {        _movement.Set(InputManager.Instance.MoveInput.x, 0f);

        _rb.velocity = _movement * _moveSpeed;
        _animator.SetFloat(_horizontal, _movement.x);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            if (!footstepsAudioSource.isPlaying){
                footstepsAudioSource.Play();
            }
        }
        else {
            footstepsAudioSource.Stop();
        }
    }

    public bool playedOpeningDialogue()
    {
        // Check if we've already been in this scene
        if (currentSceneName != null)
        {
            if (enteredScenes.ContainsKey(currentSceneName))
            {
                return true; // we've already been in this scene
            }
            else
            {
                enteredScenes[currentSceneName] = true;
                return false;
            }
        }
        return false;
    }
}
