using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAttack : Attack
{
    public abstract void ExecuteAttack(Transform spawnPoint, Vector2 targetPosition, CharacterStats attackerStats);
}
