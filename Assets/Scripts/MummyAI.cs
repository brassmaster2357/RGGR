using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MummyAI : MonoBehaviour
{
    public GameObject money;
    public GameObject heartPickup;

    GameObject target;
    PlayerController player;
    Rigidbody2D myRB;

    public AudioSource enemyHitSound;
    public AudioSource chargeSound;
    public GameObject deathSoundEmitter;

    public float speed;
    public float health;
    public int damage;

    float timer;
    public float chargeUpTime;
    float chaseTime;
    bool charging;
    public float chargeSpeed;

    private void Start()
    {
        //initialize vars
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        chaseTime = 2;
        charging = false;
    }
    private void FixedUpdate()
    {
        //The mummy has two states: charging and chasing. while chasing, it works just like a zombie. while charging, it pauses, then lunges at the player
        if (charging)
        {
            // wait timer
            if (timer < chargeUpTime)
                timer += Time.deltaTime;
            else
            {
                // Lunge
                timer = 0;
                chargeSound.Play();
                myRB.AddForce((target.transform.position - transform.position).normalized * chargeSpeed);
                charging = false;
            }
        }
        else
        {
            //add force to go towards the player while chasing
            myRB.AddForce((target.transform.position - transform.position).normalized * speed);

            //chase timer
            if (timer < chaseTime)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                charging = true;
            }
        }

        //if dead, die
        if (health <= 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collide with bullet, take damage, get knocked back and destory bullet
        if (collision.gameObject.tag == "Projectile")
        {
            health -= player.damage;
            enemyHitSound.Play();
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force * player.knockback * 1/2);
            Destroy(collision.gameObject);
        }
        //if collide with whip, take damage, get knocked back
        if (collision.gameObject.tag == "Whip")
        {
            health -= player.damage * 1.5f;
            enemyHitSound.Play();
            switch (collision.gameObject.name) // knockback depending on whip direction
            {
                case "left":
                    myRB.AddForce(Vector2.left * player.knockback * 90);
                    break;
                case "right":
                    myRB.AddForce(Vector2.right * player.knockback * 90);
                    break;
                case "up":
                    myRB.AddForce(Vector2.up * player.knockback * 90);
                    break;
                case "down":
                    myRB.AddForce(Vector2.down * player.knockback * 90);
                    break;
                default: // This should never happen
                    Vector2 force = target.GetComponent<Rigidbody2D>().velocity;
                    myRB.AddForce(force * player.knockback * 90);
                    break;
            }
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision); // don't get hit twice in a row with the same attack, because that's possible apparently
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collide with player, deal damage
        if (collision.gameObject.tag == "Player")
        {
            player.Damage(damage,myRB.velocity);
        }
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        GameObject oof = Instantiate(deathSoundEmitter, transform.position, Quaternion.identity);
        Destroy(oof, 1);
        GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
        coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        if (Random.Range(0f, 1f) >= 0.85f)
        {
            GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
            heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        }
    }
}