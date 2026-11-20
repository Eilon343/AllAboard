using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Reads circular mouse/touch dragging on the UI ship wheel.
/// Rotates the wheel image and exposes a normalized steering value (-1 to +1).
///
/// IMPORTANT: This class handles INPUT only. It does not move or rotate the ship.
/// A future ShipMotor2D will read the public Steering property.
/// </summary>
// Unity ensures this GameObject has a RectTransform (instead of a regular Transform).
[RequireComponent(typeof(RectTransform))]
public class ShipWheelInput : MonoBehaviour,
    IPointerDownHandler, // Called when a finger/mouse button presses the UI element.
    IDragHandler,        // Called while that pointer drags across the UI.
    IPointerUpHandler    // Called when the pointer is released.
{
    // [SerializeField] shows a PRIVATE field in Unity's Inspector.
    // The field is still private to other C# scripts.
    // [Header] and [Tooltip] only organize/explain fields in the Inspector.
    [Header("Wheel")]
    [Tooltip("UI RectTransform to rotate; defaults to this object's RectTransform.")]
    [SerializeField] private RectTransform wheel;

    [Tooltip("Maximum wheel rotation in degrees in either direction (tested preset: 1080 = 3 full turns).")]
    [SerializeField] private float maxAngle = 1080f;

    [Tooltip("Multiplier for finger movement. 1 means no extra scaling.")]
    [SerializeField] private float sensitivity = 1f;

    [Header("Return to Center")]
    [Tooltip("When enabled, release the wheel and it gradually returns to neutral.")]
    [SerializeField] private bool autoCenter = true;

    [Tooltip("How quickly the wheel returns to 0, measured in degrees per second.")]
    [SerializeField] private float returnSpeed = 480f;

    [Header("Debug (calculated by script)")]
    [Tooltip("Current steering output: -1 = left, 0 = straight, +1 = right. Do not set manually.")]
    [SerializeField, Range(-1f, 1f)]
    private float steering;

    /// <summary>
    /// Read-only access for other scripts.
    /// The '=>' syntax returns the private steering value (equivalent to a getter).
    /// </summary>
    public float Steering => steering;

    /// <summary>Accumulated visual wheel angle in degrees (can exceed 360).</summary>
    public float CurrentAngle => currentAngle;

    /// <summary>Maximum permitted wheel angle on each side; for haptic limit detection.</summary>
    public float MaxAngle => maxAngle;

    /// <summary>True only while the player is actively dragging the wheel.</summary>
    public bool IsDragging => dragging;

    // Internal state. These values don't need to appear in the Inspector.
    private float currentAngle;         // Wheel's accumulated angle in degrees.
    private float previousPointerAngle; // Pointer angle from the previous event.
    private bool dragging;              // True while we're actively steering.

    /// <summary>
    /// Unity calls Awake once when this component initializes.
    /// If we didn't assign a wheel reference in the Inspector,
    /// use the RectTransform on this same GameObject.
    /// </summary>
    private void Awake()
    {
        if (wheel == null)
            wheel = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Begin steering and remember the pointer's starting angle.
    /// This avoids an instant jump when the player first touches the wheel.
    /// </summary>
    /// <param name="data">Unity pointer data (position and event camera).</param>
    public void OnPointerDown(PointerEventData data)
    {
        dragging = true;
        previousPointerAngle = GetPointerAngle(data);
    }

    /// <summary>
    /// Read the pointer's circular movement and rotate the wheel accordingly.
    /// We accumulate ANGLE CHANGES, instead of snapping to the pointer angle.
    /// </summary>
    /// <param name="data">Current mouse/touch pointer data from EventSystem.</param>
    public void OnDrag(PointerEventData data)
    {
        if (!dragging) return;

        float pointerAngle = GetPointerAngle(data);

        // Shortest signed difference between angles, in degrees.
        // DeltaAngle handles wraparound (for example, +179 to -179).
        float delta = Mathf.DeltaAngle(
            previousPointerAngle,
            pointerAngle
        );

        // Accumulate the turn, apply sensitivity, and limit the wheel
        // to the range [-maxAngle, +maxAngle].
        currentAngle = Mathf.Clamp(
            currentAngle + delta * sensitivity,
            -maxAngle,
            maxAngle
        );

        // Remember this event for the next drag movement.
        previousPointerAngle = pointerAngle;

        ApplySteering();
    }

    /// <summary>
    /// Stop dragging when the player releases the pointer.
    /// If autoCenter is enabled, Update() handles the gradual return.
    /// </summary>
    /// <param name="data">Pointer data for the release event.</param>
    public void OnPointerUp(PointerEventData data)
    {
        dragging = false;
    }

    /// <summary>
    /// Unity calls Update every frame.
    /// Only auto-center when the player is NOT dragging.
    /// </summary>
    private void Update()
    {
        if (dragging || !autoCenter) return;

        // MoveTowards changes the angle by at most returnSpeed * deltaTime.
        // deltaTime makes the return rate roughly independent of frame rate.
        currentAngle = Mathf.MoveTowards(
            currentAngle,
            0f,
            returnSpeed * Time.deltaTime
        );

        ApplySteering();
    }

    /// <summary>
    /// Calculate the mouse/finger angle around the wheel's center.
    /// </summary>
    /// <param name="data">Pointer position in screen pixels + event camera.</param>
    /// <returns>Pointer direction in degrees, relative to the wheel center.</returns>
    private float GetPointerAngle(PointerEventData data)
    {
        // Convert the center of the UI wheel from local/world space
        // to a screen-space point, so it can be compared with data.position.
        Vector2 center =
            RectTransformUtility.WorldToScreenPoint(
                data.pressEventCamera,
                wheel.TransformPoint(wheel.rect.center)
            );

        Vector2 direction = data.position - center;

        // Atan2(y, x) returns radians. Rad2Deg converts them to degrees.
        return Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Display the accumulated rotation and calculate the steering output.
    /// Calling it from both OnDrag and Update keeps the visual and value in sync.
    /// </summary>
    private void ApplySteering()
    {
        // UI elements rotate around Z in a 2D Canvas.
        wheel.localRotation =
            Quaternion.Euler(0f, 0f, currentAngle);

        // Unity's positive Z rotation is counterclockwise.
        // Negating it makes CLOCKWISE steering positive (right turn).
        // Example: currentAngle = -540, maxAngle = 1080 -> Steering = +0.5.
        steering = Mathf.Clamp(
            -currentAngle / maxAngle,
            -1f,
            1f
        );
    }
}
