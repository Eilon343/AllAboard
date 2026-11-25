using UnityEngine;

[CreateAssetMenu(menuName = "Game/EconomyConfig")]
public class EconomyConfig : ScriptableObject
{
    [Header("Sources")]
    public int coinsPerLevel = 20;
    public int dailyGift = 50;
    public int plunderReward = 5;

    [Header("Sinks")]
    public int continuePrice = 50;

    [Header("Upgrades")]
    public int upgradeBaseCost = 150;
    public float upgradeGrowth = 1.5f;
    public int upgradeMaxLevel = 10;

    public int UpgradeCost(int level) =>
        Mathf.RoundToInt(
            upgradeBaseCost * Mathf.Pow(upgradeGrowth, level));

    [Header("HP upgrade")]
    public int baseHp = 100;
    public int hpPerUpgrade = 10;

    public int MaxHp(int level) =>
        baseHp + hpPerUpgrade * level;

    [Header("Encounter - Fixed Probability V1")]
    [Range(0f, 1f)]
    public float friendlyWinChance = 0.7f;

    [Range(0f, 1f)]
    public float aggressiveWinChance = 0.45f;
}