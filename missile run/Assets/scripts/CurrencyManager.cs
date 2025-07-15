using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public int coins = 0;
    public TMP_Text coinText;

    void Awake()
    {
        instance = this;
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        coinText.text = coins.ToString();
    }
}
