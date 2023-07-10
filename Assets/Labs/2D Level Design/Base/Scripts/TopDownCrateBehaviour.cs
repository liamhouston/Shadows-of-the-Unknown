using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCrateBehaviour : TopDownEntityBehaviour
{

    private TopDownSokobanBehaviour sokobanScript;
    private TopDownPlayerBehaviour playerScript;
    private Rigidbody2D player;
    private Rigidbody2D crate;

    // ice parameters
    private Vector2 _closestIce = Vector2.positiveInfinity;
    private bool _isOnIce = false;
    private float _iceThreshold = 0.9f;
    private bool _lockForce = false;
    private Vector2 previousLocation;
    private Vector2 lastPlayerLocation;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();

        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        playerScript = (TopDownPlayerBehaviour)player.gameObject.GetComponent(typeof(TopDownPlayerBehaviour));

        crate = gameObject.GetComponent<Rigidbody2D>();
        sokobanScript = (TopDownSokobanBehaviour)player.gameObject.GetComponent(typeof(TopDownSokobanBehaviour));
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    override public void FixedUpdate()
    {
        base.FixedUpdate();

        updateIce();
        previousLocation = crate.position;
    }

    override public Vector2 getMovement()
    {
        Vector2 update = Vector2.zero;
       
        if (!_lockForce)
        {
            lastPlayerLocation = playerScript.dirToVec();
        }    
        // if we are on ice, we're moving in our current direction
        if (_isOnIce)
        {
            _lockForce = true;
            return lastPlayerLocation;
        }

        else if (!_isOnIce || isStopped())
        {
            _lockForce = false;
        }

        // handle player collision with crate
        if (Mathf.Abs(Vector2.Distance(player.position, transform.position)) <= 1f)
        {
            if (Mathf.Abs(Mathf.Abs(player.position.x) - Mathf.Abs(crate.position.x)) > Mathf.Abs(Mathf.Abs(player.position.y) - Mathf.Abs(crate.position.y)))
            {
                update = new Vector2(playerScript.dirToVec().x, 0);
            }
            else
            {
                update = new Vector2(0, playerScript.dirToVec().y);
            }
        }

        return update;
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

        if (Mathf.Abs(_closestIce.x - transform.position.x) <= _iceThreshold && Mathf.Abs(_closestIce.y - transform.position.y) <= _iceThreshold && !isStopped())
        {

            _isOnIce = true;
        }
        else{
            _isOnIce = false;
        }
    }

    public bool isStopped()
    {
        return (crate.position.x == previousLocation.x && crate.position.y == previousLocation.y);
    }
}
