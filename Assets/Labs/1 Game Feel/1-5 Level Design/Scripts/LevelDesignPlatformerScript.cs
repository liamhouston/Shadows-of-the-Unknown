using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFeel{
    public class LevelDesignPlatformerScript : AudioPlatformerScriptV2
    {
        [Header("Level Settings")]
        public AudioSource audioSourceMusic; // audio source for background music
        public AudioClip backgroundMusic; // clip for background music

        protected override void Start(){
            base.Start();
            if (audioSourceMusic != null && backgroundMusic != null){
                audioSourceMusic.clip = backgroundMusic;
                audioSourceMusic.Play();
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            
        }

        protected override void OnGrounded_Hook(){
            Vector2 extents = _playerCollider.bounds.extents;
            Vector2 rayPosition = new Vector2(transform.position.x, transform.position.y - extents.y); // reminder to unhack once yalmaz makes collider protected
            RaycastHit2D checkValid = Physics2D.Raycast(rayPosition, Vector2.down);

            Debug.Log(checkValid.collider);
            if (checkValid.collider && checkValid.collider != ground){
                checkValid.collider.gameObject.SendMessage("onContact");
            }

        }
    }


    
}