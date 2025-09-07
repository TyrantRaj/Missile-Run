using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public PopupManager popup;

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

    private List<JetData> jetSkins;
    private JetDatabase jetDatabase;

    [SerializeField] private int[] jetPrices;

    private float targetSpeedValue = 0f;
    private float targetRotationValue = 0f;

    private float displayedSpeedValue = 0f;
    private float displayedRotationValue = 0f;

    void Start()
    {
        //  Load from Resources folder
        jetDatabase = Resources.Load<JetDatabase>("JetDatabase");
        if (jetDatabase == null)
        {
            Debug.LogError("JetDatabase not found in Resources!");
            return;
        }

        jetSkins = new List<JetData>(jetDatabase.jetSkins);

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

    private float scrollVelocity = 0f;
    private float lastScrollPos = 0f;

    void Update()
    {
        float lerpSpeed = 5f;

        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            scrollPos = scrollBar.value;

            // Track velocity for momentum
            scrollVelocity = (scrollPos - lastScrollPos) / Time.deltaTime;
            lastScrollPos = scrollPos;
        }
        else
        {
            if (isDragging)
            {
                isDragging = false;

                // When released, apply momentum scroll
                scrollVelocity = Mathf.Clamp(scrollVelocity, -1f, 1f); // Limit crazy fast flicks
                scrollPos += scrollVelocity * 0.2f; // Apply momentum strength
                scrollPos = Mathf.Clamp01(scrollPos);

                // Snap to nearest
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
                        break;
                    }
                }
            }

            // Smoothly lerp to target
            scrollBar.value = Mathf.Lerp(scrollBar.value, targetPos, Time.deltaTime * 8);
        }

        // Smooth attribute UI update
        displayedSpeedValue = Mathf.Lerp(displayedSpeedValue, targetSpeedValue, Time.deltaTime * lerpSpeed);
        displayedRotationValue = Mathf.Lerp(displayedRotationValue, targetRotationValue, Time.deltaTime * lerpSpeed);

        if (speedBar != null)
            speedBar.value = displayedSpeedValue;

        if (rotationBar != null)
            rotationBar.value = displayedRotationValue;

        // Scale effect on selected item
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
            selectButtonText.text = jetPrices[selectedIndex].ToString();
        }
    }

    public void OnSelectButtonClick()
    {
        if (unlockedSkins[selectedIndex])
        {
            StartCoroutine(PlaySoundAndChangeScene());
        }
        else
        {
            TryBuyJet();
        }
    }

    private IEnumerator PlaySoundAndChangeScene()
    {
        AudioClip clip = SoundManager.PlaySoundAndReturn(SoundManager.Sound.ButtonClick);

        if (clip != null)
            yield return new WaitForSeconds(clip.length);

        SaveSelectedCharacter();
        SceneManager.LoadScene("MainMenu");
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
            
            popup.ShowPopup("You don’t have enough cash!");
            
        }
    }

    public void AddCheatCoins(int amount)
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();

        Debug.Log($"Cheat: Added {amount} coins. Total: {totalCoins}");
        UpdateTotalCoinsUI();
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
