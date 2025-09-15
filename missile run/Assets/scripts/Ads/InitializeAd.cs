using UnityEngine;
using UnityEngine.Advertisements;
public class InitializeAd : MonoBehaviour , IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool isTesting;

    private string gameId;

    private void Awake()
    {
        #if UNITY_IOS
            gameId = iosGameId;
        #elif UNITY_ANDROID
            gameId = androidGameId;
        #elif UNITY_EDITOR
            gameId = androidGameId;
        #endif

        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId,isTesting,this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ad Initialized Success");
        AdsManager.Instance.interstitialAd.LoadInterstitalAd();
        AdsManager.Instance.rewardedAds.LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ad Initialized Failed");
    }
}
