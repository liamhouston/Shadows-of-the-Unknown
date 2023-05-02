using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlatformerScript : MonoBehaviour
{
    //ALL THE PUBLIC VARIABLES

    //Horizontal Movement Checks
    public float horizontalAcceleration = 10f;//how quickly it speeds up horizontally
    public float maxHorizontalVelocity = 5f;//how fast it can go horizontally
    public float groundFriction = 10f;//how quickly it slows down on the ground
    public float airFriction = 1f;//how quickly it slows down in the air
    public float airSpeedPercentage = 0.8f;//what percentage of ground speed does it have in the air

    //Vertical Movement Checks
    public float gravity = 40f;//Rate of gravity
    public float terminalVelocity = 20f;//How fast can it fall vertically
    public float kickOffAcceleration = 10f;//initial acceleration at the start of the jump
    public float maxVerticalVelocity = 20f;//max vertical velocity if you want to cap it
    public float hangTime = 0f;//how long to hang at the apex of the jump
    public bool doubleJump = false;


    //Input options
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode jump = KeyCode.Space;

    //Physics info
    private Vector3 velocity, acceleration;
    private float timer=0;
    private BoxCollider2D boxCollider;
    private bool doubleJumped = false;

    //State Machine Info
    private enum STATE {Falling, Rising, Hanging, Grounded};
    private STATE currState = STATE.Falling;

    //Speed at which time moves for this thing
    public float defaultTimeSpeed = 1f;
    public float timeSpeedOnImpact = 0.5f;
    public float impactTimeChangeTimer = 1f;
    public float timeSpeedOnJump = 0.5f;
    public float jumpTimeChangeTimer = 1f;

    //Various timers
    private float internalImpactTimeChangeTimer = 0f;
    private float internalJumpTimeChangeTimer = 0f;

    //Internal time trackers
    private float internalDefaultTimeSpeed = 0f;

    //Camera Control
    public CameraControl cameraControl;
    public bool zoomOnImpact;  
    public bool shakeOnImpact;
    public bool rotateOnImpact;

    //Audio Stuff
    public AudioSource audioSource;
    public AudioClip jumpClip, impactClip;

    //Visual Stuff
    public Vector3 jumpDimensions;
    public Vector3 impactDimensions;
    private Vector3 defaultDimensions; 

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

    //Did we just walk off a cliff?
    void OnCollisionExit2D(Collision2D col)
    {
        if(currState == STATE.Grounded)
        {
            currState = STATE.Falling;
            acceleration.y = -1*gravity;
        }
    }

    //Really cheap/lazy physics simulation
    private void CheapPhysicsSimulation(float percentageToUse)
    {
        if (Mathf.Abs(acceleration.x) > 0f)
        {
            //Accelerate up to max horizontal velocity
            if (Mathf.Abs(velocity.x) < maxHorizontalVelocity)
            {
                velocity.x += acceleration.x * Time.deltaTime* percentageToUse*defaultTimeSpeed;
            }
            //Ensure maximum velocity
            if (velocity.x < maxHorizontalVelocity * -1 * percentageToUse)
            {
                velocity.x = maxHorizontalVelocity * -1 * percentageToUse;
            }
            else if (velocity.x > maxHorizontalVelocity * percentageToUse)
            {
                velocity.x = maxHorizontalVelocity * percentageToUse;
            }
        }
        else//Friction Checks
        {
            float friction = airFriction;

            if (currState == STATE.Grounded)
            {
                friction = groundFriction;
            }

            if (Mathf.Abs(velocity.x) > 0f)
            {
                //If greater than 0 decrease. If less then 0 increase.
                if (velocity.x > 0)
                {
                    velocity.x -= friction * Time.deltaTime*defaultTimeSpeed;

                    //Ensure that we didn't flipflop
                    if (velocity.x < -0.01f)
                    {
                        velocity.x = 0f;
                    }
                }
                else
                {
                    velocity.x += friction * Time.deltaTime*defaultTimeSpeed;

                    //Ensure that we didn't flipflop
                    if (velocity.x > 0.01f)
                    {
                        velocity.x = 0f;
                    }
                }

                //Cut off point due to float issues
                if (Mathf.Abs(velocity.x) < 0.01f)
                {
                    velocity.x = 0;
                }
            }
        }
    }
}