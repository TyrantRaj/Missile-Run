using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float timer;
    private int frameCount;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        timer += Time.unscaledDeltaTime;
        frameCount++;

        if (timer >= 1f)
        {
            int fps = Mathf.RoundToInt(frameCount / timer);
            fpsText.text = fps.ToString();

            timer = 0f;
            frameCount = 0;
        }
    }
}
