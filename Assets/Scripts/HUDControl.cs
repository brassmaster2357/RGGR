using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDControl : MonoBehaviour
{
    public Image healthBar;
    public Image healthBarEmpty;
    public TextMeshProUGUI moneyText;
    public PlayerController pc;
    private float delayedCash;
    private Vector2 defaultHealthBar = new(40, -25);
    private Vector2 healthBarPosition = new(40, -25);
    private Vector2 defaultMoneyText = new(130, 64);
    private Vector2 moneyTextPosition = new(130, 64);

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        healthBarEmpty.rectTransform.sizeDelta = new(pc.healthMax * 50, 100);
    }

    void FixedUpdate()
    {
        healthBar.rectTransform.sizeDelta = new(pc.health * 50, 100);

        if (delayedCash != pc.cash)
        {
            // Overengineered code to make the money text shake depending on how much money the player is gaining/losing
            delayedCash = ((delayedCash * 5) + pc.cash) / 6;
            moneyTextPosition.x += Random.Range(-Mathf.Log(10 * Mathf.Abs(pc.cash - delayedCash) + 1), Mathf.Log(10 * Mathf.Abs(pc.cash - delayedCash) + 1));
            moneyTextPosition.y += Random.Range(-Mathf.Log(10 * Mathf.Abs(pc.cash - delayedCash) + 1), Mathf.Log(10 * Mathf.Abs(pc.cash - delayedCash) + 1));
        }

        if (pc.health <= 2 && pc.health != 0)
        {
            // Make the health bar shake if the player's health is low
            healthBarPosition.x += Random.Range(-6 / pc.health, 6 / pc.health);
            healthBarPosition.y += Random.Range(-6 / pc.health, 6 / pc.health);
        }

        healthBar.rectTransform.anchoredPosition = healthBarPosition;
        moneyText.rectTransform.anchoredPosition = moneyTextPosition;
        moneyText.text = Mathf.Round(delayedCash).ToString();

        moneyTextPosition = defaultMoneyText;
        healthBarPosition = defaultHealthBar;
    }
}
