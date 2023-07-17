using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : Attack
{
    public abstract void ExecuteAttack(Transform spawnPoint, Vector2 target, Enemy attackerStats);
}
