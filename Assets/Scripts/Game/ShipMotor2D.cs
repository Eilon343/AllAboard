using UnityEngine;

/// <summary>
/// Moves the player ship through a 2D world with arcade sailing physics.
/// ShipWheelInput provides steering; this component manages rudder lag,
/// hull turning inertia, forward thrust and water resistance.
/// All velocities are gameplay units, not measured marine physics.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public sealed class ShipMotor2D : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("UI ShipWheelInput. Read its Steering value; never move the wheel here.")]
    [SerializeField] private ShipWheelInput _wheel;

    [Header("Steering Feel")]
    [Tooltip("Nonlinear response. Larger values make small wheel turns useful.")]
    [SerializeField, Range(1f, 15f)] private float _steeringCurve = 9f;

    [Tooltip("Wheel values inside this range are treated as neutral.")]
    [SerializeField, Range(0f, 0.15f)] private float _deadZone = 0.02f;

    [Tooltip("How quickly the rudder responds to wheel input, per second.")]
    [SerializeField, Min(0.1f)] private float _rudderResponse = 6f;

    [Tooltip("How quickly the hull's turn rate follows the rudder, per second.")]
    [SerializeField, Min(0.1f)] private float _yawResponse = 5f;

    [Tooltip("Maximum turning speed at full sailing speed, degrees per second.")]
    [SerializeField, Min(0f)] private float _turnSpeed = 95f;

    [Tooltip("Fraction of turning authority while nearly stopped.")]
    [SerializeField, Range(0f, 1f)] private float _turnAtRest = 0.35f;

    [Header("Sailing Feel")]
    [Tooltip("Forward acceleration in world units per second squared.")]
    [SerializeField, Min(0f)] private float _acceleration = 9f;

    [Tooltip("Maximum speed in world units per second.")]
    [SerializeField, Min(0.1f)] private float _maxSpeed = 8f;

    [Tooltip("Water resistance applied to world velocity, per second.")]
    [SerializeField, Min(0f)] private float _waterDrag = 1.35f;

    [Tooltip("Extra resistance to sideways motion. Lower = more drifting.")]
    [SerializeField, Min(0f)] private float _sidewaysGrip = 1f;

    private Rigidbody2D _body;
    private float _rudder;
    private float _yawRate;

    /// <summary>Cache the physics body used for all ship motion.</summary>
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Convert steering to a nonlinear rudder target, then update heading and
    /// velocity on the fixed physics clock. Velocity persists through turns,
    /// giving the ship momentum without needing complex water simulation.
    /// </summary>
    private void FixedUpdate()
    {
        if (_wheel == null) return;

        float dt = Time.fixedDeltaTime;
        float targetRudder = GetRudderTarget(_wheel.Steering);
        float rudderBlend = 1f - Mathf.Exp(-_rudderResponse * dt);
        _rudder = Mathf.Lerp(_rudder, targetRudder, rudderBlend);

        // The hull does not instantly reach its requested turning speed.
        float speedRatio = Mathf.Clamp01(_body.linearVelocity.magnitude / _maxSpeed);
        float turnAuthority = Mathf.Lerp(_turnAtRest, 1f, speedRatio);
        float targetYawRate = _rudder * _turnSpeed * turnAuthority;
        float yawBlend = 1f - Mathf.Exp(-_yawResponse * dt);
        _yawRate = Mathf.Lerp(_yawRate, targetYawRate, yawBlend);

        // Positive wheel steering should rotate the bow clockwise (right).
        float nextHeading = _body.rotation - _yawRate * dt;
        _body.MoveRotation(nextHeading);

        // Compute thrust using the same heading selected for this physics step.
        Vector2 forward = Quaternion.Euler(0f, 0f, nextHeading) * Vector2.up;
        Vector2 right = new Vector2(forward.y, -forward.x);
        Vector2 velocity = _body.linearVelocity;

        // Drag acts in world space, then engine thrust accelerates forward.
        velocity *= Mathf.Exp(-_waterDrag * dt);
        velocity += forward * (_acceleration * dt);

        // Water resists sideways sliding while retaining some drifting.
        float lateralSpeed = Vector2.Dot(velocity, right);
        float lateralBlend = 1f - Mathf.Exp(-_sidewaysGrip * dt);
        velocity -= right * (lateralSpeed * lateralBlend);

        _body.linearVelocity = Vector2.ClampMagnitude(velocity, _maxSpeed);
    }

    /// <summary>
    /// Match the Mechanics Lab steering curve: short wheel gestures make a
    /// noticeable rudder change, without a sudden hard clamp at 20% input.
    /// </summary>
    private float GetRudderTarget(float wheelValue)
    {
        float magnitude = Mathf.Abs(wheelValue);
        if (magnitude <= _deadZone) return 0f;

        float denominator = 1f - Mathf.Exp(-_steeringCurve);
        float response = (1f - Mathf.Exp(-_steeringCurve * magnitude)) / denominator;
        return Mathf.Sign(wheelValue) * response;
    }
}
