using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float moveUpDistance = 80f;
    public float duration = 2f;
    public float scaleAmount = 1.3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timer;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Add CanvasGroup for fading if not already attached
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void OnEnable()
    {
        timer = 0f;
        transform.localScale = Vector3.one * scaleAmount;

        startPosition = transform.localPosition;
        targetPosition = startPosition + new Vector3(0, moveUpDistance, 0);

        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // Smooth move upward
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

        // Smooth scale back to normal
        float scale = Mathf.Lerp(scaleAmount, 1f, t);
        transform.localScale = Vector3.one * scale;

        // Smooth fade out
        if (canvasGroup != null)
            canvasGroup.alpha = 1f - t;

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        popupText.text = text;
    }
}
