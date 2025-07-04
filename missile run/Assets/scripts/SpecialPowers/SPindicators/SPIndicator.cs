using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SPIndicator : MonoBehaviour
{
    public Transform target;
    public RectTransform canvasRect;
    public Image indicatorImage;
    public TextMeshProUGUI distanceText;

    private Transform player;
    private RectTransform rect;
    public Camera cam;

    private float screenPadding = 50f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        cam = Camera.main;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (target == null || player == null || cam == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        // Check if SP is visible on-screen
        bool isVisible = screenPos.z > 0 &&
                         screenPos.x >= 0 && screenPos.x <= Screen.width &&
                         screenPos.y >= 0 && screenPos.y <= Screen.height;

        // Toggle visibility
        gameObject.SetActive(!isVisible);

        if (!isVisible)
        {
            // Clamp indicator within screen bounds
            float halfWidth = rect.rect.width / 2f;
            float halfHeight = rect.rect.height / 2f;

            screenPos.x = Mathf.Clamp(screenPos.x, screenPadding + halfWidth, Screen.width - screenPadding - halfWidth);
            screenPos.y = Mathf.Clamp(screenPos.y, screenPadding + halfHeight, Screen.height - screenPadding - halfHeight);

            rect.position = screenPos;

            // Update distance
            float dist = Vector2.Distance(player.position, target.position);
            distanceText.text = Mathf.RoundToInt(dist) + "m";
        }
    }
}
