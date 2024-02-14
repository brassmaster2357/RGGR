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
        //if collide with bullet, take damage and destory bullet
        if (collision.gameObject.tag == "Projectile")
        {
            health -= player.damage;
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force * player.knockback);
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
        coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        if (Random.Range(0f,1f) >= 0.95f)
        {
            GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
            heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        }
    }
}