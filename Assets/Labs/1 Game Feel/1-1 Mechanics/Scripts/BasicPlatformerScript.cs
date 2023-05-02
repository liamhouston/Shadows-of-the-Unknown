using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFeel{
    public class BasicPlatformerScript : MonoBehaviour
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
        protected Vector3 velocity, acceleration;
        protected float timer=0;
        protected BoxCollider2D boxCollider;
        protected bool doubleJumped = false;

        //State Machine Info
        protected enum STATE {Falling, Rising, Hanging, Grounded};
        protected STATE currState = STATE.Falling;


        void Start() {
            boxCollider = GetComponent<BoxCollider2D>();//For size of collider purposes
            acceleration.y = -1 * gravity;//Assuming you start in the air
        }

       
        // Update is called once per frame
        void Update()
        {
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
            }
            else if (Input.GetKeyDown(jump) && doubleJump && !doubleJumped)//Double jump check
            {
                doubleJumped = true;
                currState = STATE.Rising;
                timer = 0;
                acceleration.y = kickOffAcceleration;
                velocity.y = acceleration.y;
            }

            //Cheap physics simulation
            CheapPhysicsSimulation(percentageForAirOrGround);

            //State-based movement
            if (currState == STATE.Falling)
            {
                acceleration.y -= Time.deltaTime * gravity;
                //Speeding up to terminal velocity
                if (velocity.y > -1 * terminalVelocity)
                {
                    velocity.y += Time.deltaTime * acceleration.y;
                    velocity.y = Mathf.Max(velocity.y, -1 * terminalVelocity);
                }
            }
            if(currState == STATE.Rising)
            {
                acceleration.y-= Time.deltaTime * gravity;
                velocity.y += Time.deltaTime * acceleration.y;
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
                    timer += Time.deltaTime;
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
            newPosition += velocity*Time.deltaTime;

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
                    //Ensure we aren't embedded in the object (this is the bad/cheap way to do it, should be with raycasts)
                    Vector3 nonEmbeddedPos = transform.position;
                    nonEmbeddedPos.y = col.gameObject.transform.position.y+col.collider.bounds.size.y/2f+boxCollider.bounds.size.y/2f;
                    transform.position = nonEmbeddedPos;
                    doubleJumped = false;
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
        protected void CheapPhysicsSimulation(float percentageToUse)
        {
            if (Mathf.Abs(acceleration.x) > 0f)
            {
                //Accelerate up to max horizontal velocity
                if (Mathf.Abs(velocity.x) < maxHorizontalVelocity)
                {
                    velocity.x += acceleration.x * Time.deltaTime* percentageToUse;
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
                        velocity.x -= friction * Time.deltaTime;

                        //Ensure that we didn't flipflop
                        if (velocity.x < -0.01f)
                        {
                            velocity.x = 0f;
                        }
                    }
                    else
                    {
                        velocity.x += friction * Time.deltaTime;

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
}
