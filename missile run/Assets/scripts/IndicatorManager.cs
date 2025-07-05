using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] public Camera cam;
    [SerializeField] public RectTransform canvasRect;
    [SerializeField] public GameObject indicatorPrefab;

    private class IndicatorInfo
    {
        public GameObject target;
        public GameObject indicatorGO;
        public Image image;
        public TextMeshProUGUI distanceText;
        public bool showDistance; 
    }


    private Dictionary<GameObject, IndicatorInfo> activeIndicators = new Dictionary<GameObject, IndicatorInfo>();

    void Update()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var pair in activeIndicators)
        {
            GameObject target = pair.Key;
            IndicatorInfo info = pair.Value;

            if (target == null || info.indicatorGO == null)
            {
                if (info.indicatorGO != null)
                    Destroy(info.indicatorGO);
                toRemove.Add(target);
                continue;
            }

            Vector3 screenPos = cam.WorldToScreenPoint(target.transform.position);

            bool isOffscreen = screenPos.z < 0 ||
                               screenPos.x < 0 || screenPos.x > Screen.width ||
                               screenPos.y < 0 || screenPos.y > Screen.height;

            info.indicatorGO.SetActive(isOffscreen);

            if (isOffscreen)
            {
                // Clamp position
                screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
                screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);
                info.indicatorGO.transform.position = screenPos;

                // ✅ Show distance ONLY if allowed
                if (info.showDistance && info.distanceText != null && GameObject.FindGameObjectWithTag("Player") != null)
                {
                    float worldDist = Vector2.Distance(
                        GameObject.FindGameObjectWithTag("Player").transform.position,
                        target.transform.position
                    );
                    info.distanceText.text = Mathf.RoundToInt(worldDist) + "m";

                    // ✅ Always position distance text ABOVE the indicator
                    Vector3 offset = new Vector3(0f, -40f, 0f); // Adjust Y as needed
                    info.distanceText.rectTransform.position = screenPos + offset;
                }
                else if (info.distanceText != null)
                {
                    // ✅ Hide text for non-SP indicators
                    info.distanceText.text = "";
                }

                // ✅ Disable shaking effect — keep constant scale
                info.indicatorGO.transform.localScale = Vector3.one;
            }
        }

        foreach (var target in toRemove)
        {
            activeIndicators.Remove(target);
        }
    }




    public void AddTarget(GameObject target, bool showDistance = false) // ✅ New optional flag
    {
        if (activeIndicators.ContainsKey(target)) return;

        GameObject indicator = Instantiate(indicatorPrefab, canvasRect);
        indicator.SetActive(false);

        // Get custom sprite from SPInfo
        Sprite icon = null;
        SPInfo info = target.GetComponent<SPInfo>();
        if (info != null) icon = info.indicatorSprite;

        var image = indicator.GetComponentInChildren<Image>();
        var text = indicator.GetComponentInChildren<TextMeshProUGUI>();

        if (icon != null && image != null)
            image.sprite = icon;

        IndicatorInfo i = new IndicatorInfo
        {
            target = target,
            indicatorGO = indicator,
            image = image,
            distanceText = text,
            showDistance = showDistance // ✅ Assign here
        };

        activeIndicators.Add(target, i);
    }


    public void RemoveTarget(GameObject target)
    {
        if (activeIndicators.TryGetValue(target, out var info))
        {
            Destroy(info.indicatorGO);
            activeIndicators.Remove(target);
        }
    }
}
