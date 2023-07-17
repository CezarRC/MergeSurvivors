using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitAttack : EnemyAttack
{
    public override void ExecuteAttack(Transform spawnPoint, Vector2 target, Enemy attackerStats)
    {
        lastAttackTime = Time.time;
        if (((Vector2)spawnPoint.position - target).sqrMagnitude < 2.5f*2.5f)
        {

        }
    }
}
