using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    IDLE,
    RUN,
    ATTACK,
    DEATH
}
public class CharAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    CharacterStats stats;
    AnimState currentState;
    private void Start()
    {
        stats = GetComponent<CharacterStats>();
    }
    public void ChangeState(AnimState state)
    {
        if (state == currentState || animator.runtimeAnimatorController == null) return;
        
        animator.Play(System.Enum.GetName(typeof(AnimState), state));
        
        animator.SetFloat("AttackSpeed", stats.GetBuffedFireRate());
        animator.SetFloat("MoveSpeed", Group.CurrentGroup.GetGroupSpeed() / 4f);
    }
}
