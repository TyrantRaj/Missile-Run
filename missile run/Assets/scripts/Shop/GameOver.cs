using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    private bool isGameOverUIVisible = false;
    private bool isAnimating = false;
    private bool isCoinSaved = false;
    [SerializeField] SurviveTime timer;

    [SerializeField] TMP_Text time_text;
    [SerializeField] GameObject GameUi;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] Animator ExplotionAnim;
    [SerializeField] ScoreManager scoremanager;
    [SerializeField] GameObject gameOverGO;
    [SerializeField] TMP_Text HighscoretitleTxt;
    [SerializeField] TMP_Text CoinsCollectedTxt;
    [SerializeField] TMP_Text ScoreTxt;

    [SerializeField] float countSpeed = 200f;
    [SerializeField] float shakeIntensity = 1.2f;
    [SerializeField] float highscoreScaleTime = 0.5f;

    private PlayerMovement playerScript;

    private int targetCoins;
    private int displayedCoins = 0;

    private int targetScore;
    private int displayedScore = 0;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    [SerializeField] private GameObject doubleCoinsButton; // assign in Inspector

    

    public void GameOverFunction()
    {
        if (isGameOverUIVisible || isAnimating) return;

        Time.timeScale = 1f;
        timer.timerRunning = false;
        GameUi.SetActive(false);
        spawner.canSpawn = false;
        scoremanager.StopScoring();

        float survivedSeconds = timer.currentTime;
        time_text.text = FormatTime(survivedSeconds);

        GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
        foreach (GameObject missile in missiles)
        {
            HandleMissileExplosion(missile);
        }

        playerScript.Die();
        playerScript.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ExplotionAnim.SetTrigger("Explode");

        ScoreManager.Instance.StopScoring();

        targetCoins = CurrencyManager.instance.sessionCoins;
        targetScore = Mathf.FloorToInt(ScoreManager.Instance.score);

        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        if (targetScore > highscore)
        {
            PlayerPrefs.SetInt("HighScore", targetScore);
            PlayerPrefs.Save();
            StartCoroutine(AnimateHighscoreTitle());
        }

        if (AdsManager.Instance != null && targetCoins > 0)
        {
            // Show button if ads are ready and player earned some coins
            doubleCoinsButton.SetActive(true);
        }
        else
        {
            doubleCoinsButton.SetActive(false);
        }

        // Save only base coins for now if no ads are watched
        

        StartCoroutine(ShowGameOverUI());
    }

    /*public void OnDoubleCoinsButton()
    {
        if (AdsManager.Instance != null)
        {
            doubleCoinsButton.SetActive(false); // hide immediately after pressing

            AdsManager.Instance.rewardedAds.ShowRewardedAd(() =>
            {
                // Player watched ad completely → total = double
                int doubledCoins = targetCoins;

                CoinsCollectedTxt.text = doubledCoins.ToString();

                // Overwrite session coins
                CurrencyManager.instance.sessionCoins = doubledCoins;

                // Save new doubled value
                CurrencyManager.instance.SaveSessionCoinsToTotal();

                Debug.Log($"Player received double coins! Total saved: {doubledCoins}");
            });
        }
    }*/

    public void OnDoubleCoinsButton()
    {
        if (AdsManager.Instance != null)
        {
            doubleCoinsButton.SetActive(false); // hide immediately after pressing

            AdsManager.Instance.rewardedAds.ShowRewardedAd(() =>
            {
                // Player watched ad completely → set doubled value
                targetCoins = targetCoins * 2;

                // Overwrite session coins
                CurrencyManager.instance.sessionCoins = targetCoins;
                CurrencyManager.instance.SaveSessionCoinsToTotal();
                isCoinSaved = true;
                Debug.Log($"Player received double coins! Total saved: {targetCoins}");

                // Restart the coin count-up animation with doubled coins
                StopCoroutine(CountUpValues());
                StartCoroutine(CountUpValues());
            });
        }
    }





    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }



    IEnumerator ShowGameOverUI()
    {
        if (isGameOverUIVisible || isAnimating)
            yield break;

        isAnimating = true;

        gameOverGO.SetActive(true);
        RectTransform rect = gameOverGO.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;

        float duration = 0.4f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(0f, 1f, t / duration);
            rect.localScale = Vector3.one * scale;
            yield return null;
        }

        rect.localScale = Vector3.one;
        isGameOverUIVisible = true;
        isAnimating = false;

        StartCoroutine(CountUpValues());
    }


    IEnumerator HideGameOverUI(System.Action onComplete)
    {
        if (!isGameOverUIVisible || isAnimating)
        {
            onComplete?.Invoke();
            yield break;
        }

        isAnimating = true;

        RectTransform rect = gameOverGO.GetComponent<RectTransform>();
        float duration = 0.3f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(1f, 0f, t / duration);
            rect.localScale = Vector3.one * scale;
            yield return null;
        }

        rect.localScale = Vector3.zero;
        gameOverGO.SetActive(false);
        isGameOverUIVisible = false;
        isAnimating = false;

        onComplete?.Invoke();
    }


    IEnumerator CountUpValues()
    {
        displayedCoins = 0;
        displayedScore = 0;
        CoinsCollectedTxt.text = "0";
        ScoreTxt.text = "0";

        float scaleDuration = 0.1f;
        Vector3 normalScale = Vector3.one;
        Vector3 popScale = Vector3.one * shakeIntensity;

        while (displayedCoins < targetCoins || displayedScore < targetScore)
        {
            bool updated = false;

            if (displayedCoins < targetCoins)
            {
                displayedCoins += Mathf.CeilToInt(countSpeed * Time.deltaTime);
                displayedCoins = Mathf.Min(displayedCoins, targetCoins);
                CoinsCollectedTxt.text = "" +displayedCoins;
                StartCoroutine(PopTextScale(CoinsCollectedTxt.rectTransform, popScale, normalScale, scaleDuration));
                updated = true;
            }

            if (displayedScore < targetScore)
            {
                displayedScore += Mathf.CeilToInt(countSpeed * Time.deltaTime);
                displayedScore = Mathf.Min(displayedScore, targetScore);
                ScoreTxt.text = "" + displayedScore;
                StartCoroutine(PopTextScale(ScoreTxt.rectTransform, popScale, normalScale, scaleDuration));
                updated = true;
            }

            if (updated)
                yield return new WaitForSeconds(0.02f);

            yield return null;
        }
    }

    IEnumerator PopTextScale(RectTransform rectTransform, Vector3 popScale, Vector3 normalScale, float duration)
    {
        rectTransform.localScale = popScale;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            rectTransform.localScale = Vector3.Lerp(popScale, normalScale, t / duration);
            yield return null;
        }

        rectTransform.localScale = normalScale;
    }

    IEnumerator AnimateHighscoreTitle()
    {
        HighscoretitleTxt.text = "NEW HIGHSCORE!";
        HighscoretitleTxt.color = Color.yellow;

        Vector3 originalScale = HighscoretitleTxt.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        float t = 0;
        while (t < highscoreScaleTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 1.5f, t / highscoreScaleTime);
            HighscoretitleTxt.transform.localScale = originalScale * scale;
            yield return null;
        }

        t = 0;
        while (t < highscoreScaleTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(1.5f, 1f, t / highscoreScaleTime);
            HighscoretitleTxt.transform.localScale = originalScale * scale;
            yield return null;
        }

        HighscoretitleTxt.transform.localScale = originalScale;
    }

    public void Restart()
    {
        StartCoroutine(HideGameOverUI(() =>
        {
            if (!isCoinSaved)
            {
            CurrencyManager.instance.SaveSessionCoinsToTotal();

            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }));
    }

    public void MainMenu()
    {
        StartCoroutine(HideGameOverUI(() =>
        {
            if (!isCoinSaved) { 

            CurrencyManager.instance.SaveSessionCoinsToTotal();
            }
            SceneManager.LoadScene("MainMenu");
        }));
    }

    void HandleMissileExplosion(GameObject missile)
    {
        if (missile.TryGetComponent<targeting_missile>(out var targeting))
        {
            targeting.missile_speed = 0;
            targeting.rotate_speed = 0;
        }
        else if (missile.TryGetComponent<WaveMissile>(out var wave))
        {
            wave.missile_speed = 0;
            wave.enabled = false;
        }
        else if (missile.TryGetComponent<OneHitMissile>(out var oneHit))
        {
            oneHit.missile_speed = 0;
            oneHit.rotate_speed = 0;
            oneHit.enabled = false;
        }

        if (missile.TryGetComponent<Animator>(out var anim))
        {
            anim.Play("Explosion");
            Destroy(missile, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Destroy(missile);
        }

        if (FindObjectOfType<IndicatorManager>() is { } indicatorMgr)
        {
            indicatorMgr.RemoveTarget(missile);
        }
    }


}
