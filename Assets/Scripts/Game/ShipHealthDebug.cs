using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ShipHealthDebug : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    [SerializeField] private int debugAmount = 10;

    private void Update()
    {
        if (Services.PlayerHealth == null)
            return;

#if ENABLE_INPUT_SYSTEM
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.f6Key.wasPressedThisFrame)
            Hit();

        if (keyboard.f7Key.wasPressedThisFrame)
            Repair();

        if (keyboard.f8Key.wasPressedThisFrame)
            Restore();

#elif ENABLE_LEGACY_INPUT_MANAGER

        if (Input.GetKeyDown(KeyCode.F6))
            Hit();

        if (Input.GetKeyDown(KeyCode.F7))
            Repair();

        if (Input.GetKeyDown(KeyCode.F8))
            Restore();

#endif
    }

    public void Hit()
    {
        Services.PlayerHealth.HitHP(debugAmount);
        LogHealth();
    }

    public void Repair()
    {
        Services.PlayerHealth.AddHP(debugAmount);
        LogHealth();
    }

    public void Restore()
    {
        Services.PlayerHealth.RestoreHP();
        LogHealth();
    }

    private void LogHealth()
    {
        var health = Services.PlayerHealth;

        Debug.Log(
            $"[HealthDebug] " +
            $"HP: {health.CurrentHp}/{health.MaxHp} | " +
            $"Fill: {health.NormalizedHP:P0}");
    }

#endif
}