using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles one temporary ship encounter per level.
/// The player sails into the trigger, picks Friendly or Aggressive,
/// sees a random result, and receives a saved coin reward exactly once.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public sealed class EncounterShip : MonoBehaviour
{
    private const string CoinsKey = "coins";

    [Header("World")]
    [Tooltip("The Rigidbody2D belonging to the player ship.")]
    [SerializeField] private Rigidbody2D _playerBody;

    [Tooltip("Temporarily stop player sailing when the encounter starts.")]
    [SerializeField] private ShipMotor2D _shipMotor;

    [Header("UI — Existing Level Canvas")]
    [Tooltip("Popup containing TXT_Encounter and BTN_Interact.")]
    [SerializeField] private GameObject _encounterPanel;

    [Tooltip("Popup containing TXT_Result and BTN_ReturnHome.")]
    [SerializeField] private GameObject _resultPanel;

    [Tooltip("Checked = Friendly, unchecked = Aggressive.")]
    [SerializeField] private Toggle _modeToggle;

    [Tooltip("Text showing the resolved outcome and number of earned coins.")]
    [SerializeField] private TMP_Text _resultText;

    private bool _canInteract;
    private bool _resolved;

    /// <summary>Begin a fresh run with both popups hidden.</summary>
    private void Start()
    {
        if (_encounterPanel != null) _encounterPanel.SetActive(false);
        if (_resultPanel != null) _resultPanel.SetActive(false);
    }

    /// <summary>Offer an interaction when the player enters this world's trigger.</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerBody == null || _resolved || _canInteract ||
            other.attachedRigidbody != _playerBody)
            return;

        _canInteract = true;
        if (_shipMotor != null) _shipMotor.enabled = false;
        if (_playerBody != null)
        {
            _playerBody.linearVelocity = Vector2.zero;
            _playerBody.angularVelocity = 0f;
        }
        if (_encounterPanel != null) _encounterPanel.SetActive(true);
    }

    /// <summary>
    /// Called by BTN_Interact. Roll once using the active mode and EconomyConfig,
    /// persist the reward, and show the result popup.
    /// </summary>
    public void Interact()
    {
        if (!_canInteract || _resolved) return;

        if (Services.Save == null || Services.Config?.Economy == null ||
            _modeToggle == null || _resultText == null)
        {
            Debug.LogError("EncounterShip: assign UI fields and Bootstrap economy services.", this);
            return;
        }

        EconomyConfig economy = Services.Config.Economy;
        bool friendly = _modeToggle.isOn;
        float winChance = friendly ? economy.FriendlyWinChance : economy.AggressiveWinChance;
        bool won = Random.value < winChance;
        int reward = won
            ? (friendly ? economy.FriendlyReward : economy.AggressiveReward)
            : economy.LoseReward;

        _resolved = true;
        Services.Save.SetInt(CoinsKey, Services.Save.GetInt(CoinsKey) + reward);

        string choice = friendly ? "Friendly" : "Aggressive";
        string result = won ? "WIN" : "LOSE";
        _resultText.text = $"{choice} encounter\n{result}\n+{reward} coins";

        if (_encounterPanel != null) _encounterPanel.SetActive(false);
        if (_resultPanel != null) _resultPanel.SetActive(true);
    }

    /// <summary>Called by BTN_ReturnHome after the outcome has been shown.</summary>
    public void ReturnHome()
    {
        if (!_resolved) return;
        SceneManager.LoadScene("Home");
    }
}
