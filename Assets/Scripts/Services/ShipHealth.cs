using System;
using UnityEngine;

public sealed class ShipHealth
{
    public int CurrentHp { get; private set; }
    public int MaxHp { get; private set; }

    public float NormalizedHP =>
        (float)CurrentHp / MaxHp;

    public bool IsDestroyed => CurrentHp == 0;

    // Notify listeners whenever health changes.
    public event Action Changed;

    public ShipHealth(int maxHp)
    {
        MaxHp = Mathf.Max(1, maxHp);
        CurrentHp = MaxHp;
    }

    public void HitHP(int damage)
    {
        if (damage <= 0)
            return;

        SetHP(CurrentHp - damage);
    }

    public void AddHP(int amount)
    {
        if (amount <= 0)
            return;

        int missingHp = MaxHp - CurrentHp;
        SetHP(CurrentHp + Mathf.Min(amount, missingHp));
    }

    public void RestoreHP()
    {
        SetHP(MaxHp);
    }

    public void SetMaxHp(int newMaxHp)
    {
        newMaxHp = Mathf.Max(1, newMaxHp);

        if (newMaxHp == MaxHp)
            return;

        int missingHp = MaxHp - CurrentHp;
        bool wasDestroyed = IsDestroyed;

        MaxHp = newMaxHp;

        // Preserve existing damage when upgrading.
        CurrentHp = wasDestroyed
            ? 0
            : Mathf.Clamp(MaxHp - missingHp, 0, MaxHp);

        Changed?.Invoke();
    }

    private void SetHP(int value)
    {
        int nextHp = Mathf.Clamp(value, 0, MaxHp);

        if (nextHp == CurrentHp)
            return;

        CurrentHp = nextHp;

        Changed?.Invoke();
    }
}