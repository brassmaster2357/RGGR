using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyBoss : MonoBehaviour
{
    public GameObject money;
    public GameObject heartPickup;
    public GameObject zombie;
    public GameObject skele;
    public GameObject mummy;

    public AudioSource enemyHitSound;
    public GameObject deathSoundEmitter;

    GameObject target;
    PlayerController player;
    Rigidbody2D myRB;

    public float speed;
    public float health;
    float maxHealth;
    public int damage;

    float timer;
    public float chargeUpTime;
    float chaseTime;
    bool charging;
    public float chargeSpeed;

    float timer2;
    public float targetingTime;
    public float summoningTime;
    bool isSummoning;
    bool arrived;
    bool up;
    float xValue;
    float distance;
    int summons;
    int random;
    Vector2 startSpot;
    Vector2 waitSpot;
    Vector2 topSpot;
    Vector2 bottomSpot;
    GameObject enemy;

    private void Start()
    {
        //initialize vars
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        chaseTime = 2;
        charging = false;
        timer = 0;
        isSummoning = false;
        maxHealth = health;
        waitSpot = new Vector2(0, -2);
        topSpot = new Vector2(0, -1);
        bottomSpot = new Vector2(0, -3);
    }
    private void FixedUpdate()
    {
        if (isSummoning)
        {
            if (!arrived)
            {
                // go to the dance floor
                myRB.AddForce((waitSpot - (Vector2)transform.position).normalized * speed);
                if (((Vector2)transform.position - startSpot).magnitude >= distance)
                {
                    transform.position = waitSpot;
                    arrived = true;
                    topSpot.x = xValue;
                    bottomSpot.x = xValue;
                }
            }
            else
            {
                // cha-cha real smooth
                if (up)
                {
                    myRB.AddForce((topSpot - (Vector2)transform.position).normalized * speed * 2);
                    if (((Vector2)transform.position - startSpot).magnitude >= (topSpot - startSpot).magnitude)
                        up = false;
                }
                else
                {
                    myRB.AddForce((bottomSpot - (Vector2)transform.position).normalized * speed * 2);
                    if (((Vector2)transform.position - startSpot).magnitude <= (bottomSpot - startSpot).magnitude)
                        up = true;
                }
                if (timer < summoningTime)
                    timer += Time.deltaTime;
                else
                {
                    timer = 0;
                    random = Random.Range(1, 4);
                    switch (random)
                    {
                        case 1:
                            random = Random.Range(1, 3);
                            if (random == 1)
                                enemy = Instantiate(zombie, new Vector2(topSpot.x, 1), Quaternion.identity);
                            else
                                enemy = Instantiate(zombie, new Vector2(bottomSpot.x, -5), Quaternion.identity);
                            enemy.GetComponent<ZombieAI>().health = 2;
                            break;

                        case 2:
                            random = Random.Range(1, 3);
                            if (random == 1)
                                enemy = Instantiate(skele, new Vector2(topSpot.x, 1), Quaternion.identity);
                            else
                                enemy = Instantiate(skele, new Vector2(bottomSpot.x, -5), Quaternion.identity);
                            enemy.GetComponent<SkeletonAI>().health = 2;
                            break;

                        default:
                            random = Random.Range(1, 3);
                            if (random == 1)
                                enemy = Instantiate(mummy, new Vector2(topSpot.x, 1), Quaternion.identity);
                            else
                                enemy = Instantiate(mummy, new Vector2(bottomSpot.x, -5), Quaternion.identity);
                            enemy.GetComponent<MummyAI>().health = 2;
                            break;
                    }
                    summons--;
                    if (summons <= 0)
                    {
                        isSummoning = false;
                        charging = false;
                    }
                }
            }
        }
        else
        {
            print("sfdsfd");
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
            if (timer2 < targetingTime)
                timer2 += Time.deltaTime;
            else
            {
                timer2 = 0;
                timer = 0;
                isSummoning = true;
                arrived = false;
                if (target.transform.position.x >= 0)
                    xValue = 12;
                else
                    xValue = -12;
                startSpot = transform.position;
                waitSpot.x = xValue;
                distance = (waitSpot - startSpot).magnitude;
                if (maxHealth >= health && health >= maxHealth * 0.6f)
                    summons = 1;
                else if (maxHealth * 0.6f >= health && health >= maxHealth * 0.3f)
                    summons = 2;
                else if (maxHealth * 0.3f >= health && health >= 0)
                    summons = 3;
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
            myRB.AddForce(force * player.knockback * 1 / 2);
            Destroy(collision.gameObject);
			if (Random.Range(0f,1f) >= 0.985f)
			{
				GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
				heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
			}
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
        GameObject oof = Instantiate(deathSoundEmitter, transform.position, Quaternion.identity);
        Destroy(oof, 3);
        for (int i = 0; i < Random.Range(20, 61); i++)
        {
            GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-7.5f, 7.5f));
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject deathTarget in enemies)
            Destroy(deathTarget);
    }
}
