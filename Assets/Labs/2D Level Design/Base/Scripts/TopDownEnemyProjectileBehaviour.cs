using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownEnemyProjectileBehaviour : MonoBehaviour
{
    public enum Direction {North, East, South, West};
    public float speed = 5f;
    public Rigidbody2D rb;

    // the player
    private Rigidbody2D player;

    // out of bounds
    private float _bound = 50f;

    // Start is called before the first frame update
    void Start()
    {
        player = (Rigidbody2D)GameObject.Find("Player").GetComponent("Rigidbody2D");
    }

    void FixedUpdate(){
        if (Mathf.Abs(rb.position.x) > _bound || Mathf.Abs(rb.position.y) > _bound){
            Destroy(gameObject);
        }
    }

    public void setDirection(int direction){
        Vector2 velo = Vector2.zero;
        Direction dir = (Direction)direction; // goofy

        switch (dir){
            case Direction.North:
                velo.y = 1;
                break;
            case Direction.South:
                velo.y = -1;
                break;
            case Direction.East:
                velo.x = 1;
                break;
            case Direction.West:
                velo.x = -1;
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
            player.gameObject.SendMessage("takeDamage");
        }
        Destroy(gameObject);
    }
}
