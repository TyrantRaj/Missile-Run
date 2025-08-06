using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public int sessionCoins = 0; 
    public TMP_Text coinText;

    void Awake()
    {
        instance = this;
    }

    public void AddCoin(int amount)
    {
        sessionCoins += amount;
        UpdateUI();
    }

    public void SaveSessionCoinsToTotal()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += sessionCoins;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();

        Debug.Log("Saved " + sessionCoins + " coins to total. Total now: " + totalCoins);

        //sessionCoins = 0;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = sessionCoins.ToString();
    }
}
