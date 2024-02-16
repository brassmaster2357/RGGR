using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteControl : MonoBehaviour
{
    public bool objectRotates = false;

    public bool customInput = false; // Toggle customInput on to give other vectors, like shooting direction
    public Vector2 input;

    private Sprite activeSprite;
    public Sprite spriteDefault;
    public Sprite spriteNorth;
    public Sprite spriteSouth;
    public Sprite spriteEast;
    public Sprite spriteWest;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!customInput)
            input = rb.velocity;

        if (input.magnitude <= 0.01f)
            activeSprite = spriteDefault;
        else if (input.x > input.y)
        {
            if (Mathf.Sign(input.x) >= 0)
                activeSprite = spriteEast;
            else
                activeSprite = spriteWest;
        }
        else
        {
            if (Mathf.Sign(input.y) >= 0)
                activeSprite = spriteNorth;
            else
                activeSprite = spriteSouth;
        }

        spriteRenderer.sprite = activeSprite;
        transform.position = new(transform.position.x, transform.position.y, transform.position.y / 1000f);
    }
}
