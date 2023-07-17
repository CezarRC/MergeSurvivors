using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character
{
    [SerializeField] CharacterAttack mageCombo;
    [SerializeField] CharacterAttack warriorCombo;

    protected override void Awake()
    {
        base.Awake();
        warriorCombo = Instantiate(warriorCombo);
        mageCombo = Instantiate(mageCombo);
    }
    public override IEnumerator AttackCombined(Character other, Vector2 target)
    {
        yield return StartCoroutine(base.AttackCombined(other, target));
        if (other is Mage)
        {
            for (int i = 0; i < 10; i++)
            {
                mageCombo.ExecuteAttack(transform, target, stats);
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (other is Warrior)
        {
            for (int i = 0; i < 3; i++)
            {
                warriorCombo.ExecuteAttack(transform, target, stats);
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}
