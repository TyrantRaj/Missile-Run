using UnityEngine;

public class MissionButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject exclamationImage;

    private void Start()
    {
        // Delay by a frame to ensure MissionManager has loaded
        Invoke(nameof(UpdateExclamation), 0.05f);
    }

    private void OnEnable()
    {
        UpdateExclamation();
    }

    public void UpdateExclamation()
    {
        if (exclamationImage == null) return;

        bool hasUnclaimed = false;

        if (MissionManager.Instance != null)
        {
            foreach (var mission in MissionManager.Instance.currentMissions)
            {
                if (mission.isCompleted && !mission.isCollected)
                {
                    hasUnclaimed = true;
                    break;
                }
            }
        }

        exclamationImage.SetActive(hasUnclaimed);

        if (hasUnclaimed)
            StartPulse();
        else
            StopPulse();
    }

    private void StartPulse()
    {
        LeanTween.cancel(exclamationImage);
        LeanTween.scale(exclamationImage, Vector3.one * 1.2f, 0.5f)
            .setEaseInOutSine()
            .setLoopPingPong(-1);
    }

    private void StopPulse()
    {
        LeanTween.cancel(exclamationImage);
        exclamationImage.transform.localScale = Vector3.one;
    }
}
