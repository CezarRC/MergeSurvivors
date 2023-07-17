using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LootDropper : MonoBehaviour
{
    static readonly Dictionary<Rarity, float> RarityToChance = new Dictionary<Rarity, float> {
        { Rarity.Common, 100000 },
        { Rarity.Uncommon, 10000 },
        { Rarity.Rare, 5000 },
        { Rarity.Epic, 1000 },
        { Rarity.Legendary, 100 },
        { Rarity.Godly, 1 }
    };
    public static LootDropper Instance { get; private set; }

    List<Upgrade> availableUpgrades;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        availableUpgrades = new List<Upgrade>();
    }

    private void Start()
    {
        availableUpgrades = Resources.LoadAll("Upgrades/", typeof(Upgrade)).Cast<Upgrade>().ToList();
    }
    public List<Upgrade> GetRandomLoot(float luck, int amount)
    {
        List<Upgrade> upgrades = new List<Upgrade>();
        for (int i = 0; i < amount; i++)
        {
            float rarity = Random.Range(1, RarityToChance[Rarity.Common]);
            float rarityLuckyfier = Mathf.Pow(0.5f, 1 * luck);
            rarity *= rarityLuckyfier;
            foreach (var item in RarityToChance.Reverse())
            {
                if (rarity <= item.Value)
                {
                    List<Upgrade> upgradesOfRarity = availableUpgrades.Where<Upgrade>(u => { return u.rarity == item.Key; }).Cast<Upgrade>().ToList();
                    upgrades.Add(upgradesOfRarity[Random.Range(0, upgradesOfRarity.Count)]);
                    break;
                }
            }
        }
        return upgrades;
    }
}
