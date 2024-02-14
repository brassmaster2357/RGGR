using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 direction;
    public bool targetPlayer;
    public float speed;
    public float waitTime;
    float angle;
    public bool FIRE;
    private void FixedUpdate()
    {
        if (!name.Contains("Position") && FIRE)
        {
            FIRE = false;
            transform.position = startPos;
            float angle = Mathf.Atan2((direction.y - transform.position.y), (direction.x - transform.position.x)) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            direction = (direction - startPos).normalized * speed;
            if (targetPlayer)
                direction = GameObject.Find("Player").transform.position;
            StartCoroutine(ShootDelay());
        }
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Rigidbody2D>().velocity = direction;
        Destroy(this.gameObject, 3);
        FIRE = false;
    }
}
