// Assets/Editor/TestSave.cs: runs only in the editor, never in the game
using UnityEditor;
using UnityEngine;

// Tools > Reset Test Save: 1000 coins and no upgrades, to test the upgrade button.
static class TestSave
{
    const int TestCoins = 1000;   // editor test value only, not part of the economy

    [MenuItem("Tools/Reset Test Save (1000 coins, no upgrades)")]
    static void Reset()
    {
        PlayerPrefs.SetInt(CoinCounter.Key, TestCoins);
        PlayerPrefs.SetInt(UpgradeButton.LevelKey, 0);
        PlayerPrefs.Save();
        Debug.Log($"Test save: {TestCoins} coins, HP upgrade level 0");
    }
}
