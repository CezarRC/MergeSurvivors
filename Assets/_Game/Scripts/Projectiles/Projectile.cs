using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int pierceResistance = 1;
    public int piercingsLeft;
    public float projectileSpeed;
    public float timeUntilDestruction;
    protected float buffedDamage;
    protected float buffedCritChance;
    public string projectileSound;
    Rigidbody2D rb;
    protected GameObject owner;
    [SerializeField] GameObject effectOnHit;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 force = transform.up * projectileSpeed;
        rb.AddForce(force, ForceMode2D.Impulse);
        Invoke("Deactivate", timeUntilDestruction);
    }

    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public virtual void SetProjectileInfo(float damage, float critChance, GameObject owner)
    {
        this.buffedDamage = damage;
        this.buffedCritChance = critChance;
        this.owner = owner;
    }
    public float GetDamage()
    {
        return buffedDamage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Deactivate();
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log($"Applied {buffedDamage} damage points");
            bool crit = Random.Range(0, 100) < buffedCritChance;
            buffedDamage = crit ? buffedDamage * 2 : buffedDamage;
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            float remainingHealth = e.TakeDamage(buffedDamage);
            if (remainingHealth <= 0)
            {
                owner.GetComponent<Character>().AddExp(1f);
                owner.GetComponent<Character>().AddSpecial(5f);
            }
            HitNumber.CreateHitNumber(collision.gameObject.transform.position, buffedDamage, crit);
            GameObject go = Instantiate(effectOnHit);
            go.transform.position = collision.gameObject.transform.position;
            piercingsLeft--;
            if (piercingsLeft <= 0)
            {
                Deactivate();
            }
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Group"))
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(buffedDamage);
            Deactivate();
            return;
        }
    }
    protected virtual void OnEnable()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        Vector2 force = transform.up * projectileSpeed;
        rb.AddForce(force, ForceMode2D.Impulse);
        Invoke("Deactivate", timeUntilDestruction);
        piercingsLeft = pierceResistance;
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    protected virtual void OnTriggerExit2D(Collider2D collider2D)
    {

    }
}
