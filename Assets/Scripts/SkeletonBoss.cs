using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour
{
    public GameObject money;

    GameObject target;
    PlayerController PC;
    Rigidbody2D myRB;

    // Set waypoints in inspector to set patrol, leave empty for sentry mode
    public List<Vector2> waypoints;
    Vector2 currentWaypoint;
    float distance;
    float distTrack;
    Vector2 direction;

    float timer;
    public float fireRate;
    public float projSpeed;
    public GameObject projectile;

    public float health;
    public float speed;

    bool bulletHell;
    bool bulletsShooting;
    float waitTime;
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
        timer = 0;

        if (target.transform.position.x >= 0)
        {
            int random = Random.Range(0, 2);
            currentWaypoint = waypoints[random];
            direction = (currentWaypoint - (Vector2)transform.position).normalized;
            if (random == 0)
                transform.position = waypoints[1];
            else
                transform.position = waypoints[0];
        }
        else
        {
            int random = Random.Range(2, 4);
            currentWaypoint = waypoints[random];
            direction = (currentWaypoint - (Vector2)transform.position).normalized;
            if (random == 2)
                transform.position = waypoints[3];
            else
                transform.position = waypoints[2];
        }
        distance = Vector2.Distance(transform.position, currentWaypoint);
        distTrack = 0;
        bulletHell = false;
        bulletsShooting = false;
    }
    private void FixedUpdate()
    {
        if (bulletHell)
        {
            print("aggressive");
            bulletHell = false;
            BulletHell();
        }
        else if (!bulletsShooting)
        {
            // move and keep track of the distance moved to the next waypoint
            transform.position += (Vector3)(direction * Time.deltaTime * speed);
            distTrack = distance - Vector2.Distance(transform.position, currentWaypoint);
            print(currentWaypoint);
            //when you reach the waypoint, run a bullet hell
            if (distTrack <= 0)
            {
                print("made it");
                bulletHell = true;
                bulletsShooting = true;
                timer = 0;
            }
            // shooting timer
            if (timer < fireRate)
                timer += Time.deltaTime;
            else
            {
                print("passive shot");
                timer = 0;
                Shoot();
            }
        }
        // if dead, die
        if (health <= 0)
        {
            Destroy(this.gameObject);
            print("dead");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collide with bullet, take damage, get knocked back and destory bullet
        if (collision.gameObject.tag == "Projectile")
        {
            print("I got shot");
            health -= 1;
            Vector2 force = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            myRB.AddForce(force.normalized * force.magnitude * PC.knockback);
            Destroy(collision.gameObject);
        }
    }

    void Shoot()
    {
        // make an arrow, rotate it, and send it
        print("I shot at you");
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2((target.transform.position.y - transform.position.y), (target.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * projSpeed;
        Destroy(arrow, 1);
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        Instantiate(money, transform.position, Quaternion.identity);
    }
    void BulletHell()
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                waitTime = 6;
                pattern = pattern1;
                break;
            case 2:
                pattern = pattern2;
                break;
            case 3:
                pattern = pattern3;
                break;
            default:
                pattern = pattern4;
                break;
        }
        waitTime = 5;
        pattern = pattern1;
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
            print("arrow set up");
        }
        if (timer < waitTime)
            timer += Time.deltaTime;
        else
        {
            if (target.transform.position.x >= 0)
            {
                int random = Random.Range(0, 2);
                currentWaypoint = waypoints[random];
                direction = (currentWaypoint - (Vector2)transform.position).normalized;
                if (random == 0)
                    transform.position = waypoints[1];
                else
                    transform.position = waypoints[0];
            }
            else
            {
                int random = Random.Range(2, 4);
                currentWaypoint = waypoints[random];
                direction = (currentWaypoint - (Vector2)transform.position).normalized;
                if (random == 2)
                    transform.position = waypoints[3];
                else
                    transform.position = waypoints[2];
            }
            timer = 0;
            bulletsShooting = false;
        }
    }
}