
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ShipWheelInput : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("Wheel")]
    [SerializeField] private RectTransform wheel;
    [SerializeField] private float maxAngle = 120f;
    [SerializeField] private float sensitivity = 1f;

    [Header("Return to Center")]
    [SerializeField] private bool autoCenter = true;
    [SerializeField] private float returnSpeed = 240f;

    [Header("Debug")]
    [SerializeField, Range(-1f, 1f)]
    private float steering;

    public float Steering => steering;

    private float currentAngle;
    private float previousPointerAngle;
    private bool dragging;

    private void Awake()
    {
        if (wheel == null)
            wheel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        dragging = true;
        previousPointerAngle = GetPointerAngle(data);
    }

    public void OnDrag(PointerEventData data)
    {
        if (!dragging) return;

        float pointerAngle = GetPointerAngle(data);

        float delta = Mathf.DeltaAngle(
            previousPointerAngle,
            pointerAngle
        );

        currentAngle = Mathf.Clamp(
            currentAngle + delta * sensitivity,
            -maxAngle,
            maxAngle
        );

        previousPointerAngle = pointerAngle;

        ApplySteering();
    }

    public void OnPointerUp(PointerEventData data)
    {
        dragging = false;
    }

    private void Update()
    {
        if (dragging || !autoCenter) return;

        currentAngle = Mathf.MoveTowards(
            currentAngle,
            0f,
            returnSpeed * Time.deltaTime
        );

        ApplySteering();
    }

    private float GetPointerAngle(PointerEventData data)
    {
        Vector2 center =
            RectTransformUtility.WorldToScreenPoint(
                data.pressEventCamera,
                wheel.TransformPoint(wheel.rect.center)
            );

        Vector2 direction = data.position - center;

        return Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;
    }

    private void ApplySteering()
    {
        wheel.localRotation =
            Quaternion.Euler(0f, 0f, currentAngle);

        // Clockwise = positive/right steering.
        steering = Mathf.Clamp(
            -currentAngle / maxAngle,
            -1f,
            1f
        );
    }
}
