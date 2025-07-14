using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] Color[] cycleColors; // 0 = Day, 1 = Night
    [SerializeField] float timePerCycle = 10f; // seconds per transition
    [SerializeField] CloudManager cloudManager; // Assign this via Inspector

    private Camera cam;
    private int currentColorIndex = 0;
    private float timer = 0f;

    void Start()
    {
        cam = Camera.main;

        if (cycleColors.Length < 2)
        {
            Debug.LogError("Add at least 2 colors to the DayNightCycle.");
            enabled = false;
        }

        cam.backgroundColor = cycleColors[0];

        // Initialize cloud state
        if (cloudManager != null)
            cloudManager.isDay = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / timePerCycle;

        Color fromColor = cycleColors[currentColorIndex];
        Color toColor = cycleColors[(currentColorIndex + 1) % cycleColors.Length];

        cam.backgroundColor = Color.Lerp(fromColor, toColor, t);

        if (t >= 1f)
        {
            timer = 0f;
            currentColorIndex = (currentColorIndex + 1) % cycleColors.Length;

            // Update cloud manager when day/night changes
            if (cloudManager != null)
            {
                cloudManager.isDay = (currentColorIndex == 0); // Day at index 0
            }
        }
    }
}
