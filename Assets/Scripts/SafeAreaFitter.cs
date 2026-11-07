using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    Rect lastSafeArea;

    void Update()
    {
        Rect safe = Screen.safeArea;
        if (safe == lastSafeArea) return;
        lastSafeArea = safe;

        var panel = GetComponent<RectTransform>();
        panel.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
        panel.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;
    }
}