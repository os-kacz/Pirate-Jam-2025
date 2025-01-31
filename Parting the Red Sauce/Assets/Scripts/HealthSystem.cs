using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour, IHealth
{
    private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private int defence;
    private float defenceEffectivness = 0.03f;

    public event EventHandler OnHealthChanged;

    public void Start()
    {
        currentHealth = maxHealth;
        DamageEvent.OnDamageDealt += HandleDamage;
    }


    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    public int GetDefence()
    {
        return defence;
    }

    public void TakeDamage(float damageAmount)
    {
        // calculates the total damage received after defence is considered, then removes total damage received from health
        float totalDamage;
        totalDamage = damageAmount * (1 - (defence * defenceEffectivness));
        currentHealth -= totalDamage;

        //fires off the event letting any other scripts listening that the health has changed
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(float healAmount)
    {
        // heals object for specified amount
        currentHealth += healAmount;

        // checks if objects health exceeds max health and resets it to max health if necessary
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        //fires off the event letting any other scripts listening that the health has changed
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);

    }

    private void HandleDamage(IHealth target, float damage)
    {
        if (target == this)
        {
            TakeDamage(damage);
        }
    }
}
