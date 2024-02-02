using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    public UnityEngine.InputSystem.Gamepad controller;
    public Rigidbody2D rb;
    public Vector2 velocity;
    public GameObject bullet;
    public Image healthBar;
    public Image healthBarEmpty;
    public Image pickupIndicator;
    public CinemachineVirtualCamera cam;

    public float maxSpeed = 5f;
    public int maxHealth = 6;
    public float cooldownBase = 0.25f;
    public float bulletVelocity = 1000;
    public int healthMax = 6;
    public float invulMax = 1;

    public int health = 6;
    public float invul = 0;
    public float knockback = 0.5f;
    public float cooldown = 0.25f;

    public Vector3 camPos;
    public float deadzone = 0.1f;
    public Vector2 healthBarPosition = new(40, -25);

    public Vector2 leftStick;
    public Vector2 rightStick;
    public bool aButton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = UnityEngine.InputSystem.Gamepad.current;
        healthBar = GameObject.Find("Health Bar").GetComponent<Image>();
        camPos = cam.transform.position;
        pickupIndicator.enabled = false;
        healthBarEmpty.rectTransform.sizeDelta = new(healthMax * 50, 100);
    }

    private void Update()
    {
        // Update is being used to capture inputs ASAP
        if (controller.leftStick.ReadValue().magnitude > deadzone)
            leftStick = controller.leftStick.ReadValue();
        else
            leftStick = new(0, 0);

        if (controller.rightStick.ReadValue().magnitude > deadzone)
            rightStick = controller.rightStick.ReadValue();
        else
            rightStick = new(0, 0);

        if (controller.aButton.isPressed)
            aButton = true;
        else
            aButton = false;
    }

    void FixedUpdate()
    {
        if (controller == null)
            controller = UnityEngine.InputSystem.Gamepad.current;
        bool playerNotActivelyMoving = true;

        // left stick (movement)
        if (leftStick.magnitude > deadzone)
        {
            velocity = leftStick;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(velocity * 100);
                playerNotActivelyMoving = false;
            }
        }

        // right stick (shooting)
        if (rightStick.magnitude > deadzone)
        {
            if (cooldown <= 0)
            {
                cooldown = cooldownBase;
                GameObject b = Instantiate(bullet, transform);
                b.transform.position = transform.position;
                Physics2D.IgnoreCollision(b.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
                b.GetComponent<Rigidbody2D>().AddForce((1 / rightStick.magnitude) * bulletVelocity * rightStick);
                Destroy(b, 1);
            }
        }

        // time related stuff
        if (health <= 0)
        {
            Debug.Log("YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD");
        }
        if (playerNotActivelyMoving)
        {
            rb.velocity *= 0.9f; // only apply friction when the player isn't adding velocity, otherwise their max speed is actually 90% of their real max speed
        }
        cooldown -= Time.deltaTime;
        invul -= Time.deltaTime;
        healthBar.rectTransform.sizeDelta = new(health * 50, 100);
        if (health <= 2)
        {
            healthBarPosition.x += Random.Range(-6 / health, 6 / health);
            healthBarPosition.y += Random.Range(-6 / health, 6 / health);
            healthBar.rectTransform.anchoredPosition = healthBarPosition;
            healthBarPosition = new(40, -25);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup"))
        {
            pickupIndicator.enabled = true;
            if (aButton)
            {
                Debug.Log(collision.gameObject.name);
                Destroy(collision.gameObject);
            }
        }
        else
            pickupIndicator.enabled = false;
        if (collision.gameObject.CompareTag("InstantPickup"))
        {
            if (collision.gameObject.name.Contains("Heart") && health < maxHealth)
            {
                health++;
                Debug.Log("get healthed");
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup"))
        {
            pickupIndicator.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Damage(1);
        }
    }

    public void Damage(int damage)
    {
        if (invul <= 0)
        {
            StartCoroutine(CameraShake(0.1f * damage, Mathf.Pow(0.9f,damage)));
            health -= damage;
            invul = 1;
        }
    }

    private IEnumerator CameraShake(float intensity, float falloff)
    {
        while (intensity > 0.001f)
        {
            cam.transform.position = new(camPos.x + Random.Range(-intensity, intensity), camPos.y + Random.Range(-intensity, intensity), -10);
            intensity *= falloff;
            yield return new WaitForFixedUpdate();
        }
    }
}
