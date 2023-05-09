using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFeel{
    public class AudioPlatformerScript : VisualPlatformerScript
    {
        // Audio Stuff
        public AudioSource audioSourceAction; // controls singly played audio events: jump and impact
        public AudioSource audioSourceMove; // controls looping audio events: ground and air movement, falling, rising, etc

        public AudioClip jumpClip, doubleJumpClip, impactClip; // action clips
        public AudioClip groundMoveClip, airRisingClip, airFallingClip; // movement clips

        protected STATE prevState = STATE.Falling;
        protected bool prevDoubleJumped = false;

        // Update is called once per frame
        protected override void Update()
        {
            prevState = currState;
            prevDoubleJumped = doubleJumped;
            base.Update();

            // jump cases: either we are grounded, or we are double jumping
            if ((Input.GetKeyDown(jump) && prevState == STATE.Grounded)){
                // play jump sound: currently doesn't self-overlap, but can if that's desired
                if (jumpClip != null && audioSourceAction != null){
                    audioSourceAction.clip  = jumpClip;
                    if (!audioSourceAction.isPlaying){
                        audioSourceAction.Play();
                    }
                }  
            }

            if (Input.GetKeyDown(jump) && doubleJumped != prevDoubleJumped && doubleJumped == true){
                if (doubleJumpClip != null && audioSourceAction != null){
                    audioSourceAction.clip  = doubleJumpClip;
                    if (!audioSourceAction.isPlaying){
                        audioSourceAction.Play();
                    }
                }  
            }

            // impact case: if we were previously falling, and now we are grounded, we have hit the ground
            if (currState == STATE.Grounded && prevState == STATE.Falling){
                if (impactClip!=null && audioSourceAction!=null){
                    audioSourceAction.clip  = impactClip;
                    if(!audioSourceAction.isPlaying){
                        audioSourceAction.Play();
                    }
                }
            }

            // Handle looping sounds for falling
            if (currState == STATE.Falling)
            {
                // play falling sound
                if (airFallingClip != null && audioSourceMove != null){
                    audioSourceMove.clip = airFallingClip;
                    if(!audioSourceMove.isPlaying){
                        audioSourceMove.Play();
                    }
                }
                // cut off audio if no associated sound
                else if (audioSourceMove != null){
                    audioSourceMove.Stop();
                }
            }

            // Handle looping sounds for rising
            if (currState == STATE.Rising)
            {
                // play rising sound
                if (airRisingClip != null && audioSourceMove != null){
                    audioSourceMove.clip = airRisingClip;
                    if(!audioSourceMove.isPlaying){
                        audioSourceMove.Play();
                    }
                }
                // cut off audio if no associated sound
                else if (audioSourceMove != null){
                    audioSourceMove.Stop();
                }
            }

            // Handle looping sounds for grounded movement, and ground movement start/stop sounds
            if (currState == STATE.Grounded){
                if (velocity.x != 0){
                    // play ground movement sound
                    if (audioSourceMove != null && groundMoveClip != null){
                        audioSourceMove.clip = groundMoveClip;
                        if(!audioSourceMove.isPlaying){
                            audioSourceMove.Play();
                        }
                    }
                    else if (audioSourceMove != null){
                        audioSourceMove.Stop();
                    }
                }
                else{
                    audioSourceMove.Stop();
                }
            }
        }

        //Did we hit a platform?
        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            if (currState == STATE.Falling)
            {
                if (col.gameObject.transform.position.y < transform.position.y)
                {
                    //Play Impact Sound
                    if(impactClip!=null && audioSourceAction!=null){
                        audioSourceAction.clip  = impactClip;
                        if(!audioSourceAction.isPlaying){
                            audioSourceAction.Play();
                        }
                    }
                }
            }
        }
    }
}