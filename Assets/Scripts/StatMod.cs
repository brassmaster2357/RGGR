using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "Modifier")]
public class StatMod : ScriptableObject
{
    // SCRIPTABLE OBJECTS WOOOOOOOOOOOOO
    public Sprite art;
    public string[] description;
    public enum EStat
    {
        Health,
        Speed,
        InvTime,

        BulletDamage,
        BulletSpeed,
        FireRate,

        MoneyGained
    }
    public EStat[] stats;
    public float[] mods;
}
