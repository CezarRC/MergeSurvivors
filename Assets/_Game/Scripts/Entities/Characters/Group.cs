using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Group : MonoBehaviour
{
    public static Group CurrentGroup { get; private set; }

    [SerializeField] Transform[] formationPlaces;
    [SerializeField] List<Character> characters;

    [SerializeField] private bool useTransform = false;
    private bool attacking;
    private int activeCharacter = 0;
    private int formationRotations;
    private int aliveCharacterAmount;

    private float timeBetweenRotations = 1f;
    private float lastRotationTime = 0;
    [SerializeField] private float speedLimitDivisor = 200f;
    private Rigidbody2D rb;
    private Vector3 movement;
    private Vector2 moveDirection;
    private Vector2 aimPosition;

    private void Awake()
    {
        if (CurrentGroup != null && CurrentGroup != this)
        {
            Destroy(gameObject);
        }
        CurrentGroup = this;
    }
    void Start()
    {
        Enemy.OnEnemyDead.AddListener(HandleEnemyDead);
        rb = GetComponent<Rigidbody2D>();
        aliveCharacterAmount = characters.Count;
        GameManager.Instance.StartGame();
    }
    void FixedUpdate()
    {
        if (!attacking) Move();
    }
    void HandleEnemyDead()
    {
        GameManager.Instance.AddScore(1);
    }
    public void HandleCharacterDeath()
    {
        aliveCharacterAmount--;
        if (aliveCharacterAmount <= 0)
        {
            GameManager.Instance.GameOver();
            return;
        }
    }
    private void Move()
    {
        movement = moveDirection * characters[activeCharacter].stats.GetBuffedSpeed() * Time.deltaTime;
        if (movement != Vector3.zero)
        {
            foreach (var character in characters)
            {
                if (!character.IsAttacking)
                {
                    character.charAnimation.ChangeState(AnimState.RUN);
                }
            }
        }
        else
        {
            foreach (var character in characters)
            {
                if (!character.IsAttacking)
                {
                    character.charAnimation.ChangeState(AnimState.IDLE);
                }
            }
        }

        if (useTransform)
        {
            transform.Translate(movement);
        }
        else
        {
            rb.MovePosition(transform.position + movement);
        }
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>().normalized;
    }
    public float GetGroupSpeed()
    {
        return characters[activeCharacter].stats.GetBuffedSpeed();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (activeCharacter >= characters.Count || Time.timeScale == 0 || !characters[activeCharacter].gameObject.activeSelf)
            {
                return;
            }
            StartCoroutine(ActiveAttack());
        }

    }
    public IEnumerator ActiveAttack()
    {
        if (!characters[activeCharacter].CanAttackCombined())
        {
            yield break;
        }
        attacking = true;

        if (characters.Count > 1)
        {
            Vector2 delta = (aimPosition - (Vector2)transform.position).normalized;
            Vector3 cross = Vector3.Cross(delta, Vector3.forward);
            int characterToCombine;
            if (cross.y < 0.0f)
            {
                //Try combine attack with Right
                characterToCombine = (activeCharacter + 1) % characters.Count;
            }
            else
            {
                //Try Combine attack with left
                characterToCombine = activeCharacter == 0 ? characters.Count - 1 : activeCharacter - 1;
            }
            characters[activeCharacter].charAnimation.ChangeState(AnimState.ATTACK);
            characters[characterToCombine].charAnimation.ChangeState(AnimState.ATTACK);
            yield return new WaitForSeconds(0.4f);
            yield return StartCoroutine(characters[activeCharacter].AttackCombined(characters[characterToCombine], aimPosition));
        }
        attacking = false;
    }
    public void OnChangeCharacter(InputAction.CallbackContext context)
    {

        if (Time.time < timeBetweenRotations + lastRotationTime) return;
        lastRotationTime = Time.time;

        if (context.started)
        {
            formationRotations++;
            formationRotations %= formationPlaces.Length;
            FormationChangeClockwise();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        aimPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FindObjectOfType<PauseMenuUI>(true).Pause();
        }
    }

    public void FormationChangeClockwise()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].gameObject.activeSelf) return;

            int newPlace = (i + formationRotations) % formationPlaces.Length;

            characters[i].SetPlacement(formationPlaces[newPlace]);
            if (newPlace == 0)
            {
                activeCharacter = i;
                characters[i].SetControlled(true);
            }
            else
            {
                characters[i].SetControlled(false);
            }
        }
    }
}
