using UnityEngine;

public interface IHealth
{
    void TakeDamage(float damageAmount);
    void Heal(float healAmount);
    float GetHealth();
    float GetHealthPercentage();
    int GetDefence();
}
