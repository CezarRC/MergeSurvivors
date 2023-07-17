using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public static UnityEvent OnEnemyDead = new UnityEvent();
    [SerializeField] float baseHealth = 10f;
    [SerializeField] float baseDamage = 1;
    [SerializeField] float baseSpeed = 500f;
    [SerializeField] float baseFireRate = 1;
    [SerializeField] float baseRange = 10;

    public float health { get; private set; } = 10f;
    public float damage { get; private set; } = 1;
    public float speed { get; private set; } = 500f;
    public float fireRate { get; private set; } = 1;
    public float range { get; private set; } = 10;

    float lastTimeHitting;
    public float expOnDeath { get; private set; }

    [SerializeField] EnemyAttack attack;
    [SerializeField] SoundEffect attackSound;
    [SerializeField] SoundEffect onDeath;

    Rigidbody2D rb;
    Group group;
    private void OnEnable()
    {
        health = baseHealth;
        damage = baseDamage;
        speed = baseSpeed;
        expOnDeath = health / 10f;
        InvokeRepeating(nameof(Attack), 0, 0.2f);
        InvokeRepeating(nameof(CheckDespawn), 0, 0.5f);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        group = Group.CurrentGroup;
        InvokeRepeating(nameof(Attack), 0, 0.2f);
        InvokeRepeating(nameof(CheckDespawn), 0, 0.5f);
        attack = attack == null ? null : Instantiate(attack);
    }
    private void FixedUpdate()
    {
        FollowGroup();
    }
    private void CheckDespawn()
    {
        if ((group.transform.position - transform.position).sqrMagnitude > 100*100)
        {
            gameObject.SetActive(false);
        }
    }
    private void FollowGroup()
    {
        Vector2 direction = group.transform.position - transform.position;
        Vector2 movement = direction.normalized * speed * Time.deltaTime;
        rb.AddForce(movement);

        //Guarantee Correct Draw Order
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
    protected virtual void Attack()
    {
        if (attack == null) return;
        if (attack.CanAttack() && (transform.position - group.transform.position).sqrMagnitude < range * range)
        {
            AudioManager.Instance.Play(attackSound);
            attack.ExecuteAttack(transform, group.transform.position, this);
        }
    }
    public void FaceTarget()
    {
        Vector2 delta = ((Vector2)group.transform.position - (Vector2)transform.position).normalized;
        Vector3 cross = Vector3.Cross(delta, Vector3.forward);
        if (cross.y < 0.0f)
        {
            // Target is to the right
            transform.rotation = Quaternion.Euler(0f, 0, 0f);
        }
        else
        {
            // Target is to the left
            transform.rotation = Quaternion.Euler(0f, 180, 0f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Group"))
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
            AudioManager.Instance.Play(attackSound);
        }
            
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time < lastTimeHitting + fireRate)
        {
            return;
        }
        lastTimeHitting = Time.time;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Group"))
        {
            collision.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
            AudioManager.Instance.Play(attackSound);
        }

    }
    public float TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            AudioManager.Instance.Play(onDeath);
            gameObject.SetActive(false);
            OnEnemyDead?.Invoke();
        }
        return health;
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

    public float GetFireRate()
    {
        return fireRate;
    }
    public float GetRange()
    {
        return range;
    }
}
