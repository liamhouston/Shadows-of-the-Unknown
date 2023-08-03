using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownEntityBehaviour : MonoBehaviour
{
    public enum Direction {North, East, South, West};

    // public variables
    public float moveSpeed = 10f;
    public float invincibilityTime = 1f;

    // internal variables
    private float _collisionBuffer = 0.1f;
    protected Direction _currDir = Direction.North;
    protected int _health = 3;

    // invincibility
    private float _invTimer;
    private bool _invincible = false;

    // components
    public Rigidbody2D rb;
    public BoxCollider2D _collider;

    // private components
    private SpriteRenderer renderer;
    private Color baseColor;
    private Color invColor;
    
    // Start is called before the first frame update
    virtual public void Start()
    {
        // initialize variables
        _invTimer = invincibilityTime;
        renderer = (SpriteRenderer)this.gameObject.transform.GetChild(0).gameObject.GetComponent("SpriteRenderer");
        baseColor = renderer.material.color;
        
        // set up transparency
        Color temp = baseColor;
        temp.a *= 0.5f;
        invColor = temp;
    }

    // Update is called once per frame
    virtual public void FixedUpdate()
    {
        // move according to getMovement rule, and collide with collision objects
        Vector2 update = getMovement();
        moveAndCollide(update);

        // handle invulnerability timer and transparency
        if (_invincible){
            _invTimer -= Time.deltaTime;
            // reset invuln
            if (_invTimer <= 0){
                _invincible = false;
                renderer.material.color = baseColor;
                _invTimer = invincibilityTime;
            }
            else{
                renderer.material.color = invColor;
            }
        }
    }

    // overridden in children to give movement rules for objects
    virtual public Vector2 getMovement(){
        Vector2 update = Vector2.zero;
        return update;
    }

    // called when damage is taken
    virtual public void takeDamage(){
        // don't take damage if we are invincible
        if (_invincible) { return; }

        // otherwise, take damage and start invinc timer
        _health--;
        if (_health <= 0){
            handleDeath();
        }
        _invincible = true;
    }

    // called on death
    virtual public void handleDeath(){ return; }

    // controls movement and collision
    void moveAndCollide(Vector2 update){
        // if we aren't moving, don't move!
        if (update == Vector2.zero){ return; }

        // move, with respect for collisions
        Vector2 position = rb.position;
        Vector2 extents = _collider.bounds.extents;
        Vector2 flush = Vector2.zero;

        // corners for raycasting
        Vector2 topRight = new Vector2(position.x + extents.x, position.y + extents.y);
        Vector2 topLeft = new Vector2(position.x - extents.x, position.y + extents.y);
        Vector2 bottomRight = new Vector2(position.x + extents.x, position.y - extents.y);
        Vector2 bottomLeft = new Vector2(position.x - extents.x, position.y - extents.y);

        // perform 2 raycasts in total from appropriate corners to check collision
        // raycasting is used due to issues with tilemap colliders

        //    ^  ^
        //    |  |
        // <- *--* ->
        //    |  |
        // <- *--* ->
        //    |  |
        //    V  V
        
        // horizontal, right
        if (update.x == 1){
            RaycastHit2D tr = Physics2D.Raycast(topRight, Vector2.right);
            RaycastHit2D br = Physics2D.Raycast(bottomRight, Vector2.right);

            if (collided(tr) || collided(br)){
                update.x = 0;
                if (collided(tr)){
                    flush.x = tr.distance;
                }
                if (collided(br)){
                    flush.x = br.distance;
                }
            }
        }
        // horizontal, left
        else if (update.x == -1){
            RaycastHit2D tl = Physics2D.Raycast(topLeft, Vector2.left);
            RaycastHit2D bl = Physics2D.Raycast(bottomLeft, Vector2.left);

            if (collided(tl) || collided(bl)){
                update.x = 0;
                if (collided(tl)){
                    flush.x = tl.distance;
                }
                if (collided(bl)){
                    flush.x = bl.distance;
                }
            }
        }

        // vertical, up
        if (update.y == 1){
            RaycastHit2D tr = Physics2D.Raycast(topRight, Vector2.up);
            RaycastHit2D tl = Physics2D.Raycast(topLeft, Vector2.up);

            if (collided(tr) || collided(tl)){
                update.y = 0;
                if (collided(tr)){
                    flush.y = tr.distance;
                }
                if (collided(tl)){
                    flush.y = tl.distance;
                }
            }
        }
        // vertical, down
        else if (update.y == -1){
            RaycastHit2D br = Physics2D.Raycast(bottomRight, Vector2.down);
            RaycastHit2D bl = Physics2D.Raycast(bottomLeft, Vector2.down);

            if (collided(br) || collided(bl)){
                update.y = 0;
                if (collided(br)){
                    flush.y = br.distance;
                }
                if (collided(bl)){
                    flush.y = bl.distance;
                }
            }
        }

        // update position and clamp according to flush raycasts
        Vector2 newPosition = position + update * moveSpeed * Time.deltaTime;
        
        if (flush.x != 0){
            newPosition.x = Mathf.Clamp(newPosition.x, position.x - flush.x, position.x + flush.x);
        }
        if (flush.y != 0){
            newPosition.y = Mathf.Clamp(newPosition.y, position.y - flush.y, position.y + flush.y);
        }

        rb.MovePosition(newPosition);
    }

    // helper function for collisions
    bool collided(RaycastHit2D result){
        return (result.collider && result.distance < _collisionBuffer && !result.collider.isTrigger);
    }

    // helper function to take direction and get a corresponding vector
    public Vector2 dirToVec(){
        Vector2 update = Vector2.zero;
        switch (_currDir){
            case Direction.North:
                update.y = 1;
                break;
            case Direction.South:
                update.y = -1;
                break;
            case Direction.East:
                update.x = 1;
                break;
            case Direction.West:
                update.x = -1;
                break;
            default:
                break;
        }
        return update;
    }

    public int getHealth(){
        return _health;
    }

    public void setInvincible(bool inv){
        _invincible = inv;
    }
}
