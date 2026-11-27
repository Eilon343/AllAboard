using TMPro;
using UnityEngine;
using UnityEngine.UI;

// HpDisplay.cs: on TXT_HP in Home. Shows the ship's max HP from the HP upgrade level.
[DisallowMultipleComponent]
public class HpDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image hullFill;
    // [SerializeField] private Image hullBackground;

    private ShipHealth _health;

    private void OnEnable()
    {
        _health = Services.PlayerHealth;

        if (_health == null)
            return;

        _health.Changed += Show;
        Show();
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.Changed -= Show;
    }

    public void Show()
    {
        if (_health == null)
            return;

        if (label != null)
            label.text =
                $"{_health.CurrentHp}/{_health.MaxHp}";

        if (hullFill != null)
            hullFill.fillAmount = _health.NormalizedHP;
    }
}
