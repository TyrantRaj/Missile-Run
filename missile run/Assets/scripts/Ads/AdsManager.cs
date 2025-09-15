using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAd initializead;
    public InterstitialAd interstitialAd;
    public RewardedAds rewardedAds;

    private int gamesPlayed = 0;
    private int showAfter = 3;

    public static AdsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        interstitialAd.LoadInterstitalAd();
        rewardedAds.LoadRewardedAd();
    }

    public void OnGameFinished()
    {
        gamesPlayed++;

        if (gamesPlayed >= showAfter)
        {
            interstitialAd.ShowInterstitialAd();
            gamesPlayed = 0; 
        }
    }
}
