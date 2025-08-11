using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<Mission> currentMissions;

    public static MissionManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        LoadMissionProgress();
    }

    public void CollectReward(int missionIndex)
    {
        if (missionIndex < 0 || missionIndex >= currentMissions.Count) return;
        Mission mission = currentMissions[missionIndex];
        if (!mission.isCompleted || mission.isCollected) return;

        mission.isCollected = true;
        CurrencyManager.instance.CollectCoin(mission.coinReward);
        SaveMissionProgress();
    }

    // Save mission progress (example with PlayerPrefs)
    public void SaveMissionProgress()
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            PlayerPrefs.SetInt($"Mission_{i}_Completed", currentMissions[i].isCompleted ? 1 : 0);
            PlayerPrefs.SetInt($"Mission_{i}_Collected", currentMissions[i].isCollected ? 1 : 0);
            PlayerPrefs.SetInt($"Mission_{i}_CurrentValue", currentMissions[i].currentValue);
        }
        PlayerPrefs.Save();
    }

    // Load mission progress
    public void LoadMissionProgress()
    {
        for (int i = 0; i < currentMissions.Count; i++)
        {
            currentMissions[i].isCompleted = PlayerPrefs.GetInt($"Mission_{i}_Completed", 0) == 1;
            currentMissions[i].isCollected = PlayerPrefs.GetInt($"Mission_{i}_Collected", 0) == 1;
            currentMissions[i].currentValue = PlayerPrefs.GetInt($"Mission_{i}_CurrentValue", 0);
        }
    }
}
