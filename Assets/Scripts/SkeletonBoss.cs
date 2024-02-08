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
    int currentWaypoint;
    float distance;
    float distTrack;
    Vector2 direction;

    float timer;
    public float fireRate;
    public float projSpeed;
    public GameObject projectile;

    public int health;
    public float speed;

    bool bulletHell;
    int pattern;
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
            timer = 0;
            Shoot();
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
        }
    }

    void Shoot()
    {
        // make an arrow, rotate it, and send it
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2((target.transform.position.y - transform.position.y), (target.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * projSpeed;
        Destroy(arrow, 1);
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
        Instantiate(money, transform.position, Quaternion.identity);
    }
}