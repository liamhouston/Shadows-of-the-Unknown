﻿using System;
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
        [SerializeField] protected float terminalVelocity = 20;
        
        [Header("Vertical Movement Settings")]
        [SerializeField] protected float jumpStrength = 16.21f;
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
                    case STATE.Grounded: OnGrounded_Hook();print(transform.position.y);
                        break;
                    case STATE.Rising: OnRising_Hook();print(_currState);
                        break;
                    case STATE.Hanging: OnHanging_Hook();print(transform.position.y);
                        break;
                    case STATE.Falling: OnFalling_Hook();print(_currState);
                        break;
                }
            }
        }
        //Jump hooks
        protected virtual void OnGrounded_Hook(){}
        protected virtual void OnRising_Hook(){}
        protected virtual void OnHanging_Hook(){}
        protected virtual void OnFalling_Hook(){}
        
        private BoxCollider2D _playerCollider;
        private Rigidbody2D _rigidbody2D;
        private float _horizontalInput = 0;
        private Vector2 _currentVelocity = Vector2.zero;
        private Coroutine _jumpRoutine;
        private int _jumpCounter = 0;

        #region Unity LifeCycle
            private void Start()
            {
                _playerCollider = GetComponent<BoxCollider2D>();
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }
            
            protected virtual void Update()
            {
                //Input parsing
                _horizontalInput = 0;
                if (Input.GetKeyDown(jump))
                {
                    if (_jumpRoutine == null)
                    {
                        _jumpRoutine = StartCoroutine(_Jump());
                    }
                    else if (_jumpCounter < jumpCount)
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
                
                //all the physics calc short of actually moving happen in here.
                _CheckGrounded();
                _CalculateHorizontalMovement();
                _ApplyFriction();
                _ApplyGravity();
                _ClampVelocities();
            }

            protected virtual  void FixedUpdate()
            {
                //actually move in fixed update to avoid kinematic body glitches
                _TryMove();
            }
        #endregion
        
        
        /// <summary>
        /// the only time we need collision checks is while going down so we use a raycast to check if the displacement
        /// of our movement is going to put us inside an obstacle, if yest it adjusts the displacement to only go so far
        /// that it dosnt clip.
        /// </summary>
        private void _TryMove()
        {
            Vector2 position = transform.position;
            Vector2 newPosition = (Vector2)position + _currentVelocity;
            Vector2 extents = _playerCollider.bounds.extents;
            
            if (_currentVelocity.y < 0)
            {
                //only check if going down.
                RaycastHit2D checkValid = Physics2D.Raycast(position, Vector2.down);
                if (checkValid.collider)
                {
                    //we only need to do adjust if were actually about to hit something
                    float distanceFromBase = checkValid.distance - extents.y;
                    if ( distanceFromBase <= groundBuffer && distanceFromBase>0)
                    {
                        //if the distance from our position to the obstacle is less than the ground buffer then we've
                        //landed.
                        currState = STATE.Grounded;
                        _currentVelocity = new Vector2(_currentVelocity.x, 0);
                        newPosition = (Vector2)position;
                        _jumpCounter = 0;
                    }
                    else if (distanceFromBase < Mathf.Abs(_currentVelocity.y)+groundBuffer && distanceFromBase>0 )
                    {
                        //otherwise adjust the velocity; dont kill it so we can get as close as possible
                        newPosition = new Vector2(
                            newPosition.x,
                            position.y- Mathf.Clamp(distanceFromBase - groundBuffer,0,distanceFromBase)
                            );
                    }
                }
            }
            //now we can safely update
            _rigidbody2D.MovePosition(newPosition);
        }
        
        
        /// <summary>
        /// Use a ray cast to check if we walked off a ledge or if the floor got deleted. Seperated from the other raycast
        /// to avoid glitches because we only need this when we are grounded. Jump is handled with no physics
        /// </summary>
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
        
        
        /// <summary>
        /// Our physics free routine for jump. Handling the upward motion in a physical manner caused way too many inconsistencies
        /// so I just gave up and move the stuff based on a max height which is the jump strength. This also handles hang time.
        /// </summary>
        private IEnumerator _Jump()
        {
            float timer = 0;
            float oldHeight = 0;
            currState = STATE.Rising;
            _jumpCounter += 1;
            while (timer <= jumpTime)
            {
                timer += Time.deltaTime;
                float curveValue = jumpCurve.Evaluate(Mathf.Clamp(timer / jumpTime, 0, 1));
                float newHeight = Mathf.Lerp(0,jumpStrength,curveValue);
                float velocity = (newHeight - oldHeight);
                _currentVelocity = new Vector2(_currentVelocity.x, velocity);
                oldHeight = newHeight; 
                yield return new WaitForFixedUpdate();
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
        
        
        /// <summary>
        /// Simple horizontal movement calculation.
        /// </summary>
        private void _CalculateHorizontalMovement()
        {
            _currentVelocity += new Vector2(_horizontalInput, 0) * ((horizontalAcceleration) * 0.5f * Time.deltaTime);
        }
        
        
        /// <summary>
        /// These two functions calculate friction and gravity based on current velocity. There both not physically accurate
        /// but i think they look believable enough
        /// </summary>
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
        
        
        /// <summary>
        /// Clamp velocities to abide by terminal and max horizontal velocity. Jump dosnt care about clamping since its
        /// not physics based.
        /// </summary>
        private void _ClampVelocities()
        {
            float adjustedMaxHorizontal = maxHorizontalVelocity / VELOCITY_SCALING;
            float adjustedTerminal = terminalVelocity / VELOCITY_SCALING;
            float clampedHorizontal = Mathf.Clamp(_currentVelocity.x, -adjustedMaxHorizontal, adjustedMaxHorizontal);
            float clampedVertical = Mathf.Clamp(_currentVelocity.y, -adjustedTerminal, adjustedTerminal);
            _currentVelocity = new Vector2(
                clampedHorizontal,
                (currState == STATE.Falling)?clampedVertical:_currentVelocity.y
                );
        }
        
        
        //made this but ended up not using it ill get rid of it after im done with visual script or maybe move it there
        private static int _SignFloat(float val)
        {
            if (val > 0) return 1;
            if (val == 0) return 0;
            else return -1;
        }
    }
}