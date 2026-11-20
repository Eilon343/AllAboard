using UnityEngine;

/// <summary>
/// Editable economy values for rewards and upgrade pricing.
/// Designers change this asset, not the gameplay scripts.
/// </summary>
[CreateAssetMenu(fileName = "EconomyConfig", menuName = "All Aboard/Economy Config")]
public sealed class EconomyConfig : ScriptableObject
{
    [Header("Friendly Encounter")]
    [Tooltip("Chance of success when choosing the friendly option (0 to 1).")]
    [SerializeField, Range(0f, 1f)] private float _friendlyWinChance = 0.7f;

    [Tooltip("Coins granted for a successful friendly encounter.")]
    [SerializeField, Min(0)] private int _friendlyReward = 20;

    [Header("Aggressive Encounter")]
    [Tooltip("Chance of success when choosing an aggressive encounter (0 to 1).")]
    [SerializeField, Range(0f, 1f)] private float _aggressiveWinChance = 0.45f;

    [Tooltip("Coins granted for a successful aggressive encounter.")]
    [SerializeField, Min(0)] private int _aggressiveReward = 50;

    [Header("Failure")]
    [Tooltip("Coins granted when an encounter is lost.")]
    [SerializeField, Min(0)] private int _loseReward = 5;

    [Header("Upgrade Economy — for SCRUM-11")]
    [Tooltip("Cost of buying the first upgrade level.")]
    [SerializeField, Min(1)] private int _upgradeBasePrice = 50;

    [Tooltip("How quickly the next upgrade price increases per level.")]
    [SerializeField, Min(1f)] private float _upgradeGrowth = 1.5f;

    public float FriendlyWinChance => _friendlyWinChance;
    public int FriendlyReward => _friendlyReward;
    public float AggressiveWinChance => _aggressiveWinChance;
    public int AggressiveReward => _aggressiveReward;
    public int LoseReward => _loseReward;

    /// <summary>Calculate the price of the next upgrade from its current level.</summary>
    public int GetUpgradePrice(int currentLevel)
    {
        float price = _upgradeBasePrice * Mathf.Pow(_upgradeGrowth, Mathf.Max(0, currentLevel));
        return Mathf.RoundToInt(Mathf.Min(price, int.MaxValue));
    }
}
