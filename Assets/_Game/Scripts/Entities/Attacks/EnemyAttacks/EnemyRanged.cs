using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Attack", menuName = "Attacks/EnemyRanged")]
public class EnemyRanged : EnemyAttack
{
    public float spread;
    public override void ExecuteAttack(Transform spawnPoint, Vector2 target, Enemy attackerStats)
    {
        lastAttackTime = Time.time;
        ShootProjectile(spawnPoint, target, attackerStats);
    }

    void ShootProjectile(Transform spawnPoint, Vector2 target, Enemy attackerStats, float spread = 0)
    {
        GameObject go = ObjectPooler.Instance.GetPooledObject(attackPrefab);
        go.transform.position = spawnPoint.position;
        go.transform.localScale = attackPrefab.transform.localScale;
        Vector3 direction = new Vector3(target.x, target.y, 0) - spawnPoint.position;
        float rotationZ = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + Random.Range(-spread, spread));
        Projectile p = go.GetComponent<Projectile>();
        p.SetProjectileInfo(attackerStats.damage, 0, attackerStats.gameObject);
        go.SetActive(true);
    }
}
