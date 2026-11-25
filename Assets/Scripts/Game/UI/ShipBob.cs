using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ShipBob : MonoBehaviour
{
    [SerializeField] float bobHeight = 20f; //how far up/down
    [SerializeField] float bobSpeed = 1f;   //how fast it goes
    [SerializeField] float tiltAngle = 3f; //how far it rocks left/right
    [SerializeField] float tiltSpeed = 1f;   //how fast it rocks
    RectTransform rectTransform;
    Vector2 startPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition; //remember the starting position
    }

    void Update()
    {
        float t = Time.time;
        rectTransform.anchoredPosition = startPos + Vector2.up * (Mathf.Sin(t * bobSpeed) * bobHeight);
        rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(t * tiltSpeed) * tiltAngle);
    }
}
