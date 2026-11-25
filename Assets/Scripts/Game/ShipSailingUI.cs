using UnityEngine;

/// <summary>
/// Temporary sailing prototype for a ship represented by a UI RectTransform.
/// It is restricted to its parent panel, so it cannot simulate an endless world.
/// Keep as a reference, but disable it when ShipMotor2D takes over.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public sealed class ShipSailingUI : MonoBehaviour
{
    [Header("Steering")]
    [Tooltip("The existing UI steering wheel; its Steering value is between -1 and 1.")]
    [SerializeField] private ShipWheelInput _wheel;

    [Tooltip("Amplifies small wheel movements before clamping the rudder to -1..1.")]
    [SerializeField, Min(0f)] private float _steeringGain = 4f;

    [Tooltip("Maximum rudder change per second; smooths sudden steering input.")]
    [SerializeField, Min(0f)] private float _steeringResponse = 4f;

    [Tooltip("Maximum ship rotation speed, measured in degrees per second.")]
    [SerializeField, Min(0f)] private float _turnSpeed = 130f;

    [Header("Movement")]
    [Tooltip("Target forward speed, in Canvas units per second.")]
    [SerializeField, Min(0f)] private float _cruiseSpeed = 150f;

    [Tooltip("Speed added each second until the ship reaches cruise speed.")]
    [SerializeField, Min(0f)] private float _acceleration = 180f;

    // Cached UI transform and runtime motion state; not editable in Inspector.
    private RectTransform _ship;
    private float _speed;
    private float _rudder;
    private float _heading;

    /// <summary>Cache the ship RectTransform and preserve its initial heading.</summary>
    private void Awake()
    {
        _ship = GetComponent<RectTransform>();
        _heading = _ship.localEulerAngles.z;
    }

    /// <summary>
    /// Read the latest wheel input, apply rudder smoothing, accelerate forward,
    /// rotate the ship and keep the temporary UI prototype inside its panel.
    /// </summary>
    private void Update()
    {
        if (_wheel == null) return;

        float dt = Time.deltaTime;
        float targetRudder = Mathf.Clamp(_wheel.Steering * _steeringGain, -1f, 1f);
        _rudder = Mathf.MoveTowards(_rudder, targetRudder, _steeringResponse * dt);
        _speed = Mathf.MoveTowards(_speed, _cruiseSpeed, _acceleration * dt);

        // Clockwise wheel input should turn the bow to the right.
        _heading -= _rudder * _turnSpeed * dt;
        _ship.localRotation = Quaternion.Euler(0f, 0f, _heading);

        float angle = _heading * Mathf.Deg2Rad;
        Vector2 forward = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
        _ship.anchoredPosition += forward * _speed * dt;

        KeepOnScreen();
    }

    /// <summary>
    /// Clamp the UI ship to its parent RectTransform. This is deliberately
    /// temporary; a true world-space ship must NOT use this boundary.
    /// </summary>
    private void KeepOnScreen()
    {
        RectTransform parent = _ship.parent as RectTransform;
        if (parent == null) return;

        Vector2 position = _ship.anchoredPosition;
        float halfWidth = Mathf.Max(0f, (parent.rect.width - _ship.rect.width) / 2f);
        float halfHeight = Mathf.Max(0f, (parent.rect.height - _ship.rect.height) / 2f);

        position.x = Mathf.Clamp(position.x, -halfWidth, halfWidth);
        position.y = Mathf.Clamp(position.y, -halfHeight, halfHeight);
        _ship.anchoredPosition = position;
    }
}
