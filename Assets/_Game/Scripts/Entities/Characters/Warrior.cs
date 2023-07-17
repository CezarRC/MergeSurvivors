using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{
    [SerializeField] CharacterAttack mageCombo;
    [SerializeField] CharacterAttack archerCombo;
    protected override void Awake()
    {
        base.Awake();
        mageCombo = Instantiate(mageCombo);
        archerCombo = Instantiate(archerCombo);
    }
    public override IEnumerator AttackCombined(Character other, Vector2 target)
    {
        yield return StartCoroutine(base.AttackCombined(other, target));
        if (other is Mage)
        {
            for (int i = 0; i < 3; i++)
            {
                mageCombo.ExecuteAttack(transform, target, stats);
                yield return new WaitForSeconds(0.15f);
            }
        }
        else if (other is Archer)
        {
            archerCombo.ExecuteAttack(transform, target, stats);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
