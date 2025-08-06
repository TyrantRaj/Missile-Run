using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public TMP_Text totalCoinsText;

    public Slider speedBar;
    public Slider rotationBar;
    public Scrollbar scrollBar;
    public Button selectBtn;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterDesText;
    public TextMeshProUGUI selectButtonText;

    private float scrollPos = 0;
    private float targetPos = 0;
    private float[] pos;
    private int selectedIndex = 0;
    private bool isDragging = false;
    private bool[] unlockedSkins;

    public List<JetData> jetSkins = new List<JetData>()
{
    new JetData { name = "Skybolt", moveSpeed = 3.0f, rotationSpeed = 2.0f, description = "A nimble pioneer of the skies." },
    new JetData { name = "Rustburner", moveSpeed = 3.4f, rotationSpeed = 2.2f, description = "Old but durable." },
    new JetData { name = "Iron Talon", moveSpeed = 3.7f, rotationSpeed = 2.25f, description = "Forged for impact." },
    new JetData { name = "Solar Wasp", moveSpeed = 4.1f, rotationSpeed = 2.45f, description = "Slick and sunny fast." },
    new JetData { name = "Thunderbite", moveSpeed = 4.5f, rotationSpeed = 2.8f, description = "Zaps across battle." },
    new JetData { name = "Toxic Fang", moveSpeed = 4.8f, rotationSpeed = 3.0f, description = "Fast and venomous." },
    new JetData { name = "Swamp Ghost", moveSpeed = 5.0f, rotationSpeed = 3.1f, description = "Moves in shadows." },
    new JetData { name = "Crimson Edge", moveSpeed = 5.4f, rotationSpeed = 3.4f, description = "Sharper, deadlier." },
    new JetData { name = "Glacier Bat", moveSpeed = 5.7f, rotationSpeed = 3.6f, description = "Cold and clean." },
    new JetData { name = "Void Reaper", moveSpeed = 6.0f, rotationSpeed = 4.0f, description = "Max speed & turn!" }
};


    [SerializeField]
    private int[] jetPrices; // Set prices in Inspector

    private float targetSpeedValue = 0f;
    private float targetRotationValue = 0f;

    private float displayedSpeedValue = 0f;
    private float displayedRotationValue = 0f;

    void Start()
    {
        UpdateTotalCoinsUI();

        int childCount = transform.childCount;
        pos = new float[childCount];
        unlockedSkins = new bool[childCount];

        float distance = 1f / (childCount - 1f);
        for (int i = 0; i < childCount; i++)
        {
            pos[i] = distance * i;
            unlockedSkins[i] = PlayerPrefs.GetInt("SkinUnlocked_" + i, (i == 0) ? 1 : 0) == 1;
        }

        selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        scrollPos = targetPos = pos[selectedIndex];
        scrollBar.value = scrollPos;

        if (jetSkins.Count > 0)
        {
            displayedSpeedValue = jetSkins[selectedIndex].moveSpeed;
            displayedRotationValue = jetSkins[selectedIndex].rotationSpeed;
            targetSpeedValue = displayedSpeedValue;
            targetRotationValue = displayedRotationValue;
        }

        UpdateCharacterName();
        UpdateButtonText();

        selectBtn.onClick.AddListener(OnSelectButtonClick);
    }

    public void UpdateTotalCoinsUI()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoinsText.text = totalCoins.ToString();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            scrollPos = scrollBar.value;
        }
        else
        {
            if (isDragging)
            {
                isDragging = false;
                float distance = 1f / (pos.Length - 1f);

                for (int i = 0; i < pos.Length; i++)
                {
                    if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                    {
                        selectedIndex = i;
                        targetPos = pos[i];

                        SoundManager.PlaySound(SoundManager.Sound.Scroll);
                        UpdateCharacterName();
                        UpdateButtonText();
                        break; //  Removed auto save
                    }
                }
            }

            scrollBar.value = Mathf.Lerp(scrollBar.value, targetPos, Time.deltaTime * 10);
        }

        float lerpSpeed = 5f;
        displayedSpeedValue = Mathf.Lerp(displayedSpeedValue, targetSpeedValue, Time.deltaTime * lerpSpeed);
        displayedRotationValue = Mathf.Lerp(displayedRotationValue, targetRotationValue, Time.deltaTime * lerpSpeed);

        if (speedBar != null)
            speedBar.value = displayedSpeedValue;

        if (rotationBar != null)
            rotationBar.value = displayedRotationValue;

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2 targetScale = (i == selectedIndex) ? new Vector2(1.2f, 1.2f) : new Vector2(1f, 1f);
            transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, targetScale, Time.deltaTime * 5);
        }
    }

    void UpdateCharacterName()
    {
        if (selectedIndex < jetSkins.Count)
        {
            JetData currentJet = jetSkins[selectedIndex];

            characterNameText.text = currentJet.name;
            characterDesText.text = currentJet.description;

            targetSpeedValue = currentJet.moveSpeed;
            targetRotationValue = currentJet.rotationSpeed;

            selectBtn.interactable = true;
        }
    }

    void UpdateButtonText()
    {
        if (unlockedSkins[selectedIndex])
        {
            selectButtonText.text = "Select";
        }
        else
        {
            selectButtonText.text = "Buy (" + jetPrices[selectedIndex] + ")";
        }
    }

    public void OnSelectButtonClick()
    {
        if (unlockedSkins[selectedIndex])
        {
            SaveSelectedCharacter(); // Save only when clicked
            SceneManager.LoadScene("MainMenu"); // Replace with your scene name
        }
        else
        {
            TryBuyJet();
        }
    }

    public void TryBuyJet()
    {
        int price = jetPrices[selectedIndex];
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        if (totalCoins >= price)
        {
            totalCoins -= price;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.Save();

            UnlockSkin(selectedIndex);
            UpdateTotalCoinsUI();
            UpdateButtonText();
        }
        else
        {
            Debug.Log("Not enough coins to unlock this jet.");
            // Optionally: Show UI warning
        }
    }

    public void AddCheatCoins(int amount)
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();

        Debug.Log($"Cheat: Added {amount} coins. Total: {totalCoins}");
        UpdateTotalCoinsUI(); // Refresh display
    }

    public void UnlockSkin(int index)
    {
        if (index < unlockedSkins.Length)
        {
            unlockedSkins[index] = true;
            PlayerPrefs.SetInt("SkinUnlocked_" + index, 1);
            PlayerPrefs.Save();
            Debug.Log("Unlocked Skin: " + index);
        }
    }

    public void SaveSelectedCharacter()
    {
        if (unlockedSkins[selectedIndex])
        {
            PlayerPrefs.SetInt("SelectedCharacter", selectedIndex);

            JetData selectedJet = jetSkins[selectedIndex];
            PlayerPrefs.SetFloat("JetSpeed_" + selectedIndex, selectedJet.moveSpeed);
            PlayerPrefs.SetFloat("JetRot_" + selectedIndex, selectedJet.rotationSpeed);

            PlayerPrefs.Save();

            SelectedJetStats.moveSpeed = selectedJet.moveSpeed;
            SelectedJetStats.rotationSpeed = selectedJet.rotationSpeed;
            SelectedJetStats.jetSprite = selectedJet.sprite;
            SelectedJetStats.selectedJetIndex = selectedIndex;
        }
    }

}
