using UnityEngine;

public sealed class SettingsService : ISettingsService
{
    private const string SoundKey = "settings.soundOn";

    private readonly ISaveService _save;
    private bool _soundOn;

    public SettingsService(ISaveService save)
    {
        _save = save;   // hold ref to our save settings mechanism

        // 1 = sound enabled (default).
        _soundOn = _save.GetInt(SoundKey, 1) == 1;

        ApplySound();
    }

    public bool SoundOn
    {
        get => _soundOn;    // let screens read&change the definition

        set
        {
            if (_soundOn == value)
                return;

            _soundOn = value;

            _save.SetInt(SoundKey, value ? 1 : 0);

            ApplySound();
        }
    }

    private void ApplySound()
    {
        AudioListener.volume = _soundOn ? 1f : 0f;
    }
}