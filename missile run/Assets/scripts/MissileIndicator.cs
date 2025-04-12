using UnityEngine;
using UnityEngine.UI;

public class MissileIndicator : MonoBehaviour
{
    public Transform missile; // The missile to track
    public Camera cam; // Main camera
    public RectTransform indicator; // UI Image arrow

    void Update()
    {
        if (missile == null || cam == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(missile.position);

        bool isOffScreen = screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        indicator.gameObject.SetActive(isOffScreen);

        if (isOffScreen)
        {
            // Clamp position to screen bounds with padding
            screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
            screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);
            screenPos.z = 0;

            indicator.position = screenPos;

            // Point arrow towards the missile
            Vector3 dir = missile.position - cam.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicator.rotation = Quaternion.Euler(0, 0, angle );
        }
    }
}
