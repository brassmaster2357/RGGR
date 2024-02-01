using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    GameObject target;
    Rigidbody2D myRB;
    public float speed;

    private void Start()
    {
        target = GameObject.Find("Target");
        myRB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        myRB.AddForce((target.transform.position - transform.position).normalized * speed);
    }
}