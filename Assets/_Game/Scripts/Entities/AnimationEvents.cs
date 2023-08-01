using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] Character character;

    public void AttackEvent()
    {
        character.Attack();
    }

    public void Death()
    {
        transform.parent.gameObject.SetActive(false);
        Group.CurrentGroup.HandleCharacterDeath();
    }
}
