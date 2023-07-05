using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCrateBehaviour : MonoBehaviour
{

    private TopDownSokobanBehaviour sokobanScript;
    private Rigidbody2D player;
    private Rigidbody2D rb;

    // ice parameters
    private Vector2 _closestIce = Vector2.positiveInfinity;
    private bool _isOnIce = false;
    private float _iceThreshold = 0.9f;
    private bool _lockForce = false;
    private Vector2 latestVelocity;

    // Start is called before the first frame update
    void Start()
    {
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        rb = gameObject.GetComponent<Rigidbody2D>();
        sokobanScript = (TopDownSokobanBehaviour)player.gameObject.GetComponent(typeof(TopDownSokobanBehaviour));
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        updateIce();

        // Check if we are on ice and get latest velocity before being on Ice
        if (_isOnIce && !isStopped()) { 
            if (!_lockForce)
            {
                latestVelocity = rb.velocity;
                _lockForce = true;
            }

            // Check if we are going faster in the x direction or y direction and move on ice with highest direction
            // This should prevent diagonal ice movement
            if (Mathf.Abs(latestVelocity.x) >= Mathf.Abs(latestVelocity.y) && _lockForce)
            {
                rb.velocity = new Vector2(Mathf.Sign(latestVelocity.x) * 15, 0);
            }
            else if (Mathf.Abs(latestVelocity.x) < Mathf.Abs(latestVelocity.y) && _lockForce)
            {

                rb.velocity = new Vector2(0, Mathf.Sign(latestVelocity.y) * 15);
            }
        }
        if (!_isOnIce)
        {
            if (_lockForce)
            {
                _lockForce = false;
            }
        }
    }

    //Just overlapped a collider 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            sokobanScript.DecrementGoals();
            sokobanScript.DecrementCrates();

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void updateIce()
    {
        Vector2 icePos;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ice"))
        {
            icePos = go.transform.position;
            if (Mathf.Abs(Vector2.Distance(transform.position, icePos)) < Mathf.Abs(Vector2.Distance(transform.position, _closestIce)))
            {
                _closestIce = icePos;
            }
        }

        if (Mathf.Abs(_closestIce.x - transform.position.x) <= _iceThreshold && Mathf.Abs(_closestIce.y - transform.position.y) <= _iceThreshold)
        {

            _isOnIce = true;
        }
        else
        {
            _isOnIce = false;
        }
    }

    public bool isStopped()
    {
        return (rb.velocity.x == 0 && rb.velocity.y == 0);
    }
}
