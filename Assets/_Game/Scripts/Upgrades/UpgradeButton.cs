using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Upgrade upgrade;
    [SerializeField] TMPro.TMP_Text upgradeNameText;
    [SerializeField] TMPro.TMP_Text upgradeDescriptionText;
    [SerializeField] Image upgradeImage;
    [SerializeField] CharacterUpgrader charToUpgrade;

    int siblingIndex;
    Transform parent;
    Vector3 originalPosition;
    public void Init(Upgrade upgrade, CharacterUpgrader charToUpgrade)
    {
        this.upgrade = upgrade;
        this.charToUpgrade = charToUpgrade;
        SetupButton(upgrade);
    }
    public void SetupButton(Upgrade upg)
    {
        upgradeDescriptionText.text = Upgrade.GetDescription(upgrade);
        upgradeDescriptionText.color = Upgrade.RarityToColor[upg.rarity];
        upgradeImage.sprite = upg.upgradeSprite;
    }
    public void SelectUpgrade()
    {
        if (upgrade != null)
        {
            charToUpgrade.SetUpgrade(upgrade);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        parent = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        GetComponent<Button>().interactable = false;
        upgradeImage.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parent);
        transform.SetSiblingIndex(siblingIndex);
        foreach (var obj in eventData.hovered)
        {
            var otherUpgrade = obj.GetComponent<UpgradeButton>();
            if (otherUpgrade != null && obj != gameObject)
            {
                if (otherUpgrade.upgrade.rarity == upgrade.rarity)
                {
                    if (MergeUpgrades(otherUpgrade, upgrade, otherUpgrade.upgrade))
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
        transform.position = originalPosition;
        GetComponent<Button>().interactable = true;
        upgradeImage.raycastTarget = true;
    }

    public bool MergeUpgrades(UpgradeButton upgradeButton, Upgrade upg1, Upgrade upg2)
    {
        Upgrade upgrade = ScriptableObject.CreateInstance<Upgrade>();
        upgrade.upgradeSprite = upg1.upgradeSprite;
        upgrade.rarity = upg1.rarity + 1;
        upgrade.healthBuff = upg1.healthBuff + upg2.healthBuff;
        upgrade.fireRateBuff = upg1.fireRateBuff + upg2.fireRateBuff;
        upgrade.speedBuff = upg1.speedBuff + upg2.speedBuff;
        upgrade.luckBuff = upg1.luckBuff + upg2.luckBuff;
        upgrade.damageBuff = upg1.damageBuff + upg2.damageBuff;
        upgrade.critChanceBuff = upg1.critChanceBuff + upg2.critChanceBuff;
        upgrade.rangeBuff = upg1.rangeBuff + upg2.rangeBuff;
        upgrade.specialBuff = upg1.specialBuff + upg2.specialBuff;
        upgrade.expBuff = upg1.expBuff + upg2.expBuff;
        upgradeButton.upgrade = upgrade;
        upgradeButton.SetupButton(upgrade);
        if (upgrade != null && upgradeButton != null)
        {
            return true;
        }
        return false;
    }
}
