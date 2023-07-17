using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float fireRate;
    [SerializeField] float special;
    [SerializeField] float damage;
    [SerializeField] float critChance;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] float luck;

    public float BaseHealth { get => health ; }
    public float BaseFireRate { get => fireRate; }
    public float BaseSpecial { get => special; }
    public float BaseDamage { get => damage; }
    public float BaseCritChance { get => critChance; }
    public float BaseRange { get => range; }
    public float BaseSpeed { get => speed; }

    public CharacterBuffs buffs { get; private set; }

    void Start()
    {
        buffs = GetComponent<CharacterBuffs>();
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
