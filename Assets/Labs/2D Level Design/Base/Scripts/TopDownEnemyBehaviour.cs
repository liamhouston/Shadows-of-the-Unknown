using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TopDownEnemyBehaviour : TopDownEntityBehaviour
{

    // public variables
    public float moveTime = 1.5f;
    public float damageRadius = 1f;

    // internal variables
    private float _remainingTime;
    protected bool _isDying = false;
    private bool _wallDeath = false;

    // sprites
    [Header("Sprite Settings")]
    [SerializeField] private List<Sprite> walkSpritesUp = new List<Sprite>(4);
    [SerializeField] private List<Sprite> walkSpritesRight = new List<Sprite>(4);
    [SerializeField] private List<Sprite> walkSpritesDown = new List<Sprite>(4);
    [SerializeField] private List<Sprite> walkSpritesLeft = new List<Sprite>(4);

    // sounds
    [Header("Sound Settings")]
    [SerializeField] private AudioSource enemySoundSource;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    // animation speed
    private float _walkFramesPerSecond = 8f;
    protected float _currentFrame = 0f;

    // components
    protected SpriteRenderer currentSprite;

    // the player & doors
    protected Rigidbody2D player;
    private TopDownDoorBehaviour[] doors = {};
    
    // time to check if we live inside a wall
    private Tilemap tiles;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        _remainingTime = moveTime;

        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        doors = (TopDownDoorBehaviour[])GameObject.FindObjectsOfType(typeof(TopDownDoorBehaviour));
        tiles = (Tilemap)GameObject.Find("Tilemap").GetComponent("Tilemap");
        
        currentSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        _health = 1;

        _currDir = pickDirection();

        // check if there's a tile at our current location. If so, it's time to die
        Vector3Int intPosition = new Vector3Int((int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.y), 0);
        if (tiles.HasTile(intPosition) && (tiles.GetTile(intPosition).name == "CaveRuleTile" || tiles.GetTile(intPosition).name == "PitRuleTile")){
            _wallDeath = true;
        }
    }
    
    // Update is called once per frame
    override public void FixedUpdate()
    {

        if (player == null){
            player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        }

        // if we are in a wall, die time
        if (_wallDeath){
            handleDeath();
            _wallDeath = false;
        }

        base.FixedUpdate();

        

        // handle movement, change directions after elapsed time
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0){
            _currDir = pickDirection();
            _remainingTime = moveTime;
        }

        // handle player collision
        if (Mathf.Abs(Vector2.Distance(player.position, transform.position)) <= damageRadius && !_isDying){
            player.gameObject.SendMessage("takeDamage");
        }

        handleAnimation();

        // wait to die until death sound is done!
        if (_isDying && !enemySoundSource.isPlaying){
            Destroy(gameObject);
        }
    }

    // uses helper function to convert current direction into movement vector
    override public Vector2 getMovement(){
        if (_isDying) { return Vector2.zero; }
        return dirToVec();
    }

    // update our direction with a random new one
    virtual public Direction pickDirection(){
        int i = 0; // loop limiter for the uh. 0.00390625 chance 
        
        // pick a new direction that wasn't our old one
        Direction newDir = (Direction)Random.Range(0, 4);
        while (newDir == _currDir && i < 100){
            newDir = (Direction)Random.Range(0, 4);
            i++;
        }
        return newDir;
    }
    
    override public void takeDamage(){
        int pastHealth = _health;
        base.takeDamage();
        if (pastHealth != _health && _health > 0){
            enemySoundSource.clip = hurtSound;
            enemySoundSource.Play();
        }
    }

    // on death, kill!!
    override public void handleDeath(){
        // sending a message to All Doors. I am dead
        for (int i = 0; i < doors.Length; i++){
            if (doors[i] != null){
                doors[i].SendMessage("enemyDeath");
            }
        }
        _isDying = true;
        enemySoundSource.clip = deathSound;
        enemySoundSource.Play();
    }

    virtual public void handleAnimation(){
        _currentFrame = Mathf.Repeat(_currentFrame + Time.deltaTime * _walkFramesPerSecond, 4f);

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
}
