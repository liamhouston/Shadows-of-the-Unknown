using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

    public class CameraController : MonoBehaviour {
        [Header ("Camera Settings")]
        public float speed = 5f;
        [Header ("Camera Confinement")]
        public Transform backgroundTransform;
        public SpriteRenderer backgroundSpriteRenderer;


        public void Update() {
            // Built in Unity input system uses arrows or wasd
            float horizontalInput = Input.GetAxis("Horizontal"); // ranges from -1 left to 1 right
            float verticalInput = Input.GetAxis("Vertical"); // ranges from -1 down to 1 up

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;

            if (IsValid(newPosition)){
                Debug.Log("moved to new position : " + newPosition);
                transform.position = newPosition;
            }
            else {
                Debug.Log("confider says camera cant move there");
            }
        }

    // Given a position, checks that this position is valid in the bounds of the camera
    public bool IsValid(Vector3 pos){
        float horz_size = backgroundSpriteRenderer.bounds.size.x;
        float vert_size = backgroundSpriteRenderer.bounds.size.y;

        float left_bound = backgroundTransform.position.x - (horz_size / 2.0f);
        float right_bound = backgroundTransform.position.x + (horz_size / 2.0f); 
        float top_bound = backgroundTransform.position.y + (vert_size / 2.0f);
        float bottom_bound = backgroundTransform.position.y - (vert_size / 2.0f);

        return pos.x >= left_bound && pos.x <= right_bound && pos.y >= bottom_bound && pos.y <= top_bound;
    }
}