using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    public float moveUpDistance = 50f;
    public float duration = 0.5f;
    public float scaleAmount = 1.2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timer;

    void OnEnable()
    {
        startPosition = transform.localPosition;
        targetPosition = startPosition + new Vector3(0, moveUpDistance, 0);
        timer = 0f;
        transform.localScale = Vector3.one * scaleAmount;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // Move upwards
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

        // Scale down back to normal
        float scale = Mathf.Lerp(scaleAmount, 1f, t);
        transform.localScale = Vector3.one * scale;

        // Fade out or destroy
        if (t >= 1f)
        {
            Destroy(gameObject); // You can also use pooling for efficiency
        }
    }

    public void SetText(string text)
    {
        popupText.text = text;
    }
}
