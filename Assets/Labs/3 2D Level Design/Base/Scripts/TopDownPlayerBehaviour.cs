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

    [SerializeField] private List<Sprite> attackSpritesUp = new List<Sprite>(4);
    [SerializeField] private List<Sprite> attackSpritesRight = new List<Sprite>(4);
    [SerializeField] private List<Sprite> attackSpritesDown = new List<Sprite>(4);
    [SerializeField] private List<Sprite> attackSpritesLeft = new List<Sprite>(4);

    [SerializeField] private List<Sprite> fallSprites = new List<Sprite>(6);

    [SerializeField] private Sprite hurtSpriteUp;
    [SerializeField] private Sprite hurtSpriteRight;
    [SerializeField] private Sprite hurtSpriteDown;
    [SerializeField] private Sprite hurtSpriteLeft;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource playerSoundSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    
    [SerializeField] private AudioSource controlSoundSource;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip keyPickupSound;
    [SerializeField] private AudioClip pitFallSound;

    [SerializeField] private AudioSource playerWalkSoundSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip slipSound;

    // internal variables    
    // attack parameters
    private bool _isAttacking = false;
    private float _attackThreshold = 1.8f;
    private float _attackCountdown;

    // animation speed
    private float _walkFramesPerSecond = 12.5f;
    private float _attackFramesPerSecond = 15f;
    private float _fallFramesPerSecond = 15f;
    private float _currentFrame = 0f;

    // falling parameters
    private bool _isFalling = false;
    private Vector2 lastSafePosition = Vector2.zero;

    // ice parameters
    private Vector2 _closestIce = Vector2.positiveInfinity;
    private bool _isOnIce = false;
    private float _iceThreshold = 0.9f;

    // hurt parameters
    private float _hurtFrames = 24f;
    private float _currentHurtFrame = 0f;
    private bool _isHurt = false;
    private bool _isDead = false;

    // components
    private SpriteRenderer currentSprite;
    private SpriteRenderer shadow;

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
        shadow = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        // set up walk sound
        playerWalkSoundSource.clip = walkSound;

        lastSafePosition = transform.position;
    }

    // Update is called once per frame
    override public void FixedUpdate()
    {
        base.FixedUpdate();

        // handle attack and damage collisions
        if (_isAttacking || Input.GetKey(attack)){
            handleAttack();
        }

        // handle walk sound
        if (!_isFalling && !_isOnIce && lastSafePosition != (Vector2)transform.position){
            playerWalkSoundSource.volume = 0.25f;
            playerWalkSoundSource.clip = walkSound;
            if (!playerWalkSoundSource.isPlaying){
                playerWalkSoundSource.Play();
            }
        }
        else if (_isOnIce){
            playerWalkSoundSource.volume = 0.25f;
            playerWalkSoundSource.clip = slipSound;
            if (!playerWalkSoundSource.isPlaying){
                playerWalkSoundSource.Play();
            }
        }
        else{
            playerWalkSoundSource.volume = Mathf.Clamp(playerWalkSoundSource.volume - 0.05f, 0f, 1f);
            if (playerWalkSoundSource.volume == 0f){
                playerWalkSoundSource.Stop();
            }
        }

        // if we aren't falling, save our last safe position!
        if (!_isFalling){
            lastSafePosition = transform.position;
        }

        // reset button
        if (Input.GetKey(reset)){
            handleDeath();
        }

        handleAnimation();

        

        if (_isDead){
            // only die once we are done playing the death sound!
            if (playerSoundSource.isPlaying) { return; }

            // get rid of health and keys on death, and reload the scene
            PlayerPrefs.SetInt("health", 3);
            PlayerPrefs.SetInt("keys", 0);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //Destroy(gameObject);
        }
    }

    override public Vector2 getMovement(){
        Vector2 update = Vector2.zero;
        // if we are attacking or falling or hurt or dead (i wish i used a state machine), we're frozen
        if (_isAttacking || _isFalling || _isHurt || _isDead) { return update; }

        // if we are on ice, we're moving in our current direction
        if (_isOnIce) { return dirToVec(); }

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
        if (_isAttacking || _isFalling || _isOnIce || _isDead) { return; }

        // shoot out a ray looking to ATTACK
        // _canAttack = false;
        _isAttacking = true;
        Vector2 attackDir = dirToVec();
        Vector2 extents = _collider.bounds.extents;

        Vector2 posUp = new Vector2(rb.position.x - extents.x, rb.position.y);
        Vector2 posDown = new Vector2(rb.position.x + extents.x, rb.position.y);

        RaycastHit2D attackRay = Physics2D.Raycast(rb.position, attackDir);
        RaycastHit2D attackRayUp = Physics2D.Raycast(posUp, attackDir);
        RaycastHit2D attackRayDown = Physics2D.Raycast(posDown, attackDir);

        if (attackCollision(attackRay)){
            attackRay.collider.gameObject.SendMessage("takeDamage");
        }
        if (attackCollision(attackRayUp)){
            attackRayUp.collider.gameObject.SendMessage("takeDamage");
        }
        if (attackCollision(attackRayDown)){
            attackRayDown.collider.gameObject.SendMessage("takeDamage");
        }

        // play the attack sound!
        playerSoundSource.clip = attackSound;
        playerSoundSource.Play();

        _currentFrame = 0;
    }

    override public void takeDamage(){
        int currHealth = _health;
        base.takeDamage();
        if (_health > 0){
            PlayerPrefs.SetInt("health", _health);
        }

        // if we took damage, do the hurt animation and play the sound
        if (currHealth != _health){
            _isHurt = true;
            _currentHurtFrame = 0f;

            // if health is 0, we're going to play the death sound instead!
            if (_health > 0){
                playerSoundSource.clip = hurtSound;
                playerSoundSource.Play();
            }
        }
    }

    override public void handleDeath(){
        _isDead = true;

        playerSoundSource.clip = deathSound;
        playerSoundSource.Play();
    }

    public void handleAnimation(){
        // check if we are attacking!
        if (_isAttacking){
            int lastFrame = Mathf.FloorToInt(_currentFrame);
            _currentFrame = Mathf.Repeat(_currentFrame + Time.deltaTime * _attackFramesPerSecond, 4f);

            // if we are done attacking, allow movement again and play the appropriate animation!
            if (lastFrame == 3 && Mathf.FloorToInt(_currentFrame) == 0){
                _isAttacking = false;
            }
            // otherwise, time to attack
            else{
                switch (_currDir){
                    case Direction.North:
                        currentSprite.sprite = attackSpritesUp[Mathf.FloorToInt(_currentFrame)];
                        break;
                    case Direction.East:
                        currentSprite.sprite = attackSpritesRight[Mathf.FloorToInt(_currentFrame)];
                        break;
                    case Direction.South:
                        currentSprite.sprite = attackSpritesDown[Mathf.FloorToInt(_currentFrame)];
                        break;
                    case Direction.West:
                        currentSprite.sprite = attackSpritesLeft[Mathf.FloorToInt(_currentFrame)];
                        break;
                    default:
                        break;
                }
                return;
            }
            
        }

        // if we are falling, do the falling animation lol
        if (_isFalling){
            int lastFrame = Mathf.FloorToInt(_currentFrame);
            _currentFrame = Mathf.Repeat(_currentFrame + Time.deltaTime * _fallFramesPerSecond, 6f);

            // we are done falling! set our position accordingly and take some damage
            if (lastFrame == 5 && Mathf.FloorToInt(_currentFrame) == 0){
                _isFalling = false;
                setInvincible(false);
                transform.position = lastSafePosition;
                takeDamage();
            }
            // keep playing the animation otherwise
            else{
                currentSprite.sprite = fallSprites[Mathf.FloorToInt(_currentFrame)];
                return;
            }
        }

        if (_isHurt){
            // check if we should be hurt anymore
            _currentHurtFrame += 1;
            if (_currentHurtFrame > _hurtFrames){
                _isHurt = false;
                _currentFrame = 0f;
            }
            // otherwise, play the hurt animation
            else{
                switch (_currDir){
                    case Direction.North:
                        currentSprite.sprite = hurtSpriteUp;
                        break;
                    case Direction.East:
                        currentSprite.sprite = hurtSpriteRight;
                        break;
                    case Direction.South:
                        currentSprite.sprite = hurtSpriteDown;
                        break;
                    case Direction.West:
                        currentSprite.sprite = hurtSpriteLeft;
                        break;
                    default:
                        break;
                }
                return;
            }
        }

        // otherwise, update according to current movement
        Vector2 movement = getMovement();

        // should be idle, set idle sprite based on direction
        if ((movement.x == 0 && movement.y == 0) || _isOnIce){
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
            _currentFrame = 0;
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

    public void unlockDoor(bool usedKey){
        if (usedKey){
            PlayerPrefs.SetInt("keys", PlayerPrefs.GetInt("keys") - 1);
        }
        
        // door sound
        controlSoundSource.clip = doorOpenSound;
        controlSoundSource.Play();
    }

    public void collectKey(){
        PlayerPrefs.SetInt("keys", PlayerPrefs.GetInt("keys") + 1);
        // key sound
        controlSoundSource.clip = keyPickupSound;
        controlSoundSource.Play();
    }

    public void setVisible(bool vis){
        currentSprite.enabled = vis;
        shadow.enabled = vis;
    }

    public bool fallInPit(Vector2 pitPosition){
        // if we are already falling, don't fall!
        if (_isFalling) { return false; }

        // time to fall! get rid of our sprite as we play the falling animation, and make us invulnerable!
        _isFalling = true;
        setInvincible(true);

        transform.position = pitPosition;
        _currentFrame = 0;

        // play falling sound
        controlSoundSource.clip = pitFallSound;
        controlSoundSource.Play();

        // we successfully fell!
        return true;
    }

    public void updateIce(Vector2 icePos){
        if (Mathf.Abs(Vector2.Distance(transform.position, icePos)) < Mathf.Abs(Vector2.Distance(transform.position, _closestIce))){
            _closestIce = icePos;
        }

        if (Mathf.Abs(_closestIce.x - transform.position.x) <= _iceThreshold && Mathf.Abs(_closestIce.y - transform.position.y) <= _iceThreshold && !isStopped()){
            _isOnIce = true;
            
        }
        else{
            _isOnIce = false;
        }
    }

    public bool isStopped(){
        return (lastSafePosition.x == transform.position.x && lastSafePosition.y == transform.position.y);
    }

    public bool attackCollision(RaycastHit2D attackRay){
        return attackRay.collider && attackRay.distance < _attackThreshold && attackRay.transform.gameObject.tag == "Enemy";
    }
}
