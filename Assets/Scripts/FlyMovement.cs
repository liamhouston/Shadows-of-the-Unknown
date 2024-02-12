using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : AnimatedEntity
{
    public Transform targetPoint; // The point the flies should stay close to


    private Vector3 currentVelocity;
    private Vector3 targetPosition;

    void Start() {
        AnimationSetup();
    }

    private void Update()
    {
        AnimationUpdate();

    }



}
