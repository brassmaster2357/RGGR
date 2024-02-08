using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 direction;
    bool targetPlayer;
    public float speed;
    public float waitTime;
    float angle;
    private void Awake()
    {
        transform.position = startPos;
        angle = Mathf.Atan2((direction.x - startPos.x), (direction.y - startPos.y));
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (targetPlayer)
            direction = GameObject.Find("Player").transform.position;
        direction = (direction - startPos).normalized * speed;
        StartCoroutine(ShootDelay());
        GetComponent<Rigidbody2D>().velocity = direction;
        Destroy(this.gameObject, 5);
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(waitTime);
    }
}
