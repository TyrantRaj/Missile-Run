using UnityEngine;

public class SurviveTime : MonoBehaviour
{
    public bool timerRunning = true; 

    public float currentTime = 0f;
    public float SurcvivalarchivementTime = 0f;
    public float survivewithoutpoweruptimer = 0f;
    public float SlowMotionTimer = 0f;

    private int isSurvie1mincompleted;
    private int isSurvie10min;
    private int is3minwithoutpower;
    private int isslowmotionpower;

    private void Awake()
    {
        // Load achievement completion status safely in Awake
        isSurvie1mincompleted = PlayerPrefs.GetInt("1min", 0);
        isSurvie10min = PlayerPrefs.GetInt("10min", 0);
        is3minwithoutpower = PlayerPrefs.GetInt("3min", 0);
        isslowmotionpower = PlayerPrefs.GetInt("slowmotion", 0);
    }

    void Update()
    {
        if (!timerRunning) return;

        currentTime += Time.deltaTime;
        SurcvivalarchivementTime += Time.deltaTime;
        survivewithoutpoweruptimer += Time.deltaTime;
        SlowMotionTimer += Time.deltaTime;

        CheckAchievements();
    }

    private void CheckAchievements()
    {
        if (SurcvivalarchivementTime > 60f && isSurvie1mincompleted == 0)
        {
            UnlockAchievement("1min");
            isSurvie1mincompleted = 1;
            UpdateMission(Mission.MissionType.survive1min);
        }

        if (currentTime > 600f && isSurvie10min == 0)
        {
            UnlockAchievement("10min");
            isSurvie10min = 1;
            UpdateMission(Mission.MissionType.survive10min);
        }

        if (survivewithoutpoweruptimer > 180f && is3minwithoutpower == 0)
        {
            UnlockAchievement("3min");
            is3minwithoutpower = 1;
            UpdateMission(Mission.MissionType.withoutpower);
        }

        if (SlowMotionTimer >= 180f && isslowmotionpower == 0)
        {
            UnlockAchievement("slowmotion");
            isslowmotionpower = 1;
            UpdateMission(Mission.MissionType.withoutSlowmotion);
        }
    }

    private void UnlockAchievement(string key)
    {
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        Debug.Log($"Achievement unlocked: {key}");
        // Optionally: trigger UI popup here
    }

    private void UpdateMission(Mission.MissionType type)
    {
        foreach (var mission in MissionManager.Instance.currentMissions)
        {
            if (mission.missionType == type && !mission.isCompleted)
            {
                mission.currentValue++;
                if (mission.currentValue >= mission.targetValue)
                    mission.isCompleted = true;
            }
        }
    }

    // Reset methods to be called externally when conditions break
    public void ResetNoHitTimer() => SurcvivalarchivementTime = 0f;
    public void ResetNoPowerUpTimer() => survivewithoutpoweruptimer = 0f;
    public void ResetNoSlowMotionTimer() => SlowMotionTimer = 0f;
}
