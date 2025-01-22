using UnityEngine;
using System;

public class HealthSystem
{
    private float health;
    private float maxHealth;
    private int defence;
    private float defenceEffectivness = 0.03f;

    public event EventHandler OnHealthChanged;

    public HealthSystem(float maxHealth, int defence)
    {
        // settting the objects health and defence
        this.maxHealth = maxHealth;
        this.defence = defence;
        health = maxHealth;
        
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }
    public int GetDefence()
    {
        return defence;
    }

    public void TakeDamage(int damageAmount)
    {
        // calculates the total damage received after defence is considered, then removes total damage received from health
        float totalDamage;
        totalDamage = damageAmount * (1 - (defence * defenceEffectivness));
        health -= totalDamage;

        //fires off the event letting any other scripts listening that the health has changed
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        // heals object for specified amount
        health += healAmount;

        // checks if objects health exceeds max health and resets it to max health if necessary
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        //fires off the event letting any other scripts listening that the health has changed
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);

    }

}
