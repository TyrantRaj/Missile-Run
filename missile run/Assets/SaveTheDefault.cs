using UnityEngine;

public class SaveTheDefault : MonoBehaviour
{
    private void Start()
    {
        if (!PlayerPrefs.HasKey("SpeedDuration"))
        {
            PlayerPrefs.SetInt("SpeedDuration", 0);
            PlayerPrefs.SetInt("SlowMotionDuration", 0);
            PlayerPrefs.SetInt("DoubleCoinDuration", 0);
            PlayerPrefs.SetInt("DoubleScoreDuration", 0);
            PlayerPrefs.SetInt("MagnetDuration", 0);

            PlayerPrefs.Save();
        }
    }
}
