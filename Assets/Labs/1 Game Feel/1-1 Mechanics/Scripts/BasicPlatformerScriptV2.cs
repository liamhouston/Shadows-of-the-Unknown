using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

namespace GameFeel
{
    public class BasicPlatformerScriptV2 : MonoBehaviour
    {
        private const float VELOCITY_SCALING = 100;
        private float GROUND_CHECK_DEPTH = 0.7f;
        
        [Header("Input Bindings")]
        [SerializeField] protected KeyCode left = KeyCode.LeftArrow;
        [SerializeField] protected KeyCode right = KeyCode.RightArrow;
        [SerializeField] protected KeyCode jump = KeyCode.Space;
        
        [Header("Physics Settings")]
        [SerializeField] protected float gravity = 9.8f;
        [SerializeField] protected float mass = 10;
        [SerializeField] protected float groundBuffer = 0.08f;
        [SerializeField,Range(0,1)] protected float groundFriction = 0.139f;
        [SerializeField,Range(0,1)] protected float airFriction = 0.279f;
        
        [Header("Vertical Movement Settings")]
        [SerializeField] protected float jumpHeight = 16.21f;
        [SerializeField] protected float jumpTime = 0.5f;
        [SerializeField] protected AnimationCurve jumpCurve;
        [SerializeField] protected float hangTime = 0.2f;
        [SerializeField,Range(1,2)] protected int jumpCount = 1;
        
        [Header("Horizontal Movement Settings")]
        [SerializeField] protected float horizontalAcceleration = 10;
        [SerializeField] protected float maxHorizontalVelocity = 20;

        public enum STATE {Falling, Rising, Hanging, Grounded};
        private STATE _currState = STATE.Falling;
        [HideInInspector] public STATE currState
        {
            get => _currState;
            protected set
            {
                if (_currState == value)
                {
                    return;
                }
                _currState = value;
                switch (_currState)
                {
                    case STATE.Grounded: OnGrounded_Hook();print(_currState);
                        break;
                    case STATE.Rising: OnRising_Hook();print(_currState);
                        break;
                    case STATE.Hanging: OnRising_Hook();print(_currState);
                        break;
                    case STATE.Falling: OnFalling_Hook();print(_currState);
                        break;
                }
            }
        }
        protected virtual void OnGrounded_Hook(){}
        protected virtual void OnRising_Hook(){}
        protected virtual void OnHanging_Hook(){}
        protected virtual void OnFalling_Hook(){}
        
        private BoxCollider2D _playerCollider;
        private Rigidbody2D _rigidbody2D;
        private float _horizontalInput = 0;
        private Vector2 _currentVelocity = Vector2.zero;
        private Coroutine _jumpRoutine;
        private int jumpCounter = 0;

        private void Start()
        {
            _playerCollider = GetComponent<BoxCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        protected virtual void Update()
        {
            _horizontalInput = 0;
            if (Input.GetKeyDown(jump))
            {
                if (_jumpRoutine == null)
                {
                    _jumpRoutine = StartCoroutine(_Jump());
                }
                else if (jumpCounter < jumpCount)
                {
                    StopCoroutine(_jumpRoutine);
                    _jumpRoutine = StartCoroutine(_Jump());
                }
            }
            if (Input.GetKey(right))
            {
                _horizontalInput += 1;
            }
            if (Input.GetKey(left))
            {
                _horizontalInput -= 1;
            }

            _CheckGrounded();
            _CalculateHorizontalMovement();
            _ApplyFriction();
            _ApplyGravity();
            _ClampHorizontal();
        }

        protected virtual  void FixedUpdate()
        {
            _TryMove();
        }

        private void _TryMove()
        {
            Vector2 position = transform.position;
            Vector2 newPosition = (Vector2)position + _currentVelocity;
            Vector2 extents = _playerCollider.bounds.extents;
            
            if (_currentVelocity.y < 0)
            {
                RaycastHit2D checkValid = Physics2D.Raycast(position, Vector2.down);
                if (checkValid.collider)
                {
                    float distanceFromBase = checkValid.distance - extents.y;
                    if ( distanceFromBase <= groundBuffer && distanceFromBase>0)
                    {
                        currState = STATE.Grounded;
                        _currentVelocity = new Vector2(_currentVelocity.x, 0);
                        newPosition = (Vector2)position;
                        jumpCounter = 0;
                    }
                    else if (distanceFromBase < Mathf.Abs(_currentVelocity.y)+groundBuffer && distanceFromBase>0 )
                    {
                        newPosition = new Vector2(
                            newPosition.x,
                            position.y- Mathf.Clamp(distanceFromBase - groundBuffer,0,distanceFromBase)
                            );
                    }
                }
            }
            _rigidbody2D.MovePosition(newPosition);
        }

        private void _CheckGrounded()
        {
            Vector2 position = transform.position;
            Vector2 extents = _playerCollider.bounds.extents;
            Debug.DrawRay(position+ (Vector2.down * extents.y),Vector2.down*(GROUND_CHECK_DEPTH));
            
            if (currState == STATE.Grounded)
            {
                RaycastHit2D checkValid = Physics2D.Raycast(position, Vector2.down,extents.y+GROUND_CHECK_DEPTH);
                if (!checkValid.collider)
                {
                    currState = STATE.Falling;
                }
                
            }
        }

        private IEnumerator _Jump()
        {
            float timer = 0;
            float oldHeight = 0;
            currState = STATE.Rising;
            jumpCounter += 1;
            while (timer <= jumpTime)
            {
                timer += Time.deltaTime;
                float newHeight = jumpCurve.Evaluate(Mathf.Clamp(timer / jumpTime, 0, 1))*jumpHeight;
                float velocity = (newHeight - oldHeight);
                _currentVelocity = new Vector2(_currentVelocity.x, velocity);
                oldHeight = newHeight; 
                yield return null;
            }
            print("called");
            timer = 0;
            _currentVelocity = new Vector2(_currentVelocity.x, 0);
            currState = STATE.Hanging;
            
            while (timer<=hangTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            _jumpRoutine = null;
            currState = STATE.Falling;
        }
        
        private void _CalculateHorizontalMovement()
        {
            _currentVelocity += new Vector2(_horizontalInput, 0) * ((horizontalAcceleration) * 0.5f * Time.deltaTime);
        }

        private void _ApplyFriction()
        {
            if (Mathf.Abs(_currentVelocity.x) < 0.001f) _currentVelocity = new Vector2(0, _currentVelocity.y);
            _currentVelocity -= new Vector2(
                _currentVelocity.x * ((currState == STATE.Grounded) ? groundFriction : airFriction),
                0);
        }

        private void _ApplyGravity()
        {
            if (currState == STATE.Falling)
            {
                _currentVelocity += new Vector2(0, -gravity*mass) * (0.5f * Mathf.Pow(Time.deltaTime,2));
            }
        }
        
        private void _ClampHorizontal()
        {
            float adjustedMax = maxHorizontalVelocity / VELOCITY_SCALING;
            _currentVelocity = new Vector2(
                Mathf.Clamp(_currentVelocity.x,-adjustedMax,adjustedMax),
                _currentVelocity.y);
        }

        private static int _SignFloat(float val)
        {
            if (val > 0) return 1;
            if (val == 0) return 0;
            else return -1;
        }
    }
}