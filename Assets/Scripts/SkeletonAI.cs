using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public GameObject money;
    public GameObject heartPickup;

    GameObject target;
    PlayerController PC;
    Rigidbody2D myRB;
    SpriteControl spriteControl;

    public AudioSource enemyHitSound;
    public AudioSource shootSound;
    public GameObject deathSoundEmitter;

    // Set waypoints in inspector to set patrol, leave empty for sentry mode
    public List<Vector2> waypoints;
    int currentWaypoint;
    float distance;
    float distTrack;
    Vector2 direction;
    
    float timer;
    public float fireRate;
    public float projSpeed;
    public GameObject projectile;

    public float health;
    public float speed;

    private void Awake()
    {
        // initialize vars
        target = GameObject.Find("Player");
        PC = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        spriteControl = GetComponent<SpriteControl>();
        timer = 0;

        //only initialize these if the skeleton has places to go
        if (waypoints.Count >= 2)
        {
            transform.position = waypoints[0];
            currentWaypoint = 1;
            distance = Vector2.Distance(transform.position, waypoints[currentWaypoint]);
            direction = (Vector3)(waypoints[currentWaypoint] - (Vector2)(transform.position)).normalized;
            distTrack = 0;
        }
    }
    private void FixedUpdate()
    {
        spriteControl.input = target.transform.position - transform.position;
        //dont run this code in sentry mode
        if (waypoints.Count >= 2)
        {
            // move and keep track of the distance moved to the next waypoint
            transform.position += (Vector3)(direction * Time.deltaTime * speed);
            distTrack = Vector2.Distance(transform.position, waypoints[NewWaypoint(false)]);

            //when you reach a waypoint, change to the next one and reset some variables
            if (distTrack >= distance)
            {
                currentWaypoint = NewWaypoint(true);
                distance = Vector2.Distance(transform.position, waypoints[currentWaypoint]);
                direction = (Vector3)(waypoints[currentWaypoint] - (Vector2)(transform.position)).normalized;
                distTrack = 0;
            }
        }

        // if dead, die
        if (health <= 0)
            Destroy(this.gameObject);

        // shooting timer
        if (timer < fireRate)
            timer += Time.deltaTime;
        else
        {
            timer = Random.Range(-0.2f,0.2f);
            Shoot();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collide with bullet, take damage, get knocked back and destory bullet
        if (collision.gameObject.tag == "Projectile")
        {
            health -= PC.damage;
            enemyHitSound.Play();
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force.normalized * force.magnitude * PC.knockback);
            Destroy(collision.gameObject);
        }
        //if collide with whip, take damage, get knocked back
        if (collision.gameObject.tag == "Whip")
        {
            health -= PC.damage * 1.5f;
            enemyHitSound.Play();
            switch (collision.gameObject.name) // knockback depending on whip direction
            {
                case "left":
                    myRB.AddForce(Vector2.left * PC.knockback * 125);
                    break;
                case "right":
                    myRB.AddForce(Vector2.right * PC.knockback * 125);
                    break;
                case "up":
                    myRB.AddForce(Vector2.up * PC.knockback * 125);
                    break;
                case "down":
                    myRB.AddForce(Vector2.down * PC.knockback * 125);
                    break;
                default: // This should never happen
                    Vector2 force = target.GetComponent<Rigidbody2D>().velocity;
                    myRB.AddForce(force * PC.knockback * 125);
                    break;
            }
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision); // don't get hit twice in a row with the same attack, because that's possible apparently
        }
    }

    void Shoot()
    {
        // make an arrow, rotate it, and send it
        shootSound.Play();
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2((target.transform.position.y - transform.position.y), (target.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        arrow.transform.rotation = Quaternion.Euler(0,0,angle);
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * projSpeed;
        Destroy(arrow, 3);
    }
    int NewWaypoint(bool next)
    {
        // like a double looped linked list
        if (next)
        {
            if (currentWaypoint == waypoints.Count - 1)
                return 0;
            else
                return currentWaypoint + 1;
        }
        else
        {
            if (currentWaypoint == 0)
                return waypoints.Count - 1;
            else
                return currentWaypoint - 1;
        }
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        GameObject oof = Instantiate(deathSoundEmitter, transform.position, Quaternion.identity);
        Destroy(oof, 1);
        GameObject coin = Instantiate(money, transform.position, Quaternion.identity);
        coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        if (Random.Range(0f, 1f) >= 0.9f)
        {
            GameObject heart = Instantiate(heartPickup, transform.position, Quaternion.identity);
            heart.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2.5f, -2.5f), Random.Range(-2.5f, -2.5f));
        }
    }
}
