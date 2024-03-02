using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : AnimatedEntity
{
    public float speed = 1f; // Adjust speed as needed

    private float angle = 0f;
    private float x_offset; // start each fly at a different
    private float y_offset;

    void Start() {
        AnimationSetup();
        System.Random random = new System.Random();
        x_offset = (float)random.NextDouble()*3;
        y_offset = (float)random.NextDouble()*3;
    }

    private void Update()
    {
        AnimationUpdate();
        // Update the angle based on time and speed
        angle += Time.deltaTime * speed;

        // Calculate the position using sine and cosine functions
        System.Random random = new System.Random();

        float x = Mathf.Sin(angle + x_offset) + (float)(random.NextDouble()) * 2 - 1;
        float y = Mathf.Cos(2*(angle + y_offset))  + (float)(random.NextDouble()) * 2 - 1;

        // flip fly direction (sprites face right by default)
        if (x < 0 && transform.localScale.x == 1){
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (x > 0 && transform.localScale.x == -1){
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        // Update the position of the fly
        
        transform.position += new Vector3(x, y, 0) / 50;
    }
}
