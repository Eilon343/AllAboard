

using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public const string LevelKey = "upgrade.hp";
    [SerializeField] TMP_Text label;
    [SerializeField] CoinCounter coins;
    [SerializeField] HpDisplay hp;               // optional: refreshed after each purchase
    EconomyConfig Economy => Services.Config.Economy;
    int Level => Services.Save.GetInt(LevelKey);
    int Cost => Economy.UpgradeCost(Level);
    bool IsMax => Level >= Economy.upgradeMaxLevel;
    void Start() => Show();
    public void Buy()
    {
        if (IsMax || coins.Coins < Cost) return;
        coins.Add(-Cost);
        Services.Save.SetInt(LevelKey, Level + 1);
        Show();
        if (hp != null) hp.Show();
    }
    void Show() => label.text = IsMax ? "HP max" : $"HP {Level + 1}: {Cost}";
}
