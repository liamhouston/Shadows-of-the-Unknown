using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesignPlatformScript : MonoBehaviour
{   
    public enum DIRECTION { Horizontal, Vertical};
    [Header("Platform Movement")]
    public bool moving; // turns platform movement on or off
    //public DIRECTION movementOption = DIRECTION.Horizontal; // controls whether we are moving horizontally or vertically 
    public float speed; // speed of moving platform
    public float distanceFromOrigin; // bounds the platform to be [-distance, distance] away from origin
    
    [Header("On Landing Behaviour")]
    public AudioSource audioSourcePlatform; // audio source for platform landing sound
    public AudioClip platformLandingClip; // audio clip for platform landing sound
    public bool disappear; // disappear after a given time when player lands
    public float disappearTimer; // controls how long it takes the platform to disappear

    // platform movement
    protected float dir = 1.0f;
    protected float bound1;
    protected float bound2;

    // platform disappearance
    protected bool disappearing;

    // components
    private BoxCollider2D collider;
    private SpriteRenderer renderer;

    void Start(){
        // set platform bounds
        bound1 = transform.position.x - distanceFromOrigin;
        bound2 = transform.position.x + distanceFromOrigin;
        collider = (BoxCollider2D)GetComponent("BoxCollider2D");
        renderer = (SpriteRenderer)GetComponent("SpriteRenderer");
    }
     

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        if (moving){
            newPosition = horizontalMovement();
        }
        if (disappearing){
            disappearTimer -= Time.deltaTime;
            if (disappearTimer <= 0.0f){
                collider.enabled = false;
                renderer.enabled = false;
                disappearing = false;
            }
        }
        transform.position = newPosition;
    }

    Vector3 horizontalMovement(){
        Vector3 newPosition = transform.position;
        // perform movement & clamp w.r.t. bounds
        newPosition.x += speed * Time.deltaTime * dir;
        newPosition.x = Mathf.Clamp(newPosition.x, bound1, bound2);

        if (Mathf.Sign(dir) == 1 && newPosition.x == bound2){
            dir *= -1.0f;
        }
        else if (Mathf.Sign(dir) == -1 && newPosition.x == bound1){
            dir *= -1.0f;
        }
        return newPosition;
    }

    void onContact(){
        if (disappear){
            moving = false;
            disappearing = true;
        }

        if (audioSourcePlatform != null && platformLandingClip != null){
            audioSourcePlatform.clip = platformLandingClip;
            if (!audioSourcePlatform.isPlaying){
                audioSourcePlatform.Play();
            }
        }
    }

    
}
