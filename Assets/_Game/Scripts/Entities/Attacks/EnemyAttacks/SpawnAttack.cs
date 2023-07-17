using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Attack", menuName = "Attacks/Spawn Attack")]
public class SpawnAttack : EnemyAttack
{
    [SerializeField] List<GameObject> possibleEnemies;
    public override void ExecuteAttack(Transform spawnPoint, Vector2 target, Enemy attackerStats)
    {
        if (Time.time < lastAttackTime + cooldown) return;
        lastAttackTime = Time.time;

        cooldown = attackerStats.GetFireRate();
        GameObject enemyToSpawn = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
        GameObject attackEffect = ObjectPooler.Instance.GetPooledObject(attackPrefab);
        attackEffect.transform.position = spawnPoint.position;
        EnemySpawner.SpawnEnemyInRadius(spawnPoint.position, enemyToSpawn, Random.Range(3f, attackerStats.GetRange()));
    }
}
