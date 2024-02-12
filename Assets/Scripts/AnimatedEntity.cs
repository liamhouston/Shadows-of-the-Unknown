using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEntity : MonoBehaviour
{
    public List<Sprite> DefaultAnimationCycle; // Standing (idle) animation frames
    public List<Sprite> WalkingAnimationCycle; // Walking animation frames
    public float Framerate = 12f; // Frames per second
    public SpriteRenderer SpriteRenderer; // SpriteRenderer

    private float animationTimer;
    private float animationTimerMax;
    private int index;

    private bool interruptFlag;
    private List<Sprite> interruptAnimation;

    // New variables to handle different states
    private enum AnimationState { Idle, Walking }
    private AnimationState currentState = AnimationState.Idle;

    protected void AnimationSetup()
    {
        animationTimerMax = 1.0f / Framerate;
        index = 0;
    }

    protected void AnimationUpdate()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer > animationTimerMax)
        {
            animationTimer = 0;
            index++;

            if (!interruptFlag)
            {
                switch (currentState)
                {
                    case AnimationState.Idle:
                        PlayAnimationCycle(DefaultAnimationCycle);
                        break;
                    case AnimationState.Walking:
                        PlayAnimationCycle(WalkingAnimationCycle);
                        break;
                }
            }
            else
            {
                // Handle interrupt animation
                if (interruptAnimation == null || index >= interruptAnimation.Count)
                {
                    index = 0;
                    interruptFlag = false;
                    interruptAnimation = null;
                }
                else
                {
                    SpriteRenderer.sprite = interruptAnimation[index];
                }
            }
        }
    }

    private void PlayAnimationCycle(List<Sprite> animationCycle) {
        if (animationCycle.Count == 0 || index >= animationCycle.Count){
            index = 0;
        }
        if (animationCycle.Count > 0){
            SpriteRenderer.sprite = animationCycle[index];
        }
    }

    protected void Interrupt(List<Sprite> _interruptAnimation)
    {
        interruptFlag = true;
        animationTimer = 0;
        index = 0;
        interruptAnimation = _interruptAnimation;
        SpriteRenderer.sprite = interruptAnimation[index];
    }

    // Call this method to switch to walking animation
    public void setStateWalk()
    {
        currentState = AnimationState.Walking;
    }

    // Call this method to switch to idle animation
    public void setStateIdle()
    {
        currentState = AnimationState.Idle;
        index = 0; // Reset index for smooth transition
    }
}
