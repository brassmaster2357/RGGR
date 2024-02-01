using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PlayerController : MonoBehaviour
{

    public UnityEngine.InputSystem.Gamepad controller;
    public Rigidbody2D rb;
    public Vector2 velocity;
    public GameObject bullet;

    public float maxSpeed = 5f;
    public float cooldownBase = 0.25f;
    public float bulletVelocity = 1000;

    public float cooldown = 0.25f;
    public float deadzone = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = UnityEngine.InputSystem.Gamepad.current;
    }


    void Update()
    {
        bool playerNotActivelyMoving = true;

        // left stick (movement)
        if (controller.leftStick.ReadValue().magnitude > deadzone)
        {
            Vector2 stick = controller.leftStick.ReadValue();
            velocity = stick;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(velocity * 100);
                playerNotActivelyMoving = false;
            }
        }

        // right stick (shooting)
        if (controller.rightStick.ReadValue().magnitude > deadzone)
        {
            Vector2 stick = controller.rightStick.ReadValue();
            if (cooldown <= 0)
            {
                cooldown = cooldownBase;
                GameObject b = Instantiate(bullet, transform);
                b.transform.position = transform.position;
                Physics2D.IgnoreCollision(b.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
                b.GetComponent<Rigidbody2D>().AddForce(stick * (1 / stick.magnitude) * bulletVelocity);
                Destroy(b, 1);
            }
        }

        if (controller.aButton.wasPressedThisFrame)
        {
            Debug.Log("woah");
        }
        if (controller.aButton.wasReleasedThisFrame)
        {
            Debug.Log("wow");
        }

        // time related stuff
        if (playerNotActivelyMoving)
        {
            rb.velocity *= 0.9f; // only apply friction when the player isn't adding velocity, otherwise their max speed is actually 90% of their real max speed
        }
        cooldown -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup") && controller.aButton.isPressed)
        {
            Debug.Log(collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }
}
