using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    GameObject target;
    PlayerController PC;
    Rigidbody2D myRB;
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

    private void Start()
    {
        target = GameObject.Find("Player");
        PC = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        timer = 0;
        if (waypoints.Count >= 2)
        {
            transform.position = waypoints[0];
            currentWaypoint = 1;
            distance = Vector2.Distance(transform.position, waypoints[currentWaypoint]);
            direction = (Vector3)(waypoints[currentWaypoint] - (Vector2)(transform.position)).normalized;
            distTrack = 0;
        }
    }
    private void Update()
    {
        if (waypoints.Count >= 2)
        {
            transform.position += (Vector3)(direction * Time.deltaTime * speed);
            distTrack = Vector2.Distance(transform.position, waypoints[NewWaypoint(false, currentWaypoint)]);
            if (distTrack >= distance)
            {
                currentWaypoint = NewWaypoint(true, currentWaypoint);
                distance = Vector2.Distance(transform.position, waypoints[currentWaypoint]);
                direction = (Vector3)(waypoints[currentWaypoint] - (Vector2)(transform.position)).normalized;
                distTrack = 0;
            }
        }
        if (health <= 0)
            Destroy(this.gameObject);
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
        //if collide with bullet, take damage and destory bullet
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
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2((target.transform.position.y - transform.position.y), (target.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        arrow.transform.rotation = Quaternion.Euler(0,0,angle);
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * projSpeed;
        Destroy(arrow, 1);
    }
    int NewWaypoint(bool way, int point)
    {
        if (way)
        {
            if (point == waypoints.Count - 1)
                return 0;
            else
                return point + 1;
        }
        else
        {
            if (point == 0)
                return waypoints.Count - 1;
            else
                return point - 1;
        }
    }
}
