using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MummyAI : MonoBehaviour
{
    public GameObject money;

    GameObject target;
    PlayerController player;
    Rigidbody2D myRB;

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
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force * player.knockback * 1/2);
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collide with player, deal damage
        if (collision.gameObject.tag == "Player")
        {
            player.Damage(damage);
        }
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
        coin.GetComponent<Rigidbody2D>().velocity = new(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
    }
}