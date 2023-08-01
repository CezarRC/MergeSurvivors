using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuffs : MonoBehaviour
{
    public float healthBuffMultiplier { get; private set; } = 1f;
    public float fireRateBuffMultiplier { get; private set; } = 1f;
    public float damageBuffMultiplier { get; private set; } = 1f;
    public float rangeBuffMultiplier { get; private set; } = 1f;
    public float speedBuffMultiplier { get; private set; } = 1f;
    public float critChanceBuffMultiplier { get; private set; } = 1f;
    public float specialBuffMultiplier { get; private set; } = 1f;
    public float luckBuffMultiplier { get; private set; } = 1f;
    public float expBuffMultiplier { get; private set; } = 1f;
    public void BuffBaseHealth(float percentage)
    {
        healthBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalHealth(float percentage)
    {
        healthBuffMultiplier *= 1f + percentage / 100f;
    }

    public void BuffBaseFireRate(float percentage)
    {
        fireRateBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalMana(float percentage)
    {
        fireRateBuffMultiplier *= 1f + percentage / 100f;
    }

    public void BuffBaseSpecial(float percentage)
    {
        specialBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalSpecial(float percentage)
    {
        specialBuffMultiplier *= 1f + percentage / 100f;
    }

    public void BuffBaseDamage(float percentage)
    {
        damageBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalDamage(float percentage)
    {
        damageBuffMultiplier *= 1f + percentage / 100f;
    }

    public void BuffBaseCritChance(float percentage)
    {
        critChanceBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalCritChance(float percentage)
    {
        critChanceBuffMultiplier *= 1f + percentage / 100f;
    }
    public void BuffBaseRange(float percentage)
    {
        rangeBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalRange(float percentage)
    {
        rangeBuffMultiplier *= 1f + percentage / 100f;
    }
    public void BuffBaseSpeed(float percentage)
    {
        speedBuffMultiplier += percentage / 100f;
    }
    public void BuffTotalSpeed(float percentage)
    {
        speedBuffMultiplier *= 1f + percentage / 100f;
    }
    public void BuffTotalLuck(float percentage)
    {
        speedBuffMultiplier *= 1f + percentage / 100f;
    }
    public void BuffTotalExp(float percentage)
    {
        speedBuffMultiplier *= 1f + percentage / 100f;
    }

    public void ApplyBuffs(Upgrade upgrade)
    {
        BuffBaseHealth(upgrade.healthBuff);
        BuffBaseFireRate(upgrade.fireRateBuff);
        BuffBaseCritChance(upgrade.critChanceBuff);
        BuffBaseDamage(upgrade.damageBuff);
        BuffBaseRange(upgrade.rangeBuff);
        BuffBaseSpecial(upgrade.specialBuff);
        BuffBaseSpeed(upgrade.speedBuff);
        BuffTotalExp(upgrade.expBuff);
        BuffTotalLuck(upgrade.luckBuff);
    }
}
