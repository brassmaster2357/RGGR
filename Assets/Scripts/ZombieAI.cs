using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public GameObject money;
    public GameObject heartPickup;

    GameObject target;
    PlayerController player;
    Rigidbody2D myRB;

    public AudioSource enemyHitSound;
    public GameObject deathSoundEmitter;

    public float speed;
    public float health;
    public int damage;

    private void Start()
    {
        //initialize vars
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //add force to go towards the player
        myRB.AddForce((target.transform.position - transform.position).normalized * speed);

        //if dead, die
        if (health <= 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        //if collide with bullet, take damage and destory bullet
        if (collision.gameObject.tag == "Projectile")
        {
            health -= player.damage;
            enemyHitSound.Play();
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force * player.knockback);
            Destroy(collision.gameObject);
        }
        //if collide with whip, take damage
        if (collision.gameObject.tag == "Whip")
        {
            Debug.Log("Whip");
            health -= player.damage * 1.5f;
<<<<<<< Updated upstream
            switch (collision.gameObject.name) // knockback depending on whip direction
            {
                case "left":
                    myRB.AddForce(Vector2.left * player.knockback * 125);
                    break;
                case "right":
                    myRB.AddForce(Vector2.right * player.knockback * 125);
                    break;
                case "up":
                    myRB.AddForce(Vector2.up * player.knockback * 125);
                    break;
                case "down":
                    myRB.AddForce(Vector2.down * player.knockback * 125);
                    break;
                default: // This should never happen
                    Vector2 force = target.GetComponent<Rigidbody2D>().velocity;
                    myRB.AddForce(force * player.knockback * 125);
                    break;
            }
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision); // don't get hit twice in a row with the same attack, because that's possible apparently
=======
>>>>>>> Stashed changes
            enemyHitSound.Play();
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collide with player, deal damage
        if (collision.gameObject.tag == "Player")
        {
            player.Damage(damage,myRB.velocity,1);
        }
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        GameObject oof = Instantiate(deathSoundEmitter, transform.position, Quaternion.identity);
        Destroy(oof, 1);
        GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
        coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        if (Random.Range(0f,1f) >= 0.9f)
        {
            GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
            heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        }
    }
}