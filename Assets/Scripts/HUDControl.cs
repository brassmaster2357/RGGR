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
    public Canvas poweringUpScreen;
    private GameManager gm;
    public StatMod[] modifiers;

    private float delayedCash;
    private Vector2 defaultHealthBar = new(40, -25);
    private Vector2 healthBarPosition = new(40, -25);
    private Vector2 defaultMoneyText = new(130, 64);
    private Vector2 moneyTextPosition = new(130, 64);

    private StatMod leftMod;
    private StatMod rightMod;
    private Image leftImage;
    private TextMeshProUGUI leftName;
    private TextMeshProUGUI leftDesc;
    private Image rightImage;
    private TextMeshProUGUI rightName;
    private TextMeshProUGUI rightDesc;

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        gm = GameObject.Find("GameManagr").GetComponent<GameManager>();
        healthBarEmpty.rectTransform.sizeDelta = new(pc.healthMax * 50, 100);
    }

    void FixedUpdate()
    {
        if (pc.gettingModifiers)
        {
            StartPoweringUp();
        }
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

    void StartPoweringUp()
    {
        poweringUpScreen.enabled = true;
        gm.paused = true;
        leftMod = modifiers[Random.Range(0, modifiers.Length - 1)];
        rightMod = modifiers[Random.Range(0, modifiers.Length - 1)];
        while (rightMod == leftMod)
        {
            rightMod = modifiers[Random.Range(0, modifiers.Length - 1)];
        }
        leftImage.sprite = leftMod.art;
        rightImage.sprite = rightMod.art;
        leftName.text = leftMod.name;
        rightName.text = rightMod.name;
        leftDesc.text = "";
        for (int i = 0; i < leftMod.description.Length; i++)
        {
            leftDesc.text += leftMod.description[i] + "\n";
        }
        for (int i = 0; i < rightMod.description.Length; i++)
        {
            rightDesc.text += rightMod.description[i] + "\n";
        }
    }

    // I love buttons
    public void LeftChosen()
    {
        Choose(leftMod);
    }
    public void RightChosen()
    {
        Choose(rightMod);
    }

    public void Choose(StatMod modifier)
    {
        for (int i = 0; i < modifier.stats.Length; i++)
        {
            switch (modifier.stats[i])
            {
                case StatMod.EStat.Health:
                    pc.healthMax += (int)modifier.mods[i];
                    break;
                case StatMod.EStat.Speed:
                    pc.maxSpeed *= modifier.mods[i];
                    break;
                case StatMod.EStat.InvTime:
                    pc.invulMax += modifier.mods[i];
                    break;
                case StatMod.EStat.BulletDamage:
                    pc.damage *= modifier.mods[i];
                    break;
                case StatMod.EStat.BulletSpeed:
                    pc.bulletVelocity *= modifier.mods[i];
                    break;
                case StatMod.EStat.FireRate:
                    pc.cooldownBase *= modifier.mods[i];
                    break;
                case StatMod.EStat.MoneyGained:
                    pc.moneyMult *= modifier.mods[i];
                    break;
                default:
                    Debug.Log("LOSER L LOSER NO MODIFICATIONS LOSER L + RATIO");
                    break;
            }
        }
        poweringUpScreen.enabled = false;
        pc.gettingModifiers = false;
        gm.paused = false;
    }
}
