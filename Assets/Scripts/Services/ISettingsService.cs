
/// <summary>
/// Defines the contract for accessing and changing game settings.
/// </summary>
public interface ISettingsService
{
    bool SoundOn { get; set; } 
    
    
    // Future additions:
    // bool HapticsOn { get; set; }
    // float SteeringSensitivity { get; set; }
    // bool LeftHandedMode { get; set; }
}
