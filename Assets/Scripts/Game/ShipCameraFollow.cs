using UnityEngine;

/// <summary>
/// Smoothly follows a world-space ship while looking slightly ahead of its movement.
/// Camera lag keeps the ship from feeling permanently glued to the screen center.
/// The UI Canvas remains screen-space and does not follow the camera.
/// </summary>
[RequireComponent(typeof(Camera))]
public sealed class ShipCameraFollow : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Transform of the world-space PlayerShip, not the UI Image.")]
    [SerializeField] private Transform _target;

    [Tooltip("Optional Rigidbody2D used to look in the direction of travel.")]
    [SerializeField] private Rigidbody2D _targetBody;

    [Header("Camera Feel")]
    [Tooltip("How long the camera takes to catch up. Higher values add more lag.")]
    [SerializeField, Min(0.01f)] private float _smoothTime = 0.3f;

    [Tooltip("Seconds of velocity projected ahead for camera framing.")]
    [SerializeField, Min(0f)] private float _lookAheadSeconds = 0.35f;

    [Tooltip("Maximum distance of camera look-ahead in world units.")]
    [SerializeField, Min(0f)] private float _maxLookAhead = 1.5f;

    private Vector3 _cameraVelocity;
    private float _cameraZ;

    /// <summary>Remember the camera's original depth to keep a correct 2D view.</summary>
    private void Awake()
    {
        _cameraZ = transform.position.z;
    }

    /// <summary>
    /// Follow after the ship moves. The target is not rigidly locked to center;
    /// velocity look-ahead and smoothing create subtle relative screen motion.
    /// </summary>
    private void LateUpdate()
    {
        if (_target == null) return;

        Vector2 lookAhead = _targetBody != null
            ? Vector2.ClampMagnitude(_targetBody.linearVelocity * _lookAheadSeconds, _maxLookAhead)
            : Vector2.zero;

        Vector3 desired = new Vector3(
            _target.position.x + lookAhead.x,
            _target.position.y + lookAhead.y,
            _cameraZ);

        transform.position = Vector3.SmoothDamp(
            transform.position, desired, ref _cameraVelocity, _smoothTime);
    }
}
