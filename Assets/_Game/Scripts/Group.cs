using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Group : MonoBehaviour
{
    public static Group CurrentGroup { get; private set; }
    Vector3 movement;
    Vector2 moveDirection;
    Vector2 aimPosition;

    [SerializeField] Transform[] formationPlaces;
    [SerializeField] List<Character> characters;

    bool attacking;
    int activeCharacter = 0;
    int formationRotations;

    float timeBetweenRotations = 1f;
    float lastRotationTime = 0;
    [SerializeField] float speedLimitDivisor = 200f;
    Rigidbody2D rb;

    [SerializeField] bool useTransform;

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
        characters = Character.characters;
        Enemy.OnEnemyDead.AddListener(HandleEnemyDead);
        Character.OnCharacterDied.AddListener(HandleCharacterDeath);
        rb = GetComponent<Rigidbody2D>();
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
    void HandleCharacterDeath()
    {
        characters = Character.characters;
        if (characters.Count == 0)
        {
            GameManager.Instance.GameOver();
            return;
        }
        while (characters[activeCharacter] == null)
        {
            FormationChangeClockwise();
        }
    }
    private void Move()
    {
        movement = moveDirection * characters[activeCharacter].stats.GetBuffedSpeed() * Time.deltaTime;
        movement.z = movement.y;
        if (movement != Vector3.zero)
        {
            foreach (var character in characters)
            {
                character.charAnimation.ChangeState(AnimState.RUN);
            }
        }
        else
        {
            foreach (var character in characters)
            {
                character.charAnimation.ChangeState(AnimState.IDLE);
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
            if (activeCharacter >= characters.Count || Time.timeScale == 0)
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

    public void FormationChangeClockwise()
    {
        for (int i = 0; i < characters.Count; i++)
        {
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
    public void FormationChangeCounterClockwise()
    {

    }
}
