using System;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif

/// <summary>
/// A small, reusable gateway for the game's haptic feedback.
///
/// Other scripts ask for a semantic effect (wheel tick, direction change,
/// wheel end-stop). This class talks to Android. It NEVER handles steering.
///
/// Android's performHapticFeedback respects the phone's touch-feedback
/// setting and does not require the VIBRATE permission.
/// In the Editor the vibration is intentionally a no-op.
/// </summary>
public class ShipHapticsEngine : MonoBehaviour
{
    // These values are Android HapticFeedbackConstants (available on API 29).
    // Choosing a different preset changes the type of feedback requested,
    // NOT an exact vibration strength. The phone decides the actual feeling.
    public enum HapticPreset
    {
        LongPress = 0,
        VirtualKey = 1,
        KeyboardTap = 3,
        ClockTick = 4,
        ContextClick = 6
    }

    [Header("Haptic Preferences")]
    [SerializeField] private bool enableHaptics = true;

    [Header("Android Presets - experiment on your phone")]
    [Tooltip("Subtle mechanical click while turning the wheel.")]
    [SerializeField] private HapticPreset wheelTick = HapticPreset.ClockTick;

    [Tooltip("Feedback when reversing steering direction.")]
    [SerializeField] private HapticPreset directionChange = HapticPreset.KeyboardTap;

    [Tooltip("More noticeable feedback when reaching the wheel's limit.")]
    [SerializeField] private HapticPreset wheelEndStop = HapticPreset.LongPress;

    [Header("Testing")]
    [Tooltip("Logs events in Editor, but cannot vibrate a PC.")]
    [SerializeField] private bool logEventsInEditor = false;

    public bool HapticsEnabled => enableHaptics;

    /// <summary>Hook this to a future Settings toggle (bool).</summary>
    public void SetHapticsEnabled(bool value) => enableHaptics = value;

    /// <summary>Mechanical click during active steering.</summary>
    public void PlayWheelTick() => Play(wheelTick, "Wheel tick");

    /// <summary>Optional feedback when the player reverses direction.</summary>
    public void PlayDirectionChange() => Play(directionChange, "Direction changed");

    /// <summary>One larger feedback event at the wheel's rotation limit.</summary>
    public void PlayWheelEndStop() => Play(wheelEndStop, "End stop");

    /// <summary>
    /// Send one effect to Android on its UI thread.
    /// No continuous vibration, no custom manifest, no editor-only hacks.
    /// </summary>
    private void Play(HapticPreset preset, string label)
    {
        if (!enableHaptics) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            // Unity 6 API. The activity belongs to the Unity runtime:
            // do not Dispose() the activity object.
            AndroidJavaObject activity = AndroidApplication.currentActivity;
            if (activity == null) return;

            int effectId = (int)preset;

            // Android View methods should run on the Android UI thread.
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                try
                {
                    using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow"))
                    using (AndroidJavaObject view = window.Call<AndroidJavaObject>("getDecorView"))
                    {
                        // We deliberately do NOT ignore the user's haptic settings.
                        view.Call<bool>("performHapticFeedback", effectId);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"ShipHapticsEngine: Android haptic failed: {e.Message}");
                }
            }));
        }
        catch (Exception e)
        {
            Debug.LogWarning($"ShipHapticsEngine: Could not request haptic: {e.Message}");
        }
#elif UNITY_EDITOR
        if (logEventsInEditor)
            Debug.Log($"[Ship Haptics] {label} ({preset}) - Editor preview only");
#endif
    }
}
