using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Character : MonoBehaviour
{
    public static List<Character> characters = new List<Character>();
    public static UnityEvent OnCharacterDied = new UnityEvent();
    [SerializeField] float followThreshold = 0.4f;
    [SerializeField] float slideSpeed = 1000f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Transform formationPlacement;
    [SerializeField] GameObject playerSprite;
    Rigidbody2D rb;
    protected bool isControlled;
    [SerializeField] protected Vector2 target;
    public CharacterStats stats { get; private set; }
    CharacterUpgrader characterUpgrader;
    public CharAnimation charAnimation { get; private set; }

    [SerializeField] CharacterAttack baseAttack;
    [SerializeField] SoundEffect deathSound;

    [SerializeField] UnityEngine.UI.Slider specialBar;
    [SerializeField] UnityEngine.UI.Slider levelBar;

    private float exp = 0f;
    private float nextLevelExp = 10f;
    private float currentSpecial;
    public int level { get; private set; } = 1 ;

    protected virtual void Awake()
    {
        characters.Add(this);
        InvokeRepeating(nameof(SetTargetPosition), 0, 0.5f);
        InvokeRepeating(nameof(AutoAttack), 0, 0.1f);
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        characterUpgrader = GetComponent<CharacterUpgrader>();
        charAnimation = GetComponent<CharAnimation>();
        currentSpecial = stats.BaseSpecial;
        baseAttack = Instantiate(baseAttack);
}
    protected virtual void Start()
    {
        GetComponent<CharacterHealth>().OnHealthDepleted.AddListener(Die);
    }
    private void FixedUpdate()
    {
        Move();
        FaceTarget();
    }
    public void Move()
    {
        if (formationPlacement == null)
        {
            return;
        }
        Vector2 direction = (formationPlacement.position - transform.position);
        if (direction.sqrMagnitude > followThreshold * followThreshold)
        {
            rb.AddForce(direction.normalized * slideSpeed * Time.fixedDeltaTime);   
        }
        //Guarantee Correct Draw Order
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
    public void FaceTarget()
    {
        Vector2 delta = (target - (Vector2)transform.position).normalized;
        Vector3 cross = Vector3.Cross(delta, Vector3.forward);
        if (cross.y < 0.0f)
        {
            // Target is to the right
            playerSprite.transform.rotation = Quaternion.Euler(0f, 0, 0f);
        }
        else
        {
            // Target is to the left
            playerSprite.transform.rotation = Quaternion.Euler(0f, 180, 0f);
        }
    }
    public bool CanAttackCombined()
    {
        return currentSpecial == stats.GetBuffedSpecial();
    }
    public virtual IEnumerator AttackCombined(Character other, Vector2 target)
    {
        //Attack Target
        currentSpecial -= 100f;
        specialBar.value = Mathf.Lerp(specialBar.value, currentSpecial / stats.GetBuffedSpecial(), 20f * Time.deltaTime);
        yield break;
    }
    public void AutoAttack()
    {
        if (baseAttack.CanAttack() && target != Vector2.zero)
        {
            baseAttack.ExecuteAttack(transform, target, stats);
            AudioManager.Instance.Play(baseAttack.attackSound);
        }
            
    }
    public virtual void SetTargetPosition()
    {
        //Boxcast to find the closest target
        RaycastHit2D[] enemies = Physics2D.CircleCastAll(transform.position, stats.GetBuffedRange(), Vector2.up, 0f, enemyLayer);
        if (enemies == null || enemies.Length == 0)
        {
            target = Vector2.zero;
            return;
        }
        enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).Take(1);
        target = enemies[0].transform.position;
    }
    public virtual void SetTargetPosition(Vector2 newTarget)
    {
        target = newTarget;
    }
    public void SetPlacement(Transform newPlacement)
    {
        formationPlacement = newPlacement;
    }
    public void SetControlled(bool controlled)
    {
        isControlled = controlled;
    }
    public void AddExp(float amount)
    {
        exp += amount * stats.buffs.expBuffMultiplier;
        if (exp >= nextLevelExp)
        {
            level++;
            exp = 0f;
            nextLevelExp *= 1.15f;
            characterUpgrader.GenerateUpgradeUI();
            AudioManager.Instance.Play(SoundEffect.LEVELUP);
        }
        levelBar.value = Mathf.Lerp(levelBar.value, exp / nextLevelExp, 20f * Time.deltaTime);
    }
    public void AddSpecial(float amount)
    {
        currentSpecial += amount * stats.buffs.specialBuffMultiplier;

        if (currentSpecial > stats.GetBuffedSpecial())
        {
            currentSpecial = stats.GetBuffedSpecial();
        }
        specialBar.value = Mathf.Lerp(specialBar.value, currentSpecial / stats.GetBuffedSpecial(), 20f * Time.deltaTime);
    }
    public void Die()
    {
        gameObject.SetActive(false);
        CancelInvoke();
        characters.Remove(this);
        OnCharacterDied?.Invoke();
    }
    public void OnDestroy()
    {
        characters.Remove(this);
    }
}
