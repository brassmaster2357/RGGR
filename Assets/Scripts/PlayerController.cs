using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public PlayerStatCarryOver playerStatCarryOver;

    private UnityEngine.InputSystem.Gamepad controller;
    private Rigidbody2D rb;
    public Vector2 velocity;
    public GameObject bullet;
    public GameObject whip;
    private bool canWhip = true;
    public Image pickupIndicator;
    private CameraController cam;
    private GameManager gm;
    private SpriteRenderer playerSprite;
    private SpriteControl spriteControl;
    private Color invincibilityFlash = new(1,1,1,1);
    public bool gettingModifiers;
    public HUDControl hudControl;

    public AudioSource playerHitSound;
    public AudioSource fireSound;
    public AudioSource whipSound;
    public AudioSource dashrollSound;
    public GameObject coinSoundPrefab;

    public float maxSpeed = 8f;
    public int maxHealth = 6;
    public float cooldownBase = 0.25f;
    public float bulletVelocity = 1000;
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
    public bool bButton;
    public bool xButton;
    public bool yButton;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        gm = GameObject.Find("GameManagr").GetComponent<GameManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        spriteControl = GetComponent<SpriteControl>();
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

        if (controller.bButton.isPressed)
            bButton = true;
        else
            bButton = false;

        if (controller.xButton.isPressed)
            xButton = true;
        else
            xButton = false;

        if (controller.rightShoulder.wasPressedThisFrame)
            StartCoroutine(cooHWhip());

        if (controller.startButton.wasPressedThisFrame && !gettingModifiers)
            gm.PressedPauseButton();

        if (controller.leftShoulder.wasPressedThisFrame)
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
            spriteControl.input = rightStick;
            if (cooldown <= 0)
            {
                cooldown = cooldownBase;
                GameObject b = Instantiate(bullet, transform);
                fireSound.Play();
                b.transform.position = transform.position;
                Physics2D.IgnoreCollision(b.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                b.GetComponent<Rigidbody2D>().AddForce((1 / rightStick.magnitude) * bulletVelocity * rightStick);
                b.GetComponent<Rigidbody2D>().SetRotation(Vector2.SignedAngle(Vector2.up, rightStick));
                Destroy(b, 3);
            }
        }
        else
            spriteControl.input = leftStick;

        // Time related stuff
        if (health <= 0)
        {
            Debug.Log("YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD YOU'RE DEAD");
            gm.GameOver();
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
<<<<<<< Updated upstream
            cash += Random.Range(50 * moneyMult, 200 * moneyMult);
=======
            float completelyUselessVariableName = collision.gameObject.transform.localScale.x * 100;
            cash += Random.Range(completelyUselessVariableName / 2, completelyUselessVariableName * 2);
>>>>>>> Stashed changes
            GameObject coinSoundObj = Instantiate(coinSoundPrefab, transform);
            AudioSource coinSound = coinSoundObj.GetComponent<AudioSource>();
            coinSound.pitch = Random.Range(0.85f, 1.15f);
            coinSound.Play();
            Destroy(coinSoundObj, 2);
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
                        pickupIndicator.enabled = false;
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
            invul = 0.55f * invulMax;
            rollDelay = 2.5f;
        }
    }

    //dont correct spelling, trust
    private IEnumerator cooHWhip(){
        if (!canWhip)
            yield break;
        canWhip = false;
        string dir = "";
        Transform whipObject;
        Debug.Log("https://www.youtube.com/watch?v=nfVEvgWd4ek");
        if (rightStick != Vector2.zero){
<<<<<<< Updated upstream
            if (Mathf.Abs(rightStick.x) > Mathf.Abs(rightStick.y))
=======
            if (rightStick == Vector2.zero){
                dir = "down";
            }
            else if (Mathf.Abs(rightStick.x) > Mathf.Abs(rightStick.y))
>>>>>>> Stashed changes
            {
                if (rightStick.x > 0)
                    dir = "right";
                else if (rightStick.x < 0)
                    dir = "left";
            }
<<<<<<< Updated upstream
            else if (Mathf.Abs(rightStick.y) >= Mathf.Abs(rightStick.x))
=======
            else if (Mathf.Abs(rightStick.y) > Mathf.Abs(rightStick.x))
>>>>>>> Stashed changes
            {
                if (rightStick.y > 0)
                    dir = "up";
                else if (rightStick.y < 0)
                    dir = "down";
            }
        } 
        else {
            if (leftStick == Vector2.zero){
                dir = "down";
            }
            else if (Mathf.Abs(leftStick.x) > Mathf.Abs(leftStick.y))
            {
                if (leftStick.x > 0)
                    dir = "right";
                else if (leftStick.x < 0)
                    dir = "left";
            }
<<<<<<< Updated upstream
            else if (Mathf.Abs(leftStick.y) >= Mathf.Abs(leftStick.x))
=======
            else if (Mathf.Abs(leftStick.y) > Mathf.Abs(leftStick.x))
>>>>>>> Stashed changes
            {
                if (leftStick.y > 0)
                    dir = "up";
                else if (leftStick.y < 0)
                    dir = "down";
            }
        }
        
        
        whipObject = whip.transform.Find(dir);
        Debug.Log("whip " + dir);
        whipObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        whipObject.gameObject.SetActive(false);
        canWhip = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pickupIndicator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Damage(1,collision.gameObject.GetComponent<Rigidbody2D>().velocity);
        }
        if (collision.gameObject.CompareTag("Beeg Arrow"))
        {
            Damage(1, collision.gameObject.GetComponent<Rigidbody2D>().velocity, 2);
        }
    }

    public void Damage(int damage, Vector2 angle, float knockback = 1)
    {
        if (invul <= 0)
        {
            playerHitSound.Play();
            cam.InitiateCameraShake(damage);
            health -= damage;
            invul = 1 * invulMax;
            rb.velocity = angle * knockback;
        }
    }
}