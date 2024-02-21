using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoss : MonoBehaviour
{
    public GameObject money;
    public GameObject heartPickup;

    GameObject target;
    PlayerController player;
    Rigidbody2D myRB;

    public AudioSource enemyHitSound;
    public GameObject deathSoundEmitter;

    public float baseSpeed;
    float speed;
    public float health;
    public int damage;

    bool isCharging;
    float timer;
    public float stunTime;
    float dmgReduct;

    float timer2;
    public float flashPeriod;
    SpriteRenderer mySR;
    Color baseGreen;
    Color lightGreen;

    private void Start()
    {
        //initialize vars
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        mySR = GetComponent<SpriteRenderer>();
        baseGreen = new Color(1,1,1);
        lightGreen = new Color(0.75f,0.75f,0.75f);
        timer = 0;
        timer2 = 0;
        speed = baseSpeed;
        dmgReduct = 0.5f;
        isCharging = true;
    }
    private void FixedUpdate()
    {
        if (isCharging)
        {
            //add force to go towards the player
            myRB.AddForce((target.transform.position - transform.position).normalized * speed);

            //increase the speed
            speed += Time.deltaTime * 3;
        }
        else
        {
            // timer for being stunned
            if (timer < stunTime)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                timer2 = 0;
                speed = baseSpeed;
                dmgReduct = 0.5f;
                mySR.color = baseGreen;
                isCharging = true;
            }

            // timer for hue change
            if (timer2 < flashPeriod)
                timer2 += Time.deltaTime;
            else
            {
                if (mySR.color == baseGreen)
                {
                    mySR.color = lightGreen;
                }
                else
                {
                    mySR.color = baseGreen;
                }
                timer2 = 0;
            }
        }
        //if dead, die
        if (health <= 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collide with bullet, take damage and destory bullet
        if (collision.gameObject.tag == "Projectile" )
        {
            health -= 1 * dmgReduct;
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force * player.knockback);
            Destroy(collision.gameObject);
            enemyHitSound.Play();
            if (Random.Range(0f,1f) >= 0.985f)
			{
				GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
				heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f));
			}
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collide with player, deal damage
        if (collision.gameObject.tag == "Player")
        {
            player.Damage(damage,myRB.velocity,4);
        }
        // if collide with wall, get stunned
        if (collision.gameObject.tag == "Walls" && isCharging && myRB.velocity.magnitude >= 3)
        {
            isCharging = false;
            dmgReduct = 1;
        }
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        GameObject oof = Instantiate(deathSoundEmitter, transform.position, Quaternion.identity);
        Destroy(oof, 3);
        for (int i = 0; i < Random.Range(10, 31); i++)
        {
            GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-7.5f, 7.5f));
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
            heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-7.5f, 7.5f));
        }
    }
}