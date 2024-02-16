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
    public StatMod[] rareModifiers;

    private float delayedCash;
    private Vector2 defaultHealthBar = new(40, -25);
    private Vector2 healthBarPosition = new(40, -25);
    private Vector2 defaultMoneyText = new(130, 64);
    private Vector2 moneyTextPosition = new(130, 64);

    public StatMod leftMod;
    public StatMod rightMod;
    public Image leftImage;
    public TextMeshProUGUI leftName;
    public TextMeshProUGUI leftDesc;
    public Image rightImage;
    public TextMeshProUGUI rightName;
    public TextMeshProUGUI rightDesc;

    void Start()
    {
        poweringUpScreen.enabled = false;
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        gm = GameObject.Find("GameManagr").GetComponent<GameManager>();
        healthBarEmpty.rectTransform.sizeDelta = new(pc.healthMax * 50, 100);
    }

    private void Update()
    {
        if (pc.xButton && pc.gettingModifiers)
            Choose(leftMod);
        else if (pc.bButton && pc.gettingModifiers)
            Choose(rightMod);
    }

    void FixedUpdate()
    {
        healthBar.rectTransform.sizeDelta = new(pc.health * 50, 100);
        healthBarEmpty.rectTransform.sizeDelta = new(pc.healthMax * 50, 100);

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

    public void StartPoweringUp()
    {
        Debug.Log("we're starting to power up baby");
        poweringUpScreen.enabled = true;
        gm.PauseNoMenu();
        leftMod = modifiers[Random.Range(0, modifiers.Length)];
        if (Random.Range(0f,1f) > 0.9f)
        {
            leftMod = rareModifiers[Random.Range(0, rareModifiers.Length)];
        }
        rightMod = modifiers[Random.Range(0, modifiers.Length)];
        if (Random.Range(0f, 1f) > 0.9f)
        {
            rightMod = rareModifiers[Random.Range(0, rareModifiers.Length)];
        }
        while (rightMod == leftMod)
        {
            rightMod = modifiers[Random.Range(0, modifiers.Length)];
        }
        leftImage.sprite = leftMod.art;
        rightImage.sprite = rightMod.art;
        leftName.text = leftMod.name;
        rightName.text = rightMod.name;
        leftDesc.text = "";
        rightDesc.text = "";
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
        pc.gettingModifiers = false;
        for (int i = 0; i < modifier.stats.Length; i++)
        {
            switch (modifier.stats[i])
            {
                case StatMod.EStat.Health:
                    int temp = (int)modifier.mods[i];
                    pc.healthMax += temp;
                    if (Mathf.Sign(temp) == -1)
                    {
                        if (pc.healthMax < 1)
                            pc.healthMax = 1;
                        if (pc.health >= pc.healthMax)
                            pc.health = pc.healthMax;
                    } else
                        pc.health += temp;
                    break;
                case StatMod.EStat.Speed:
                    pc.maxSpeed *= modifier.mods[i];
                    if (pc.maxSpeed < 0.25f) pc.maxSpeed = 0.25f;
                    break;
                case StatMod.EStat.InvTime:
                    pc.invulMax += modifier.mods[i];
                    if (pc.invulMax < 0.1f) pc.invulMax = 0.1f;
                    break;
                case StatMod.EStat.BulletDamage:
                    pc.damage *= modifier.mods[i];
                    if (pc.damage < 0.1f) pc.damage = 0.1f;
                    break;
                case StatMod.EStat.BulletSpeed:
                    pc.bulletVelocity *= modifier.mods[i];
                    if (pc.bulletVelocity < 0.25f) pc.bulletVelocity = 0.25f;
                    break;
                case StatMod.EStat.FireRate:
                    pc.cooldownBase *= modifier.mods[i];
                    if (pc.cooldownBase < 0.025f) pc.cooldownBase = 0.025f;
                    break;
                case StatMod.EStat.MoneyGained:
                    pc.moneyMult *= modifier.mods[i];
                    // If you somehow mess up bad enough to get a *negative* money multiplier, that's your own fault.
                    break;
                default:
                    Debug.Log("LOSER L LOSER NO MODIFICATIONS LOSER L + RATIO");
                    break;
            }
        }
        poweringUpScreen.enabled = false;
        gm.ResumeGame();
        Debug.Log("Done");
    }
}
