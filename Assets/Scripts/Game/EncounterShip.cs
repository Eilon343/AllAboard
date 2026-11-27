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
    [Header("World")] [Tooltip("The Rigidbody2D belonging to the player ship.")] [SerializeField]
    private Rigidbody2D _playerBody;

    [Tooltip("Temporarily stop player sailing when the encounter starts.")] [SerializeField]
    private ShipMotor2D _shipMotor;

    [Header("UI — Existing Level Canvas")]
    [Tooltip("Popup containing TXT_Encounter and BTN_Interact.")]
    [SerializeField]
    private GameObject _encounterPanel;

    [Tooltip("Popup containing TXT_Result and BTN_ReturnHome.")] [SerializeField]
    private GameObject _resultPanel;

    [Tooltip("Checked = Friendly, unchecked = Aggressive.")] [SerializeField]
    private Toggle _modeToggle;

    [Tooltip("Text showing the resolved outcome and number of earned coins.")] [SerializeField]
    private TMP_Text _resultText;

    private bool _canInteract;
    private bool _resolved;

    // for locking Toggle interaction
    private bool _lockedFriendlyMode;

    [Header("HUD")] [SerializeField] private CoinCounter _coinCounter;

    /// <summary>Begin a fresh run with both popups hidden.</summary>
    private void Start()
    {
        if (_encounterPanel != null) _encounterPanel.SetActive(false);
        if (_resultPanel != null) _resultPanel.SetActive(false);
        if (_modeToggle != null) _modeToggle.interactable = true;
    }

    /// <summary>Offer an interaction when the player enters this world's trigger.</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerBody == null || _resolved || _canInteract ||
            other.attachedRigidbody != _playerBody)
            return;

        if (_modeToggle == null)
        {
            Debug.LogError("EncounterShip: Missing mode toggle.", this);
            return;
        }

        // Capture and lock the selected encounter mode.
        _lockedFriendlyMode = _modeToggle.isOn;
        _modeToggle.interactable = false;

        _canInteract = true;

        if (_shipMotor != null)
            _shipMotor.enabled = false;

        _playerBody.linearVelocity = Vector2.zero;
        _playerBody.angularVelocity = 0f;

        if (_encounterPanel != null)
            _encounterPanel.SetActive(true);
    }

    /// <summary>
    /// Resolves an encounter, calculates the stage and plunder
    /// rewards, saves the coins once, and displays the result.
    /// </summary>
    public void Interact()
    {
        if (!_canInteract || _resolved)
            return;

        if (Services.Save == null ||
            Services.Config?.Economy == null ||
            _modeToggle == null ||
            _resultText == null)
        {
            Debug.LogError(
                "EncounterShip: missing UI or economy services.",
                this);
            return;
        }

        EconomyConfig economy = Services.Config.Economy;

        // Capture the player's decision.
        bool friendly = _lockedFriendlyMode;

        // Prevent duplicate rewards.
        _resolved = true;

        // prevent change modes midPopup
        _modeToggle.interactable = false;

        // M4: Fixed Probability System.
        // Probabilities are configurable through EconomyConfig,
        // but do not depend on ship or fleet strength yet.
        //
        // Future expansion:
        // Friendly encounters may recruit allied ships.
        // Allies may assist in combat and escort missions.
        //
        // Proposed dynamic combat model:
        // P(win) = PlayerStrength /
        //          (PlayerStrength + EnemyStrength)
        //
        // Strength may include HP, upgrades, cannons
        // and allied ships.

        float winChance = friendly
            ? economy.friendlyWinChance
            : economy.aggressiveWinChance;

        bool won = winChance >= 1f ||
                   (winChance > 0f && Random.value < winChance);

        // In M4, winning an encounter completes the stage.
        int stageReward = won
            ? economy.coinsPerLevel
            : 0;

        // Befriending alone does not award coins.
        int plunderReward = won && !friendly
            ? economy.plunderReward
            : 0;

        int reward = stageReward + plunderReward;

        Services.Save.SetInt(
            CoinCounter.Key,
            Services.Save.GetInt(CoinCounter.Key) + reward);

        // Refresh the current Level HUD.(.? is for null check operator)
        _coinCounter?.Show(); // exists? show

        string choice = friendly ? "Friendly" : "Aggressive";
        string result = won ? "WIN" : "LOSE";

        string resultColor = won ? "#8BDAA0" : "#FF7777";

        _resultText.text =
            $"{choice} encounter\n" +
            $"<size=150%><color={resultColor}><b>{result}</b></color></size>\n\n" +
            $"Stage reward: {stageReward}\n" +
            $"Plunder reward: {plunderReward}\n" +
            $"<color=#FFCF77><b>+{reward} coins</b></color>";

        if (_encounterPanel != null)
            _encounterPanel.SetActive(false);

        if (_resultPanel != null)
            _resultPanel.SetActive(true);
    }

    /// <summary>Called by BTN_ReturnHome after the outcome has been shown.</summary>
    public void ReturnHome()
    {
        if (!_resolved) return;
        SceneManager.LoadScene("Home");
    }
}