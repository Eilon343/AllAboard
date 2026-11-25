using TMPro;
using UnityEngine;

// CoinCounter.cs (Assets/Scripts/Game): on your coin text in the HUD
public class CoinCounter : MonoBehaviour
{
    public const string Key = "coins"; // key for saving coins in PlayerPrefs
    [SerializeField] TMP_Text label;         // drag your coin text here
    public int Coins => Services.Save.GetInt(Key); // get the current coin count

    void Start() => Show();

    public void Add(int amount)              // a button: On Click () -> CoinCounter.Add, 10
    {
        Services.Save.SetInt(Key, Services.Save.GetInt(Key) + amount);
        Show();
    }

    void Show() => label.text = Services.Save.GetInt(Key).ToString();
}
