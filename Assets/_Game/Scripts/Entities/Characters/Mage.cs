using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{
    [SerializeField] CharacterAttack WarriorCombo;
    [SerializeField] CharacterAttack ArcherCombo;
    int selectedAttack = 0;
    protected override void Awake()
    {
        base.Awake();
        WarriorCombo = Instantiate(WarriorCombo);
        ArcherCombo = Instantiate(ArcherCombo);
    }
    public override IEnumerator AttackCombined(Character other, Vector2 target)
    {
        yield return StartCoroutine(base.AttackCombined(other, target));
        if (other is Warrior)
        {
            WarriorCombo.ExecuteAttack(transform, target, stats);
            yield return new WaitForSeconds(0.4f);
        }
        else if (other is Archer)
        {
            ArcherCombo.ExecuteAttack(transform, target, stats);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
