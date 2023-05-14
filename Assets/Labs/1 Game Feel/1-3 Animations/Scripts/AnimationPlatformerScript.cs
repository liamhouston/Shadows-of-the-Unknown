using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFeel{
public class AnimationPlatformerScript : VisualPlatformerScript
{

private Animator animator;              //Reference to the animator component
private SpriteRenderer spriteRenderer;  //Reference to the sprite renderer component

void Start() {
    base.Start();
    //Obtain references to the needed components
    GameObject spriteObject = transform.GetChild(0).gameObject;
    animator = spriteObject.GetComponent<Animator>();
    spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
}


// Update is called once per frame
        void Update()
        {
            base.Update();

            //Update the animation state
            animator.SetBool("isGrounded",currState == STATE.Grounded);
            if(currState == STATE.Grounded && (Input.GetKey(left) || Input.GetKey(right))){
                animator.SetBool("isWalking",true);
            }
            else{
                animator.SetBool("isWalking",false);
            }
            animator.SetFloat("yVelocity",velocity.y);


            //Update the sprite facing direction
            if(Input.GetKey(right)){
                spriteRenderer.flipX = false;
            }
            else if(Input.GetKey(left)){
                spriteRenderer.flipX = true;
            }
        }

}
}