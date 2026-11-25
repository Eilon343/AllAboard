using TMPro;
using UnityEngine;

// HpDisplay.cs: on TXT_HP in Home. Shows the ship's max HP from the HP upgrade level.
public class HpDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    void Start() => Show();

    public void Show()
    {
        int level = Services.Save.GetInt(UpgradeButton.LevelKey);
        label.text = $"HP {Services.Config.Economy.MaxHp(level)}";
    }
}
