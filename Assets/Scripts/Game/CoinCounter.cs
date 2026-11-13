using TMPro;
using UnityEngine;

// CoinCounter.cs (Assets/Scripts/Game): on your coin text in the HUD
public class CoinCounter : MonoBehaviour
{
    [SerializeField] TMP_Text label;         // drag your coin text here

    void Start() => Show();

    public void Add(int amount)              // a button: On Click () -> CoinCounter.Add, 10
    {
        Services.Save.SetInt("coins", Services.Save.GetInt("coins") + amount);
        Show();
    }

    void Show() => label.text = Services.Save.GetInt("coins").ToString();
}
