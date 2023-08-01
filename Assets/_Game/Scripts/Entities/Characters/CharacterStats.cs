using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public float BaseHealth { get => health ; }
    public float BaseFireRate { get => fireRate; }
    public float BaseSpecial { get => special; }
    public float BaseDamage { get => damage; }
    public float BaseCritChance { get => critChance; }
    public float BaseRange { get => range; }
    public float BaseSpeed { get => speed; }

    [SerializeField] private float health;
    [SerializeField] private float fireRate;
    [SerializeField] private float special;
    [SerializeField] private float damage;
    [SerializeField] private float critChance;
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private float luck;

    [SerializeField] private CharacterBuffs buffs;

    public CharacterBuffs GetBuffs()
    {
        return buffs;
    }
    public float GetBuffedHealth()
    {
        return health * buffs.healthBuffMultiplier;
    }
    public float GetBuffedFireRate()
    {
        return fireRate * buffs.fireRateBuffMultiplier;
    }
    public float GetBuffedSpecial()
    {
        return special * buffs.specialBuffMultiplier;
    }
    public float GetBuffedDamage()
    {
        return damage * buffs.damageBuffMultiplier;
    }
    public float GetBuffedCritChance()
    {
        return critChance * buffs.critChanceBuffMultiplier;
    }
    public float GetBuffedRange()
    {
        return range * buffs.rangeBuffMultiplier;
    }
    public float GetBuffedSpeed()
    {
        return speed * buffs.speedBuffMultiplier;
    }
    public float GetBuffedLuck()
    {
        return luck * buffs.luckBuffMultiplier;
    }
}
