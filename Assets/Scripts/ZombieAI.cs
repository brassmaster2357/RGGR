using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    GameObject target;
    Rigidbody2D myRB;
    public float speed;
    int health;

    private void Start()
    {
        target = GameObject.Find("Player");
        myRB = GetComponent<Rigidbody2D>();
        health = 3;
    }
    private void Update()
    {
        myRB.AddForce((target.transform.position - transform.position).normalized * speed);
        if (health <= 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            health -= 1;
            Destroy(collision.gameObject);
        }
    }
}