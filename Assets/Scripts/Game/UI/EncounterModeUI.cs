using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the encounter toggle appearance
/// according to its current state.
/// </summary>
[RequireComponent(typeof(Toggle))]
public sealed class EncounterModeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text label;

    [Header("Friendly Mode")]
    [SerializeField] private string friendlyText = "Friendly";
    [SerializeField] private Color friendlyColor =
        new Color32(40, 103, 66, 255);

    [Header("Aggressive Mode")]
    [SerializeField] private string aggressiveText = "Aggressive";
    [SerializeField] private Color aggressiveColor =
        new Color32(139, 52, 49, 255);
    
    [SerializeField] private Image modeIcon;
    [SerializeField] private Sprite friendlyIcon;
    [SerializeField] private Sprite aggressiveIcon;
    
    [Header("Encounter Action")]
    [SerializeField] private TMP_Text encounterActionLabel;

    [SerializeField] private string friendlyActionText = "Befriend";
    [SerializeField] private string aggressiveActionText = "Attack";
    
    private Toggle _toggle;

    /// <summary>
    /// find toggle component
    /// </summary>
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }
    
    // sign to event + update appearance
    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(Refresh);
        Refresh(_toggle.isOn);
    }
    
    // cancel sign to event + update appearance
    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(Refresh);
    }

    /// <summary>
    /// changes text according to state
    /// </summary>
    /// <param name="isFriendly"></param>
    private void Refresh(bool isFriendly)
    {
        if (label != null)
            label.text = isFriendly
                ? friendlyText
                : aggressiveText;

        if (background != null)
            background.color = isFriendly
                ? friendlyColor
                : aggressiveColor;
        
        if (modeIcon != null)
        {
            modeIcon.sprite = isFriendly
                ? friendlyIcon
                : aggressiveIcon;

            modeIcon.enabled = modeIcon.sprite != null;
        }
        
        if (encounterActionLabel != null)
        {
            encounterActionLabel.text = isFriendly
                ? friendlyActionText
                : aggressiveActionText;
        }
    }
}