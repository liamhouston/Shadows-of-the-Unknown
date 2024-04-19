using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : AnimatedEntity
{
    public float rand_speed = 4f; // Adjust speed as needed
    public float min_speed = 1f;
    private float speed;

    public float radius = 5f;
    private float close = 0.2f;
    private Vector2 origPos;
    private Vector2 targetPos;

    private System.Random random = new System.Random();

    void Start() {
        AnimationSetup();

        origPos = transform.position;
        speed = min_speed + rand_speed*(float)random.NextDouble();

        GenerateNewTargetPosition();
    }

    private void Update()
    {
        AnimationUpdate();

        if (Vector2.Distance(transform.position, targetPos) < close){
            GenerateNewTargetPosition();
        }
        
        // Move towards the target
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        transform.position += new Vector3((float) (0.1 * random.NextDouble()), (float) (0.1 * random.NextDouble()), 0);

        // flip fly direction (sprites face right by default)
        if (targetPos.x - transform.position.x > 0 && transform.localScale.x < -1){
            // flip right
            transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (targetPos.x - transform.position.x < 0 && transform.localScale.x > 1){
            // flip left
            transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void GenerateNewTargetPosition()
    {
        // Generate a random angle within a full circle
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        // Calculate a point within the radius based on the angle
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        
        // Set the target position
        targetPos = origPos + new Vector2(x, y);
    }
}
