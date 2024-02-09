using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public PlayerStatCarryOver playerStatCarryOver;

    private UnityEngine.InputSystem.Gamepad controller;
    private Rigidbody2D rb;
    public Vector2 velocity;
    public GameObject bullet;
    public Image pickupIndicator;
    private CameraController cam;
    private GameManager gm;
    private SpriteRenderer playerSprite;
    private Color invincibilityFlash = new(1,1,1,1);
    public bool gettingModifiers;
    public HUDControl hudControl;

    public float maxSpeed = 8f;
    public int maxHealth = 6;
    public float cooldownBase = 0.25f;
    public float bulletVelocity = 1000;
    public int healthMax = 6;
    public float invulMax = 1;

    public float damage = 1;
    public int health = 6;
    public float invul = 0;
    private float rollDelay = 2.5f;
    public float knockback = 10f;
    public float cooldown = 0.25f;
    public float cash = 0;
    public float moneyMult = 1;

    public float deadzone = 0.1f;

    public Vector2 leftStick;
    public Vector2 rightStick;
    public bool aButton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        gm = GameObject.Find("GameManagr").GetComponent<GameManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        controller = UnityEngine.InputSystem.Gamepad.current;
        pickupIndicator.enabled = false;
        playerStatCarryOver.Apply(this);
    }

    void Update()
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
        if (controller.startButton.wasPressedThisFrame && !gettingModifiers)
            gm.PressedPauseButton();
        if (controller.leftStickButton.wasPressedThisFrame)
            Roll();
    }

    void FixedUpdate()
    {
        // Miscellaneous stuff
        if (controller == null)
            controller = UnityEngine.InputSystem.Gamepad.current;
        bool playerNotActivelyMoving = true;

        // Left stick (movement)
        if (leftStick.magnitude > deadzone)
        {
            velocity = leftStick;
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(velocity * 100);
                playerNotActivelyMoving = false;
            }
        }

        // Right stick (shooting)
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

        // Time related stuff
        if (health <= 0)
        {
            Debug.Log("YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD");
            gm.ReturnToMenu();
        }
        if (playerNotActivelyMoving)
        {
            rb.velocity *= 0.9f; // only apply friction when the player isn't adding velocity, otherwise their max speed is actually 90% of their real max speed
        }
        cooldown -= Time.deltaTime;
        invul -= Time.deltaTime;
        rollDelay -= Time.deltaTime;
        if (invul > 0)
            invincibilityFlash.a = Mathf.Cos(invul * 5 * Mathf.PI) * 0.2f + 0.4f;
        else
            invincibilityFlash.a = 1;
        playerSprite.color = invincibilityFlash;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Money"))
        {
            float completelyUselessVariableName = collision.gameObject.transform.localScale.x * 100;
            cash += Random.Range(completelyUselessVariableName / 2, completelyUselessVariableName * 2);
            Debug.Log("get moneyed");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Heart"))
        {
            if (health < maxHealth)
            {
                health++;
                Debug.Log("get healthed");
                Destroy(collision.gameObject);
            }
        }
        else
        {
            pickupIndicator.enabled = true;
            if (aButton)
            {
                switch (collision.gameObject.tag)
                {
                    case "Pickup":
                        cash += Random.Range(50f, 250f);
                        Destroy(collision.gameObject);
                        break;
                    case "Relic":
                        hudControl.StartPoweringUp();
                        gettingModifiers = true;
                        Destroy(collision.gameObject);
                        break;
                    case "Staircase":
                        playerStatCarryOver.Get(this);
                        gm.LoadNextLevel();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void Roll()
    {
        if (rollDelay <= 0)
        {
            rb.AddForce(225 * maxSpeed * leftStick.normalized);
            invul = 0.55f;
            rollDelay = 2.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup") || collision.gameObject.CompareTag("Relic") || collision.gameObject.CompareTag("Staircase"))
        {
            pickupIndicator.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Damage(1);
        }
    }

    public void Damage(int damage)
    {
        if (invul <= 0)
        {
            cam.InitiateCameraShake(damage);
            health -= damage;
            invul = 1;
        }
    }
}