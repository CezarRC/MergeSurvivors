using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    CharacterStats stats;
    float maxHealth = 100;
    [SerializeField] float currentHealth = 100;
    [SerializeField] UnityEngine.UI.Slider Healthbar;

    public UnityEvent OnHealthDepleted;
    private void Awake()
    {
        if (OnHealthDepleted == null)
        {
            OnHealthDepleted = new UnityEvent();
        }
    }
    private void Start()
    {
        StartCoroutine(Init());
    }
    private IEnumerator Init()
    {
        stats = GetComponent<CharacterStats>();
        if (stats.buffs == null)
        {
            yield return null;
        }
        UpdateMaxHealth();
        currentHealth = maxHealth;
    }
    void UpdateMaxHealth()
    {
        float actualHealthPercentage = currentHealth / maxHealth;
        maxHealth = stats.GetBuffedHealth();
        currentHealth = maxHealth * actualHealthPercentage;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Healthbar.value = Mathf.Lerp(Healthbar.value, currentHealth / maxHealth, 20f * Time.deltaTime);
        if (currentHealth <= 0)
        {
            OnHealthDepleted?.Invoke();
        }
    }
}
