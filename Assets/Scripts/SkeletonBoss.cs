using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour
{
    public GameObject money;
    public GameObject heartPickup;

    GameObject target;
    PlayerController PC;
    Rigidbody2D myRB;
    SpriteControl spriteControl;

    public AudioSource enemyHitSound;
    public GameObject deathSoundEmitter;

    // Set waypoints in inspector to set patrol, leave empty for sentry mode
    public List<Vector2> waypoints;
    Vector2 currentWaypoint;
    float distance;
    float distTrack;
    Vector2 direction;
    Vector2 lastWaypoint;

    float timer;
    public float fireRate;
    public float projSpeed;
    public GameObject projectile;

    public float health;
    public float speed;

    float timer2;
    public float restTime;
    bool bulletHell;
    bool bulletsShooting;
    List<Arrow> pattern;
    public List<Arrow> pattern1;
    public List<Arrow> pattern2;
    public List<Arrow> pattern3;
    public List<Arrow> pattern4;

    private void Awake()
    {
        // initialize vars
        target = GameObject.Find("Player");
        PC = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        spriteControl = GetComponent<SpriteControl>();
        timer = 0;
        timer2 = 0;

        if (target.transform.position.x >= 0)
        {
            int random = Random.Range(0, 2);
            currentWaypoint = waypoints[random];
            if (random == 0)
            {
                transform.position = waypoints[1];
                lastWaypoint = waypoints[1];
            }
            else
            {
                transform.position = waypoints[0];
                lastWaypoint = waypoints[0];
            }
        }
        else
        {
            int random = Random.Range(2, 4);
            currentWaypoint = waypoints[random];
            if (random == 2)
            {
                transform.position = waypoints[3];
                lastWaypoint = waypoints[3];
            }
            else
            {
                transform.position = waypoints[2];
                lastWaypoint = waypoints[2];
            }
        }
        distance = Vector2.Distance(transform.position, currentWaypoint);
        distTrack = 0;
        bulletHell = false;
        bulletsShooting = false;
        direction = (currentWaypoint - (Vector2)transform.position).normalized;
    }
    private void FixedUpdate()
    {
        spriteControl.input = target.transform.position - transform.position;
        if (!bulletsShooting)
        {
            if (timer2 < restTime)
                timer2 += Time.deltaTime;
            else
                BulletHell();
        }
        // move and keep track of the distance moved to the next waypoint
        myRB.velocity = direction * speed;
        distTrack = Vector2.Distance(transform.position, lastWaypoint);
        //when you reach the waypoint, run a bullet hell
        if (distTrack >= distance)
        {
            if (target.transform.position.x >= 0)
            {
                int random = Random.Range(0, 2);
                currentWaypoint = waypoints[random];
                if (random == 0)
                {
                    transform.position = waypoints[1];
                    lastWaypoint = waypoints[1];
                }
                else
                {
                    transform.position = waypoints[0];
                    lastWaypoint = waypoints[0];
                }
            }
            else
            {
                int random = Random.Range(2, 4);
                currentWaypoint = waypoints[random];
                if (random == 2)
                {
                    transform.position = waypoints[3];
                    lastWaypoint = waypoints[3];
                }
                else
                {
                    transform.position = waypoints[2];
                    lastWaypoint = waypoints[2];
                }
            }
            distance = Vector2.Distance(transform.position, currentWaypoint);
            distTrack = 0;
            direction = (currentWaypoint - (Vector2)transform.position).normalized;
            timer = 0;
        }
        // shooting timer
        if (timer < fireRate)
            timer += Time.deltaTime;
        else
        {
            timer = 0;
            if (!bulletsShooting)
                Shoot();
        }
        
        // if dead, die
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collide with bullet, take damage, get knocked back and destory bullet
        if (collision.gameObject.tag == "Projectile")
        {
            health -= 1;
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force.normalized * force.magnitude * PC.knockback);
            Destroy(collision.gameObject);
			if (Random.Range(0f,1f) >= 0.985f)
			{
				GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
				heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
			}
        }
    }

    void Shoot()
    {
        // make an arrow, rotate it, and send it
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2((target.transform.position.y - transform.position.y), (target.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * projSpeed;
        Destroy(arrow, 3);
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        GameObject oof = Instantiate(deathSoundEmitter, transform.position, Quaternion.identity);
        Destroy(oof, 3);
        for (int i = 0; i < Random.Range(15, 46); i++)
        {
            GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-7.5f, 7.5f));
        }
    }
    void BulletHell()
    {
        bulletsShooting = true;
        switch (Random.Range(1, 5))
        {
            case 1:
                pattern = pattern1;
                break;
            case 2:
                pattern = pattern2;
                break;
            default:
                pattern = pattern3;
                break;
        }
        foreach (Arrow arrow in pattern)
        {
            GameObject gameObject = Instantiate(projectile, new Vector2(1000, 1000), Quaternion.identity);
            gameObject.AddComponent<Arrow>();
            Arrow myA = gameObject.GetComponent<Arrow>();
            myA.startPos = arrow.startPos;
            myA.direction = arrow.direction;
            myA.speed = arrow.speed;
            myA.waitTime = arrow.waitTime;
            myA.FIRE = true;
        }
        StartCoroutine(WaitForArrows());
        
    }
    IEnumerator WaitForArrows()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Beeg Arrow").Length == 0);
        bulletsShooting = false;
        timer2 = 0;
    }
}