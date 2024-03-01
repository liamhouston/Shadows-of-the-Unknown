using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public static PlayerInput PlayerInput;
    public Vector2 MoveInput { get; private set; }

    public Vector2 MouseInput { get; private set; }
    public bool MenuOpenInput { get; private set; }
    public bool MenuCloseInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool RightClickInput { get; private set; }
    public bool ClickInput { get; private set; }

    private InputAction _moveInputAction;
    private InputAction _mouseInputAction;
    private InputAction _menuOpenAction;
    private InputAction _menuCloseAction;
    private InputAction _rightClickAction;
    private InputAction _clickAction;
    private InputAction _interactInputAction;
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

        _moveInputAction = PlayerInput.actions["Move"];
        _menuOpenAction = PlayerInput.actions["MenuOPEN"];
        _menuCloseAction = PlayerInput.actions["MenuClose"];
        _interactInputAction = PlayerInput.actions["Interact"];
        _mouseInputAction = PlayerInput.actions["Point_c"];
        _rightClickAction = PlayerInput.actions["RightClick"];
        _clickAction = PlayerInput.actions["Click"];
    }

    // Update is called once per frame
    private void Update()
    {
        MoveInput = _moveInputAction.ReadValue<Vector2>();
        MouseInput = _mouseInputAction.ReadValue<Vector2>();

        MenuOpenInput = _menuOpenAction.WasPressedThisFrame();
        MenuCloseInput = _menuCloseAction.WasPerformedThisFrame();
        InteractInput = _interactInputAction.WasPerformedThisFrame();
        RightClickInput = _rightClickAction.WasPressedThisFrame();
        ClickInput = _clickAction.WasPressedThisFrame();
    }
}
