using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFeel{
public class AnimationPlatformerScript : VisualPlatformerScriptV2
{

    [Header("Annimation Settings")]
    [SerializeField,Range(0.25f,4f)] protected float walkAnimationSpeedMultiplier = 1.0f;   //How fast the walking animation will play

    private Animator animator;              //Reference to the animator component
    private SpriteRenderer spriteRenderer;  //Reference to the sprite renderer component

    protected override void Start() {
        base.Start();
        //Obtain references to the needed components
        GameObject spriteObject = transform.GetChild(0).gameObject;
        animator = spriteObject.GetComponent<Animator>();
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Update the animation state
        animator.SetBool("isGrounded",currState == STATE.Grounded);
        if(currState == STATE.Grounded && _horizontalInput != 0){
            animator.SetBool("isWalking",true);
        }
        else{
            animator.SetBool("isWalking",false);
        }
        animator.SetFloat("yVelocity",_currentVelocity.y);

        //Update animation speed multipliers
        animator.speed = walkAnimationSpeedMultiplier;
        //note: currently only the walk has more than one frame, will require change in logic if that change

        //Update the sprite facing direction
        if(_horizontalInput > 0){
            spriteRenderer.flipX = false;
        }
        else if(_horizontalInput < 0){
            spriteRenderer.flipX = true;
        }
    }

}
}