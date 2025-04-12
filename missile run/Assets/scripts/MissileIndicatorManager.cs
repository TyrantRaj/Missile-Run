using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileIndicatorManager : MonoBehaviour
{
    [SerializeField] public Camera cam;
    [SerializeField] public RectTransform canvasRect;
    [SerializeField] public GameObject indicatorPrefab;

    private Dictionary<GameObject, GameObject> missileIndicators = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var pair in missileIndicators)
        {
            GameObject missile = pair.Key;
            GameObject indicator = pair.Value;

            if (missile == null)
            {
                Destroy(indicator);
                toRemove.Add(missile);
                continue;
            }

            Vector3 screenPos = cam.WorldToScreenPoint(missile.transform.position);

            // Check if it's off-screen
            bool isOffscreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0;

            indicator.SetActive(isOffscreen);

            if (isOffscreen)
            {
                screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
                screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

                indicator.transform.position = screenPos;

                // Heartbeat effect based on distance from screen center
                float distanceToCenter = Vector2.Distance(screenPos, new Vector2(Screen.width / 2f, Screen.height / 2f));
                float maxDistance = Mathf.Sqrt(Mathf.Pow(Screen.width / 2f, 2) + Mathf.Pow(Screen.height / 2f, 2));
                float proximity = 1 - Mathf.Clamp01(distanceToCenter / maxDistance); // Closer => closer to 1

                // Heartbeat: pulse speed and scale based on proximity
                float pulseSpeed = Mathf.Lerp(1f, 8f, proximity); // faster when closer
                float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.2f; // pulsating between 0.8 to 1.2 scale
                indicator.transform.localScale = new Vector3(scale, scale, 1f);

                // Optional: Rotate towards missile (if desired)
                Vector3 dir = missile.transform.position - cam.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }


            /*if (isOffscreen)
            {
                // Clamp the position to screen edges
                screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
                screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

                indicator.transform.position = screenPos;

                Vector3 dir = missile.transform.position - cam.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90 );
            }*/
        }

        // Clean up null missiles
        foreach (var m in toRemove)
        {
            missileIndicators.Remove(m);
        }
    }

    public void AddMissile(GameObject missile)
    {
        if (!missileIndicators.ContainsKey(missile))
        {
            GameObject indicator = Instantiate(indicatorPrefab, canvasRect);
            missileIndicators.Add(missile, indicator);
        }
    }

    public void RemoveMissile(GameObject missile)
    {
        if (missileIndicators.ContainsKey(missile))
        {
            Destroy(missileIndicators[missile]);
            missileIndicators.Remove(missile);
        }
    }
}
