using UnityEngine;

/// <summary>
/// Connects ShipWheelInput (what the player does) to ShipHapticsEngine
/// (what the phone feels). Neither class needs to know about ship physics.
///
/// Reads the REAL accumulated wheel angle, including rotations beyond 360°.
/// Does not vibrate when auto-centering on release.
/// </summary>
[RequireComponent(typeof(ShipWheelInput))]
public class ShipWheelHaptics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShipWheelInput wheelInput;
    [SerializeField] private ShipHapticsEngine haptics;

    [Header("Mechanical Wheel Ticks")]
    [Tooltip("Degrees of manual wheel travel between light clicks.")]
    [SerializeField, Min(1f)] private float degreesPerTick = 35f;

    [Tooltip("At most one light click per this many seconds.")]
    [SerializeField, Min(0.02f)] private float tickCooldown = 0.09f;

    [Header("End Stop")]
    [Tooltip("How close to Max Angle counts as the end of the wheel.")]
    [SerializeField, Min(0f)] private float endTolerance = 3f;

    [Tooltip("How far to turn back before another end-stop hit can vibrate.")]
    [SerializeField, Min(1f)] private float endResetDistance = 40f;

    [Header("Optional Direction Feedback")]
    [SerializeField] private bool vibrateOnDirectionChange = false;
    [SerializeField, Min(0.5f)] private float directionNoiseThreshold = 2f;
    [SerializeField, Min(0.05f)] private float directionCooldown = 0.2f;

    private float previousAngle;
    private float lastTickAngle;
    private float lastTickTime = -100f;
    private float lastDirectionTime = -100f;
    private int lastDirection;
    private bool wasDragging;
    private bool endStopArmed = true;

    private void Awake()
    {
        if (wheelInput == null)
            wheelInput = GetComponent<ShipWheelInput>();
    }

    /// <summary>
    /// LateUpdate samples the wheel AFTER its pointer input changes the angle.
    /// Time.unscaledTime is used for cooldowns (works even if timeScale changes).
    /// </summary>
    private void LateUpdate()
    {
        if (wheelInput == null || haptics == null) return;

        float angle = wheelInput.CurrentAngle;

        // Never buzz during auto-center. Releasing is silent.
        if (!wheelInput.IsDragging)
        {
            wasDragging = false;
            previousAngle = angle;
            return;
        }

        // Begin a new gesture without an initial sudden click.
        if (!wasDragging)
        {
            wasDragging = true;
            previousAngle = angle;
            lastTickAngle = angle;
            lastDirection = 0;
            endStopArmed = Mathf.Abs(angle) < wheelInput.MaxAngle - endTolerance;
            return;
        }

        float delta = angle - previousAngle;
        previousAngle = angle;
        if (Mathf.Abs(delta) < 0.001f) return;

        // Rearm only after the wheel moves meaningfully away from the limit.
        if (Mathf.Abs(angle) <= wheelInput.MaxAngle - endResetDistance)
            endStopArmed = true;

        // Priority: hitting a hard stop should not also trigger a soft tick.
        if (endStopArmed &&
            Mathf.Abs(angle) >= wheelInput.MaxAngle - endTolerance)
        {
            haptics.PlayWheelEndStop();
            endStopArmed = false;
            lastTickAngle = angle;
            return;
        }

        // Optional direction feedback. The deadzone reduces tiny jitter.
        if (Mathf.Abs(delta) >= directionNoiseThreshold)
        {
            int direction = delta > 0f ? 1 : -1;
            if (vibrateOnDirectionChange &&
                lastDirection != 0 && direction != lastDirection &&
                Time.unscaledTime - lastDirectionTime >= directionCooldown)
            {
                haptics.PlayDirectionChange();
                lastDirectionTime = Time.unscaledTime;
                lastTickAngle = angle;
                lastDirection = direction;
                return;
            }
            lastDirection = direction;
        }

        // Click after enough actual wheel travel, with a minimum time gap.
        if (Mathf.Abs(angle - lastTickAngle) >= degreesPerTick &&
            Time.unscaledTime - lastTickTime >= tickCooldown)
        {
            haptics.PlayWheelTick();
            lastTickAngle = angle;
            lastTickTime = Time.unscaledTime;
        }
    }
}
