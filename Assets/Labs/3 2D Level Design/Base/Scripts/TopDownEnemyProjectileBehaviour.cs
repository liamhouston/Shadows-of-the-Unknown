using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEnemyProjectileBehaviour : MonoBehaviour
{
    public enum Direction {North, East, South, West};
    public float speed = 5f;
    public Rigidbody2D rb;

    // sound
    public AudioSource soundSource;
    public AudioClip impactClip;

    // sprites and animation
    [SerializeField] private List<Sprite> idleSprites = new List<Sprite>(2);
    [SerializeField] private List<Sprite> impactSprites = new List<Sprite>(4);

    // animation parameters
    private float _currentFrame = 0f;
    private float _framesPerSecond = 15f;
    private bool _isImpact = false;

    // components
    private SpriteRenderer currentSprite;

    // the player
    private Rigidbody2D player;

    // out of bounds
    private float _bound = 50f;

    // Start is called before the first frame update
    void Start()
    {
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
        currentSprite = (SpriteRenderer)this.gameObject.transform.GetChild(0).gameObject.GetComponent("SpriteRenderer");
    }

    void FixedUpdate(){
        if (Mathf.Abs(rb.position.x) > _bound || Mathf.Abs(rb.position.y) > _bound){
            Destroy(gameObject);
        }

        handleAnimation();
    }

    public void setDirection(int direction){
        Vector2 velo = Vector2.zero;
        Direction dir = (Direction)direction; // goofy

        // swap position and rotate sprite accordingly
        switch (dir){
            case Direction.North:
                velo.y = 1;
                transform.Rotate(Vector3.forward * -270);
                break;
            case Direction.South:
                velo.y = -1;
                transform.Rotate(Vector3.forward * 270);
                break;
            case Direction.East:
                velo.x = 1;
                break;
            case Direction.West:
                velo.x = -1;
                transform.Rotate(Vector3.forward * 180);
                break;
            default:
                break;
        }
        velo *= speed;
        rb.velocity = velo;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Enemy") { return; }
        if (col.tag == "Player"){
            if (player != null)
            {
                player.gameObject.SendMessage("takeDamage");
            }

        }

        // impact has happened, adjust animations and play sound
        _isImpact = true;
        rb.velocity = Vector2.zero;
        _currentFrame = 0f;

        soundSource.clip = impactClip;
        soundSource.Play();
    }

    void handleAnimation(){
        if (_isImpact){
            int lastFrame = Mathf.FloorToInt(_currentFrame);
            _currentFrame = Mathf.Repeat(_currentFrame + Time.deltaTime * _framesPerSecond, 4f);
            if (lastFrame == 3 && Mathf.FloorToInt(_currentFrame) == 0){
                Destroy(gameObject);
            }
            else{
                currentSprite.sprite = impactSprites[Mathf.FloorToInt(_currentFrame)];
            }
        }
        else{
            _currentFrame = Mathf.Repeat(_currentFrame + Time.deltaTime * _framesPerSecond, 2f);
            currentSprite.sprite = idleSprites[Mathf.FloorToInt(_currentFrame)];
        }
        return;
    }
}
