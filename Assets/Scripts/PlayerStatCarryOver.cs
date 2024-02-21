using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatCarryOver", menuName = "PlayerStatCarryOver")]
public class PlayerStatCarryOver : ScriptableObject
{
    public float maxSpeed = 8f;
    public int maxHealth = 6;
    public float cooldownBase = 0.25f;
    public float bulletVelocity = 1000;
    public int healthMax = 6;
    public float invulMax = 1;
    public float damage = 1;
    public int health = 6;
    public float invul = 0;
    public float knockback = 10f;
    public float cooldown = 0.25f;
    public float cash = 0;
    public float moneyMult = 1;

    public void Reset()
    {
        maxSpeed = 8f;
        maxHealth = 6;
        cooldownBase = 0.25f;
        bulletVelocity = 1000;
        healthMax = 6;
        invulMax = 1;
        damage = 1;
        health = 6;
        invul = 0;
        knockback = 10f;
        cooldown = 0.25f;
        cash = 0;
        moneyMult = 1;
    }

    public void Get(PlayerController player)
    {
        maxSpeed = player.maxSpeed;
        maxHealth = player.maxHealth;
        cooldownBase = player.cooldownBase;
        bulletVelocity = player.bulletVelocity;
        healthMax = player.maxHealth;
        invulMax = player.invulMax;
        damage = player.damage;
        health = player.health;
        invul = player.invul;
        knockback = player.knockback;
        cooldown = player.cooldown;
        cash = player.cash;
        moneyMult = player.moneyMult;
    }

    public void Apply(PlayerController player)
    {
        player.maxSpeed = maxSpeed;
        player.maxHealth = maxHealth;
        player.cooldownBase = cooldownBase;
        player.bulletVelocity = bulletVelocity;
        player.maxHealth = healthMax;
        player.invulMax = invulMax;
        player.damage = damage;
        player.health = health;
        player.invul = invul;
        player.knockback = knockback;
        player.cooldown = cooldown;
        player.cash = cash;
        player.moneyMult = moneyMult;
    }
}