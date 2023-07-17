using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public GameObject attackPrefab;
    public SoundEffect attackSound;
    public float cooldown;
    public float lastAttackTime;

    public bool CanAttack()
    {
        return Time.time > lastAttackTime + cooldown;
    }
}
