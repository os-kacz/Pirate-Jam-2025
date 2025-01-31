using System;

public static class DamageEvent
{
    public static event Action<IHealth, float> OnDamageDealt;

    public static void TriggerDamage(IHealth target, float damageAmount)
    {
        OnDamageDealt?.Invoke(target, damageAmount);
    }
}
