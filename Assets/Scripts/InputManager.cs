using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public static PlayerInput PlayerInput;
    public Vector2 MoveInput { get; private set; }
    public Vector2 MoveCInput { get; private set; }

    public Vector2 MouseInput { get; private set; }
    public Vector2 MouseCInput { get; private set; }

    public bool MenuOpenInput { get; private set; }
    public bool MenuCloseInput { get; private set; }
    public bool RightClickInput { get; private set; }
    public bool ClickInput { get; private set; }
    public bool ClickCInput { get; private set; }
    public bool ClickUIInput { get; private set; }

    private InputAction _moveInputAction;
    private InputAction _moveCInputAction;
    private InputAction _mouseInputAction;
    private InputAction _mouseCInputAction;
    private InputAction _menuOpenAction;
    private InputAction _menuOpenCAction;
    private InputAction _menuCloseAction;
    private InputAction _rightClickAction;
    private InputAction _clickAction;
    private InputAction _clickCAction;
    private InputAction _clickUIAction;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerInput = GetComponent<PlayerInput>();

        _moveInputAction = PlayerInput.actions["Move_p"];
        _moveCInputAction = PlayerInput.actions["Move_c"];
        _menuOpenAction = PlayerInput.actions["Player/MenuOPEN"];
        _menuOpenCAction = PlayerInput.actions["Camera/MenuOPEN"];
        // _menuCloseAction = PlayerInput.actions["MenuClose"];
        // _mouseInputAction = PlayerInput.actions["Point"];
        _mouseCInputAction = PlayerInput.actions["Point_c"];
        _rightClickAction = PlayerInput.actions["RightClick_c"];
        // _clickAction = PlayerInput.actions["Click_p"];
        _clickCAction = PlayerInput.actions["Click_C"];
        // _clickUIAction = PlayerInput.actions["UI/Click"];
    }

    // Update is called once per frame
    private void Update()
    {
        MoveInput = _moveInputAction.ReadValue<Vector2>();
        MoveCInput = _moveCInputAction.ReadValue<Vector2>();
        // MouseInput = _mouseInputAction.ReadValue<Vector2>();
        MouseCInput = _mouseCInputAction.ReadValue<Vector2>();

        MenuOpenInput = _menuOpenAction.WasPressedThisFrame() || _menuOpenCAction.WasPressedThisFrame();
        // MenuCloseInput = _menuCloseAction.WasPerformedThisFrame();
        RightClickInput = _rightClickAction.WasPressedThisFrame();
        // ClickInput = _clickAction.WasPressedThisFrame();
        ClickCInput = _clickCAction.WasPressedThisFrame();
        // ClickUIInput = _clickUIAction.WasPressedThisFrame();
    }
}
