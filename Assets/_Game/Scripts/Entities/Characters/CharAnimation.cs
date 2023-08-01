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
    [SerializeField] CharacterStats stats;
    AnimState currentState;
    public void ChangeState(AnimState state)
    {
        if (state == currentState || animator.runtimeAnimatorController == null) return;

        animator.SetFloat("AttackSpeed", stats.GetBuffedFireRate());
        animator.SetFloat("MoveSpeed", Group.CurrentGroup.GetGroupSpeed() / 4f);
        
        animator.Play(System.Enum.GetName(typeof(AnimState), state));   
    }
}
