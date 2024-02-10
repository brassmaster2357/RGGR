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
        if (this.gameObject.name == "Beeg Arrow" && FIRE)
        {
            print("Firing");
            transform.position = startPos;
            angle = Mathf.Atan2((direction.x - startPos.x), (direction.y - startPos.y)) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if (targetPlayer)
                direction = GameObject.Find("Player").transform.position;
            direction = (direction - startPos).normalized * speed;
            StartCoroutine(ShootDelay());
            GetComponent<Rigidbody2D>().velocity = direction;
            Destroy(this.gameObject, 5);
            FIRE = false;
        }
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(waitTime);
    }
}
