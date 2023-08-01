using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUpgrader : MonoBehaviour
{
    UpgradeUI upgradeUI;
    [SerializeField] GameObject upgradeButtonPrefab;
    CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        upgradeUI = FindObjectOfType<UpgradeUI>(true);
    }
    public void GenerateUpgradeUI()
    {
        upgradeUI.gameObject.SetActive(true);
        upgradeUI.SetupUpgradeUI(GetComponent<Character>());

        Time.timeScale = 0;

        List<Upgrade> randomUpgrades = LootDropper.Instance.GetRandomLoot(characterStats.GetBuffedLuck(), 3);
        foreach (var upgrade in randomUpgrades)
        {
            GameObject go = Instantiate(upgradeButtonPrefab);
            go.GetComponent<UpgradeButton>().Init(upgrade,this);
            go.transform.SetParent(upgradeUI.transform, false);
        }
        return;
    }
    public void SetUpgrade(Upgrade upgrade)
    {
        characterStats.GetBuffs().ApplyBuffs(upgrade);
        CloseUI();
    }
    void CloseUI()
    {
        UpgradeButton[] buttons = upgradeUI.GetComponentsInChildren<UpgradeButton>();
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
        upgradeUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
