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
            rewardText.text = $"{mission.coinReward} ";

            // Set slider value (0 to 1, clamped)
            if (progressSlider != null)
                progressSlider.value = Mathf.Clamp01((float)mission.currentValue / mission.targetValue);

            // Button state and click event
            collectBtn.interactable = mission.isCompleted && !mission.isCollected;
            collectBtn.onClick.RemoveAllListeners();
            int missionIndex = i; // Capture for closure
            collectBtn.onClick.AddListener(() =>
            {
                MissionManager.Instance.CollectReward(missionIndex);
                collectBtn.interactable = false;
                progressText.text = "Completed!";
                if (progressSlider != null) progressSlider.value = 1f; // Fill slider
            });
        }
    }
}
