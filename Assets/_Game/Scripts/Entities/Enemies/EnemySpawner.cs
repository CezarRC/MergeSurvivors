using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float enemiesPerSecond = 1;
    [SerializeField] bool shouldSpawn = true;

    Transform groupTransform;
    [SerializeField] List<GameObject> tempEnemiesToSpawn;
    [SerializeField] GameObject bossPrefab;
    //SE DER TEMPO - fazer scriptable object de Wave e refatorar o spawn

    int currentWave = 1;
    float startTime = 0;
    private void Start()
    {
        groupTransform = Group.CurrentGroup.transform;
        StartCoroutine(SpawnEnemyRepeating());
        startTime = Time.time;
    }
    IEnumerator SpawnEnemyRepeating()
    {
        while (shouldSpawn)
        {
            if (Time.time > startTime + 30)
            {
                currentWave++;
                startTime = Time.time;
                if (currentWave % 2 == 0) SpawnEnemyInRadius(groupTransform.position, bossPrefab, 20f);
            }
            SpawnEnemyInRadius(groupTransform.position, tempEnemiesToSpawn[Random.Range(0, tempEnemiesToSpawn.Count)], 20f);
            yield return new WaitForSeconds(1f / enemiesPerSecond * (1.5f * currentWave));
        }
    }
    public static void SpawnEnemyInRadius(Vector2 position, GameObject enemy, float radius)
    {
        float angle = Random.value * Mathf.PI * 2;
        Vector2 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        Vector3 spawnPosition = position + dir * radius;
        spawnPosition.z = spawnPosition.y;
        GameObject go = ObjectPooler.Instance.GetPooledObject(enemy);
        go.transform.position = spawnPosition;
        go.transform.rotation = Quaternion.Euler(0, 0, 0);
        go.SetActive(true);
    }
}
