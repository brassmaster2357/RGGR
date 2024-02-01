using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    GameObject target;
    PlayerController player;
    Rigidbody2D myRB;
    public List<Vector2> waypoints;
    Vector2 currentWaypoint;
    float distance;
    Vector2 direction;
    public int health;
    public int damage;

    private void Start()
    {
        target = GameObject.Find("Player");
        player = target.GetComponent<PlayerController>();
        myRB = GetComponent<Rigidbody2D>();
        if (waypoints != null && waypoints.Count != 1)
        {
            transform.position = waypoints[0];
            currentWaypoint = waypoints[1];
            distance = Vector2.Distance(transform.position, currentWaypoint);
            direction = (Vector3)(currentWaypoint - (Vector2)(transform.position)).normalized;
        }
    }
    private void Update()
    {
        if (waypoints != null && waypoints.Count != 1)
        {
            transform.position += (Vector3)(direction * Time.deltaTime);
        }
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
