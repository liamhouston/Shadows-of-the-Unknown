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

    [Header("Sprite Settings")]
    [SerializeField] private Sprite idleSpriteUp;
    [SerializeField] private List<Sprite> walkSpritesUp = new List<Sprite>(6);
    [SerializeField] private Sprite idleSpriteRight;
    [SerializeField] private List<Sprite> walkSpritesRight = new List<Sprite>(6);
    [SerializeField] private Sprite idleSpriteDown;
    [SerializeField] private List<Sprite> walkSpritesDown = new List<Sprite>(6);
    [SerializeField] private Sprite idleSpriteLeft;
    [SerializeField] private List<Sprite> walkSpritesLeft = new List<Sprite>(6);

    // internal variables    
    // attack parameters
    private bool _canAttack = true;
    private float _attackThreshold = 1.5f;
    private float _attackCountdown;

    // animation speed
    private float _walkFramesPerSecond = 12.5f;
    private float _currentFrame = 0f;

    // components
    private SpriteRenderer currentSprite;

    // key parameters
    private int _keys = 0;
    
    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        _attackCountdown = attackCooldown;

        // reset the room if we aren't using the connective wrapper
        if (SceneManager.sceneCount == 1){
            PlayerPrefs.DeleteAll();
        }
        
        if (!PlayerPrefs.HasKey("keys")){
            PlayerPrefs.SetInt("keys", 0);
        }
        if (PlayerPrefs.HasKey("health")){
            _health = PlayerPrefs.GetInt("health");
        }

        // set up sprite
        currentSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
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

        handleAnimation();
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

    override public void takeDamage(){
        base.takeDamage();
        if (_health > 0){
            PlayerPrefs.SetInt("health", _health);
        }
    }

    override public void handleDeath(){
        // todo: death anim / sound, wait on anim/sound

        // get rid of health and keys on death
        PlayerPrefs.SetInt("health", 3);
        PlayerPrefs.SetInt("keys", 0);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void handleAnimation(){
        // update according to current movement
        Vector2 movement = getMovement();

        // should be idle, set idle sprite based on direction
        if (movement.x == 0 && movement.y == 0){
            switch (_currDir){
                case Direction.North:
                    currentSprite.sprite = idleSpriteUp;
                    break;
                case Direction.East:
                    currentSprite.sprite = idleSpriteRight;
                    break;
                case Direction.South:
                    currentSprite.sprite = idleSpriteDown;
                    break;
                case Direction.West:
                    currentSprite.sprite = idleSpriteLeft;
                    break;
                default:
                    break;
            }
        }
        // otherwise we're moving, set the update accordingly!
        else{
            _currentFrame = Mathf.Repeat(_currentFrame + Time.deltaTime * _walkFramesPerSecond, 6f);

            switch (_currDir){
                case Direction.North:
                    currentSprite.sprite = walkSpritesUp[Mathf.FloorToInt(_currentFrame)];
                    break;
                case Direction.East:
                    currentSprite.sprite = walkSpritesRight[Mathf.FloorToInt(_currentFrame)];
                    break;
                case Direction.South:
                    currentSprite.sprite = walkSpritesDown[Mathf.FloorToInt(_currentFrame)];
                    break;
                case Direction.West:
                    currentSprite.sprite = walkSpritesLeft[Mathf.FloorToInt(_currentFrame)];
                    break;
                default:
                    break;
            }

        }

        return;
    }

    public int getKeys(){
        return PlayerPrefs.GetInt("keys");
    }

    public void unlockDoor(){
        PlayerPrefs.SetInt("keys", PlayerPrefs.GetInt("keys") - 1);
    }

    public void collectKey(){
        PlayerPrefs.SetInt("keys", PlayerPrefs.GetInt("keys") + 1);
    }
}
