using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "New Upgrade")]
public class Upgrade : ScriptableObject
{
    public static readonly Dictionary<Rarity, Color> RarityToColor = new Dictionary<Rarity, Color>{
        { Rarity.Common, Color.grey },
        { Rarity.Uncommon, Color.green },
        { Rarity.Rare, Color.blue },
        { Rarity.Epic, Color.magenta },
        { Rarity.Legendary, new Color(1, 0.55f, 0.2f) }, //Orange
        { Rarity.Godly, Color.red },
        { Rarity.Unbelievable, Color.cyan }
    };
    public Sprite upgradeSprite;
    public string upgradeName;
    public string upgradeDescription;
    public float healthBuff;
    public float fireRateBuff;
    public float damageBuff;
    public float rangeBuff;
    public float speedBuff;
    public float critChanceBuff;
    public float specialBuff;
    public float luckBuff;
    public float expBuff;
    public Rarity rarity;

    public static string GetDescription(Upgrade upg)
    {
        string health = upg.healthBuff > 0f ? "HEALTH: +" + upg.healthBuff + "%\n" : "";
        string fireRate = upg.fireRateBuff > 0f ? "FIRE RATE: +" + upg.fireRateBuff + "%\n" : "";
        string damage = upg.damageBuff  > 0f ? "DAMAGE: +" + upg.damageBuff + "%\n" : "";
        string range = upg.rangeBuff   > 0f ? "RANGE: +" + upg.rangeBuff + "%\n" : "";
        string speed = upg.speedBuff > 0f ? "SPEED: +" + upg.speedBuff + "%\n" : "";
        string crit = upg.critChanceBuff > 0f ? "CRIT CHANCE: +" + upg.critChanceBuff  + "%\n" : "";
        string special = upg.specialBuff > 0f ? "SPECIAL: +" + upg.specialBuff + "%\n" : "";
        string luck = upg.luckBuff > 0f ? "LUCK: +" + upg.luckBuff + "%\n" : "";
        string exp = upg.expBuff > 0f ? "EXP: +" + upg.expBuff + "%\n" : "";
        return health + fireRate + damage + range + speed + crit + special + luck + exp;
    }
}
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Godly,
    Unbelievable
}