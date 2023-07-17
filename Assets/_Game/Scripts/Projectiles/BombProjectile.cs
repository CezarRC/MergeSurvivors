using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    float startTime;
    [SerializeField]
    float explosionTime;
    [SerializeField]
    float scaleFactor = 1f;
    [SerializeField]
    GameObject explosionProjectiles;
    [SerializeField]
    int projectileAmount;
    [SerializeField]
    GameObject explosionParticle;
    [SerializeField]
    string explosionSoundName;

    protected override void Start()
    {
        Init();
    }
    void Init()
    {
        Vector2 force = transform.up * projectileSpeed;
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        startTime = Time.time;
    }
    private void Update()
    {
        if (Time.time > startTime + explosionTime)
        {
            Deactivate();
        }
    }

    protected virtual void Explode()
    {
        GameObject particle = Instantiate(explosionParticle);
        particle.transform.position = gameObject.transform.position;
        particle.transform.rotation = gameObject.transform.rotation;
        for (int i = 0; i < projectileAmount; i++)
        {
            ShootProjectiles((360 / projectileAmount) * i);
        }
    }

    protected virtual void ShootProjectiles(float projectileRotation)
    {
        GameObject go = ObjectPooler.Instance.GetPooledObject(explosionProjectiles);
        go.transform.position = gameObject.transform.position;
        go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, projectileRotation);
        go.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        go.GetComponent<Projectile>().SetProjectileInfo(buffedDamage, buffedCritChance, owner);
        go.SetActive(true);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void Deactivate()
    {
        Explode();
        base.Deactivate();
    }
    protected override void OnEnable()
    {
        Init();
    }
}