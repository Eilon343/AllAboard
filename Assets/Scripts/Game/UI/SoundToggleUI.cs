using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public sealed class SoundToggleUI : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        if (Services.Settings == null)
        {
            Debug.LogError(
                "SettingsService is not initialized.", this);
            return;
        }

        bool soundOn = Services.Settings.SoundOn;

        _toggle.SetIsOnWithoutNotify(soundOn);
        UpdateIcon(soundOn);

        _toggle.onValueChanged.AddListener(OnSoundChanged);
    }

    private void OnDisable()
    {
        if (_toggle != null)
            _toggle.onValueChanged.RemoveListener(OnSoundChanged);
    }

    private void OnSoundChanged(bool soundOn)
    {
        if (Services.Settings == null)
            return;

        Services.Settings.SoundOn = soundOn;
        UpdateIcon(soundOn);
    }

    private void UpdateIcon(bool soundOn)
    {
        if (_icon == null)
            return;

        _icon.sprite = soundOn
            ? _soundOnSprite
            : _soundOffSprite;
    }
}