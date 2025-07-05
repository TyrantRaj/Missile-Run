using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]public GameObject scorePopupPrefab;
    [SerializeField] public RectTransform scoreTextTransform;
    [SerializeField] public RectTransform canvasTransform;


    public static ScoreManager Instance;
    private float lastMultiplierTime = 0f;

    public float score = 0f;
    public float scoreMultiplier = 1f; // Increase this for faster score gain
    public TextMeshProUGUI scoreText;

    private bool isRunning = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isRunning)
        {
            score += Time.deltaTime * scoreMultiplier;
            UpdateUI();

            // Check if 30 seconds have passed since last increase
            if (Time.timeSinceLevelLoad - lastMultiplierTime >= 30f)
            {
                IncreaseMultiplier();
                lastMultiplierTime = Time.timeSinceLevelLoad;
            }
        }
    }


    public void IncreaseMultiplier(int amount = 1)
    {
        scoreMultiplier += amount;
    }


    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    public void StopScoring()
    {
        isRunning = false;
    }

    public void AddScore(int amount, Vector3 worldPos)
    {
        score += amount;
        UpdateUI();

        // Spawn in world space
        GameObject popup = Instantiate(scorePopupPrefab, worldPos, Quaternion.identity);
        popup.GetComponent<ScorePopup>().SetText("+" + amount);
    }


    public void ResetScore()
    {
        score = 0;
        isRunning = true;
    }
}
