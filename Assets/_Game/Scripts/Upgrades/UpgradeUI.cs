using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Image characterLeveled;
    [SerializeField] TMPro.TMP_Text LevelUpText;

    public void SetupUpgradeUI(Character character)
    {
        characterLeveled.sprite = character.GetComponentInChildren<SpriteRenderer>().sprite;
        characterLeveled.rectTransform.sizeDelta = character.GetComponentInChildren<SpriteRenderer>().sprite.rect.size * 2;
        LevelUpText.text = $"Congratulations! {character.name} is now level {character.level}!";
    }
}
