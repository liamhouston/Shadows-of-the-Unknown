using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFeel{
    public class AudioPlatformerScript : VisualPlatformerScript
    {
        //Audio Stuff
        public AudioSource audioSource;
        public AudioClip jumpClip, impactClip;

        void Start() {
            boxCollider = GetComponent<BoxCollider2D>();//For size of collider purposes
            acceleration.y = -1 * gravity;//Assuming you start in the air
            internalDefaultTimeSpeed = defaultTimeSpeed;//In case you want to set a slow down
            defaultDimensions = transform.localScale;//Basic scale
        }

       
        // Update is called once per frame
        void Update()
        {
            //Jump Time Speed Change + Scale Timer
            if (internalJumpTimeChangeTimer>0){
                internalJumpTimeChangeTimer-=Time.deltaTime;
                if(internalJumpTimeChangeTimer<=0){
                    defaultTimeSpeed = internalDefaultTimeSpeed;
                    //Set Regular Scale
                    transform.localScale = defaultDimensions;

                }
            }
            //Impact Time Speed Change + Scale Timer
            if (internalImpactTimeChangeTimer>0){
                internalImpactTimeChangeTimer-=Time.deltaTime;
                if(internalImpactTimeChangeTimer<=0){
                    defaultTimeSpeed = internalDefaultTimeSpeed;
                    //Set Regular Scale
                    transform.localScale = defaultDimensions;
                    transform.position+=Vector3.up*(defaultDimensions.y-impactDimensions.y);
                }
            }


            //Position Update Information
            Vector3 newPosition = transform.position;

            float percentageForAirOrGround = airSpeedPercentage;

            if (currState == STATE.Grounded)
            {
                percentageForAirOrGround = 1f;
            }

            //Input Checks
            if (Input.GetKey(left))
            {
                acceleration.x = horizontalAcceleration * -1f * percentageForAirOrGround;
            }
            if (Input.GetKey(right))
            {
                acceleration.x = horizontalAcceleration * 1f * percentageForAirOrGround;
            }
            if(!Input.GetKey(left) && !Input.GetKey(right))
            {
                acceleration.x = 0f;
            }

            if (currState == STATE.Grounded && Input.GetKeyDown(jump))
            {
                currState = STATE.Rising;
                timer = 0;
                acceleration.y = kickOffAcceleration;
                velocity.y = acceleration.y;

                //Jumped!
                if (jumpTimeChangeTimer>0){
                    internalJumpTimeChangeTimer = jumpTimeChangeTimer;
                    defaultTimeSpeed = timeSpeedOnJump;
                }

                //Play Jump Sound
                if(jumpClip!=null && audioSource!=null){
                    audioSource.clip  = jumpClip;
                    if(!audioSource.isPlaying){
                        audioSource.Play();
                    }
                }

                //Set jump "stretch"
                transform.localScale = jumpDimensions;
                transform.position+=Vector3.up*(jumpDimensions.y-defaultDimensions.y);
            }
            else if (Input.GetKeyDown(jump) && doubleJump && !doubleJumped)//Double jump check
            {
                doubleJumped = true;
                currState = STATE.Rising;
                timer = 0;
                acceleration.y = kickOffAcceleration;
                velocity.y = acceleration.y;

                //Jumped!
                if (jumpTimeChangeTimer>0){
                    internalJumpTimeChangeTimer = jumpTimeChangeTimer;
                    defaultTimeSpeed = timeSpeedOnJump;
                }

                //Play Jump Sound
                if(jumpClip!=null && audioSource!=null){
                    audioSource.clip  = jumpClip;
                    if(!audioSource.isPlaying){
                        audioSource.Play();
                    }
                }

                //Set jump "stretch"
                transform.localScale = jumpDimensions;
                transform.position+=Vector3.up*(jumpDimensions.y-defaultDimensions.y);
            }

            //Cheap physics simulation
            CheapPhysicsSimulation(percentageForAirOrGround);

            //State-based movement
            if (currState == STATE.Falling)
            {
                acceleration.y -= Time.deltaTime * gravity*defaultTimeSpeed;
                //Speeding up to terminal velocity
                if (velocity.y > -1 * terminalVelocity)
                {
                    velocity.y += Time.deltaTime * acceleration.y*defaultTimeSpeed;
                    velocity.y = Mathf.Max(velocity.y, -1 * terminalVelocity);
                }
            }
            if(currState == STATE.Rising)
            {
                acceleration.y-= Time.deltaTime * gravity*defaultTimeSpeed;
                velocity.y += Time.deltaTime * acceleration.y*defaultTimeSpeed;
                velocity.y = Mathf.Min(velocity.y, maxVerticalVelocity);

                if (velocity.y <= 0)
                {
                    if (hangTime > 0)
                    {
                        currState = STATE.Hanging;
                        velocity.y = 0;
                        timer = 0;
                    }
                    else
                    {
                        currState = STATE.Falling;
                    }
                    
                    
                }



            }
            else if (currState == STATE.Hanging)
            {
                if (timer < hangTime)
                {
                    timer += Time.deltaTime*defaultTimeSpeed;
                }

                if (timer >= hangTime)
                {
                    currState = STATE.Falling;
                }
            }

            if (velocity.y < 0)
            {
                currState = STATE.Falling;
            }

            //Update based on velocity
            newPosition += velocity*Time.deltaTime*defaultTimeSpeed;

            // Check for slamming into wall
            if (currState == STATE.Falling){
                RaycastHit2D hit = Physics2D.Raycast(transform.position+(newPosition-transform.position).normalized*(1.1f), newPosition-transform.position, (newPosition-transform.position).magnitude);

                if (currState == STATE.Falling && hit.collider != null)
                {
                    if (hit.collider != null && hit.collider.gameObject.transform.position.y < transform.position.y)
                    {
                        currState = STATE.Grounded;
                        velocity.y = 0;
                        acceleration.y = 0;
                        //Ensure we aren't embedded in the object (this is the bad/cheap way to do it, should be with raycasts)
                        Vector3 nonEmbeddedPos = transform.position;
                        nonEmbeddedPos.y = hit.collider.gameObject.transform.position.y+hit.collider.bounds.size.y/2f+boxCollider.bounds.size.y/2f;
                        transform.position = nonEmbeddedPos;
                        doubleJumped = false;

                        //Play Impact Sound
                        if(impactClip!=null && audioSource!=null){
                            audioSource.clip  = impactClip;
                            if(!audioSource.isPlaying){
                                audioSource.Play();
                            }
                        }
                    }
                }
            }
            else if(currState == STATE.Grounded){
                RaycastHit2D hit = Physics2D.Raycast(newPosition+Vector3.down*(1.1f)*boxCollider.bounds.size.y/2f, Vector3.down, 0.1f);

                if(hit.collider==null){
                    currState = STATE.Falling;
                    acceleration.y = -1*gravity;
                }
            }

            //Set new position
            transform.position = newPosition;
        }


        //Did we hit a platform?
        void OnCollisionEnter2D(Collision2D col)
        {
            if (currState == STATE.Falling)
            {
                if (col.gameObject.transform.position.y < transform.position.y)
                {
                    currState = STATE.Grounded;
                    velocity.y = 0;
                    acceleration.y = 0;
                    

                    //Impact!
                    if (impactTimeChangeTimer>0){
                        internalImpactTimeChangeTimer = impactTimeChangeTimer;
                        defaultTimeSpeed = timeSpeedOnImpact;
                    }

                    if(zoomOnImpact){
                        cameraControl.ZoomOnPlayer();
                    }
                    if(shakeOnImpact){
                        cameraControl.ShakeCamera();
                    }
                    if(rotateOnImpact){
                        cameraControl.RotateCamera();
                    }

                    //Play Impact Sound
                    if(impactClip!=null && audioSource!=null){
                        audioSource.clip  = impactClip;
                        if(!audioSource.isPlaying){
                            audioSource.Play();
                        }
                    }

                    
                    //Ensure we aren't embedded in the object (this is the bad/cheap way to do it, should be with raycasts)
                    Vector3 nonEmbeddedPos = transform.position;
                    nonEmbeddedPos.y = col.gameObject.transform.position.y+col.collider.bounds.size.y/2f+boxCollider.bounds.size.y/2f;
                    transform.position = nonEmbeddedPos;
                    doubleJumped = false;

                    //Set Impact Scale
                    transform.localScale = impactDimensions;
                    transform.position+=Vector3.down*(defaultDimensions.y-impactDimensions.y);

                    
                }
            }
        }
    }
}