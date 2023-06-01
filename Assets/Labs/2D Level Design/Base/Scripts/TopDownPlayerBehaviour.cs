using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TopDownPlayerBehaviour : TopDownEntityBehaviour
{
    [Header("Input Bindings")]
    [SerializeField] public KeyCode left = KeyCode.LeftArrow;
    [SerializeField] public KeyCode right = KeyCode.RightArrow;
    [SerializeField] public KeyCode up = KeyCode.UpArrow;
    [SerializeField] public KeyCode down = KeyCode.DownArrow;
    [SerializeField] public KeyCode attack = KeyCode.Space;
    [SerializeField] public KeyCode reset = KeyCode.R;

    // public variables
    public float attackCooldown = 2f;

    // internal variables    
    // attack parameters
    private bool _canAttack = true;
    private float _attackThreshold = 1.5f;
    private float _attackCountdown;

    // key parameters
    private int _keys = 0;
    
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        _attackCountdown = attackCooldown;
    }

    // Update is called once per frame
    override public void FixedUpdate()
    {
        base.FixedUpdate();

        // handle attack cooldown
        if (!_canAttack){
            _attackCountdown -= Time.deltaTime;
            if (_attackCountdown <= 0f){
                _canAttack = true;
                _attackCountdown = attackCooldown;
            }
        }

        // handle attack and damage collisions
        if (Input.GetKey(attack)){
            handleAttack();
        }

        // reset button
        if (Input.GetKey(reset)){
            handleDeath();
        }

        
    }

    override public Vector2 getMovement(){
        Vector2 update = Vector2.zero;
        // read input, and set our direction accordingly
        if (Input.GetKey(right)){
            update.x = 1;
            _currDir = Direction.East;
        }
        else if (Input.GetKey(left)){
            update.x = -1;
            _currDir = Direction.West;
        }
        else if (Input.GetKey(up)){
            update.y = 1;
            _currDir = Direction.North;
        }
        else if (Input.GetKey(down)){
            update.y = -1;
            _currDir = Direction.South;
        }
        return update;
    }

    void handleAttack(){
        // if we can't attack, don't
        if (!_canAttack) { return; }

        // shoot out a ray looking to ATTACK
        _canAttack = false;
        Vector2 attackDir = dirToVec();

        RaycastHit2D attackRay = Physics2D.Raycast(rb.position, attackDir);
        if (attackRay.collider && attackRay.distance < _attackThreshold){
            Debug.Log("hit!");
            Debug.Log(_currDir);

            attackRay.collider.gameObject.SendMessage("takeDamage");

        }
        else{
            Debug.Log("no hit!");
        }
    }

    override public void handleDeath(){
        // todo: death anim / sound, wait on anim/sound
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int getKeys(){
        return _keys;
    }

    public void unlockDoor(){
        _keys--;
    }

    public void collectKey(){
        _keys++;
    }
}
