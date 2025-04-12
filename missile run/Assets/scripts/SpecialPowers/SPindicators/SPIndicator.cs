using UnityEngine;
using UnityEngine.UI;

public class SPIndicator : MonoBehaviour
{
    public Transform target; // The SP object
    public RectTransform canvasRect;
    public Image indicatorImage;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Clean up if target is gone
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // Check if off-screen
        if (screenPos.z > 0 && (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height))
        {
            indicatorImage.enabled = true;

            // Clamp to screen edges
            screenPos.x = Mathf.Clamp(screenPos.x, 50f, Screen.width - 50f);
            screenPos.y = Mathf.Clamp(screenPos.y, 50f, Screen.height - 50f);

            transform.position = screenPos;
        }
        else
        {
            // On screen, hide indicator
            indicatorImage.enabled = false;
        }
    }
}
