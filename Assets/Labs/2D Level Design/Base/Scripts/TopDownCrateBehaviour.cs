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

    // sound parameters
    [SerializeField] private AudioSource boxSoundSource;
    [SerializeField] private AudioClip boxGroundPush;
    [SerializeField] private AudioClip boxIceSlide;

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
            boxSoundSource.volume = 1f;
            boxSoundSource.clip = boxIceSlide;
            if (!boxSoundSource.isPlaying){
                boxSoundSource.Play();
            }
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
            // check if our position matches with the player
            Vector2 position = rb.position;
            Vector2 extents = _collider.bounds.extents;

            // each of the sides for checking against the player 
            Vector2 right = new Vector2(position.x + extents.x, position.y);
            Vector2 left = new Vector2(position.x - extents.x, position.y);
            Vector2 up = new Vector2(position.x, position.y + extents.y);
            Vector2 down = new Vector2(position.x, position.y - extents.y);
            
            Vector2 rightDir = new Vector2(1, 0);
            Vector2 leftDir = new Vector2(-1, 0);
            Vector2 upDir = new Vector2(0, 1);
            Vector2 downDir = new Vector2(0, -1);

            float rightDist = Vector2.Distance(right, player.position);
            float leftDist = Vector2.Distance(left, player.position);
            float upDist = Vector2.Distance(up, player.position);
            float downDist = Vector2.Distance(down, player.position);

            float minDist = Mathf.Min(rightDist, leftDist, upDist, downDist);
            Vector2 playerDirection = playerScript.dirToVec();

            // according to case, check if we should be allowed to push the block!
            if (minDist == rightDist && playerDirection == leftDir){
                update = new Vector2(-1, 0);
            }
            else if (minDist == leftDist && playerScript.dirToVec() == rightDir){
                update = new Vector2(1, 0);
            }
            else if (minDist == upDist && playerScript.dirToVec() == downDir){
                update = new Vector2(0, -1);
            }
            else if (minDist == downDist && playerScript.dirToVec() == upDir){
                update = new Vector2(0, 1);
            }
        }

        // if we have an update, we need to be making the move sound!
        if (previousLocation != (Vector2)transform.position){
            boxSoundSource.clip = boxGroundPush;
            boxSoundSource.volume = 1f;
            if (!boxSoundSource.isPlaying){
                boxSoundSource.Play();
            }
        }
        // if the box is stationary, stop all sound!
        else{
            boxSoundSource.Stop();
            boxSoundSource.volume = Mathf.Clamp(boxSoundSource.volume - 0.1f, 0f, 1f);
            if (boxSoundSource.volume == 0f){
                boxSoundSource.Stop();
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
