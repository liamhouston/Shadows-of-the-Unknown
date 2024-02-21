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
        public Camera mainCamera;

        private CinemachineVirtualCamera cvc;


        public void Start(){
            cvc = GetComponentInChildren<CinemachineVirtualCamera>();
            Debug.Assert(cvc != null, "Must have Cinemachine Virtual Camera component alongside Camera Controller object.");
            Debug.Assert(mainCamera != null, "Must have main camera attached.");
        }


        public void Update() {
            // Built in Unity input system uses arrows or wasd
            float horizontalInput = Input.GetAxis("Horizontal"); // ranges from -1 left to 1 right
            float verticalInput = Input.GetAxis("Vertical"); // ranges from -1 down to 1 up

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;


            if (IsValid(newPosition)){
                transform.position = newPosition;
            }
            else {
                // light camera shake to indicate edge
                // GameController.Instance.StartShake(1,1,0.5f);
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
            
            // We need to consider the size of the virtual camera when determining if a position is valid

            // Orthographic size determines the amount of units from the center of the screen to the top. For example, the virtual camera has an ortho size of 4 which means you can see a total of 8 units in the y direction.
            float aspectRatio = (float) mainCamera.pixelWidth / mainCamera.pixelHeight;
            
            float y_offset = cvc.m_Lens.OrthographicSize;
            float x_offset = y_offset * aspectRatio;

            bool validity = true;
            validity &= ( (pos.x - x_offset) >= left_bound); // validity stays true if we can't see past left bound
            validity &= ( (pos.x + x_offset) <= right_bound); // can't see past right bound
            validity &= ( (pos.y - y_offset) >= bottom_bound); // can't see below bottom
            validity &= ( (pos.y + y_offset) <= top_bound); // can't see above

            return validity;
        }
}