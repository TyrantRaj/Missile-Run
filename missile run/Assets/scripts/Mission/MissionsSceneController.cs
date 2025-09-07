using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionsSceneController : MonoBehaviour
{
    [SerializeField] private Transform missionsContentParent; // ScrollView content transform
    [SerializeField] private GameObject missionEntryPrefab;

    private List<GameObject> missionEntries = new List<GameObject>();

    void Start()
    {
        PopulateMissionList();
    }

    void PopulateMissionList()
    {
        // Clear old entries
        foreach (var entry in missionEntries)
            Destroy(entry);
        missionEntries.Clear();

        var missions = MissionManager.Instance.currentMissions;

        for (int i = 0; i < missions.Count; i++)
        {
            Mission mission = missions[i];
            GameObject entryGO = Instantiate(missionEntryPrefab, missionsContentParent);
            missionEntries.Add(entryGO);

            // Get UI references
            var texts = entryGO.GetComponentsInChildren<TMP_Text>();
            TMP_Text descText = texts[0];
            TMP_Text progressText = texts[1];
            TMP_Text rewardText = texts[2];
            Button collectBtn = entryGO.GetComponentInChildren<Button>();
            Slider progressSlider = entryGO.GetComponentInChildren<Slider>();

            // Set mission details
            descText.text = mission.description;
            progressText.text = $"{mission.currentValue} / {mission.targetValue}";
            rewardText.text = $"{mission.coinReward}";

            if (progressSlider != null)
                progressSlider.value = Mathf.Clamp01((float)mission.currentValue / mission.targetValue);

            // Show/Hide collect button
            if (mission.isCompleted && !mission.isCollected)
            {
                collectBtn.gameObject.SetActive(true);
                collectBtn.interactable = true;
            }
            else
            {
                collectBtn.gameObject.SetActive(false);
            }

            // Button click
            int missionIndex = i; // closure
            collectBtn.onClick.RemoveAllListeners();
            collectBtn.onClick.AddListener(() =>
            {
                // Add reward to coins using PlayerPrefs
                PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins", 0) + mission.coinReward);
                PlayerPrefs.Save();

                // Mark mission as collected
                mission.isCollected = true;

                // Save progress so it's persistent
                MissionManager.Instance.SaveMissionProgress();
                SoundManager.PlaySound(SoundManager.Sound.Archivement);

                // Animate the reward text scaling (coin collect effect)
                LeanTween.scale(rewardText.gameObject, rewardText.transform.localScale * 1.5f, 0.3f)
                    .setEasePunch()
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(rewardText.gameObject, rewardText.transform.localScale / 1.5f, 0.2f);
                    });

                // Animate the entry fading out slightly for collected feel
                CanvasGroup cg = entryGO.GetComponent<CanvasGroup>();
                if (cg == null) cg = entryGO.AddComponent<CanvasGroup>();
                LeanTween.alphaCanvas(cg, 0.5f, 0.5f);

                // Update UI
                collectBtn.interactable = false;
                collectBtn.gameObject.SetActive(false);
                progressText.text = "Collected!";
                if (progressSlider != null) progressSlider.value = 1f;
            });
        }
    }
}
