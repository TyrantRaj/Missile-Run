using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public enum UpgradeItems
    {
        SlowMotion,
        Speed,
        Magnet,
        DoubleCoin,
        DoubleScore
    }

    [SerializeField] private TMP_Text current_Coin;
    [SerializeField] private UpgradeItems itemID;
    [SerializeField] private Image[] Bars;
    [SerializeField] private Button buyBtn;
    [SerializeField] private TMP_Text NextUpgradeDes;
    [SerializeField] private TMP_Text PriceText;

    private readonly int[] upgradeDurations = { 12, 15, 17, 20, 25 };
    private readonly int[] upgradePrices = { 0, 100, 200, 300, 400 };

    private int currentLevel;
    private int totalCoins;

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt(itemID + "Level", 0);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        UpdateBars();
        UpdateUI();
        UpdateCoinText();

        buyBtn.onClick.AddListener(OnBuyClicked);
    }

    private void UpdateBars()
    {
        for (int i = 0; i < Bars.Length; i++)
        {
            Bars[i].color = i <= currentLevel ? Color.green : Color.white;
        }
    }

    private void UpdateUI()
    {
        if (currentLevel >= upgradeDurations.Length - 1)
        {
            NextUpgradeDes.text = "MAX LEVEL";
            PriceText.text = "";
            buyBtn.interactable = false;
        }
        else
        {
            NextUpgradeDes.text = $"{upgradeDurations[currentLevel]}s → {upgradeDurations[currentLevel + 1]}s";
            PriceText.text = $"{upgradePrices[currentLevel + 1]} COINS";
            buyBtn.interactable = totalCoins >= upgradePrices[currentLevel + 1];
        }
    }

    private void UpdateCoinText()
    {
        if (current_Coin != null)
            current_Coin.text = totalCoins.ToString();
    }

    private void OnBuyClicked()
    {
        if (currentLevel >= upgradeDurations.Length - 1) return;

        int price = upgradePrices[currentLevel + 1];
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (totalCoins >= price)
        {
            totalCoins -= price;
            currentLevel++;

            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.SetInt(itemID + "Level", currentLevel);
            PlayerPrefs.Save();

            UpdateBars();
            UpdateUI();
            UpdateCoinText();
        }
    }

    public static float GetDuration(UpgradeItems item)
    {
        int[] durations = { 12, 15, 17, 20, 25 };
        int level = PlayerPrefs.GetInt(item + "Level", 0);
        return durations[Mathf.Clamp(level, 0, durations.Length - 1)];
    }
}
