using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AllPowers : MonoBehaviour
{
    SurviveTime timer;
    [SerializeField] Image FillImg;
    [SerializeField] Sprite OrdinarySprite;
    [SerializeField] Sprite duringReduceSprite;

    public float slowMoDuration = 5f;
    public Slider progressBar;

    private void Start()
    {
        timer = FindAnyObjectByType<SurviveTime>();
    }

    public void EnableSlowMotion()
    {
        timer.ResetNoSlowMotionTimer();
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        UpdateAllSoundPitch(Time.timeScale);
    }

    public void DisableSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        UpdateAllSoundPitch(1f);
    }

    private void UpdateAllSoundPitch(float newPitch)
    {
        GameObject[] soundObjects = GameObject.FindGameObjectsWithTag("Sound");
        foreach (GameObject soundObj in soundObjects)
        {
            AudioSource audio = soundObj.GetComponent<AudioSource>();
            if (audio != null)
                audio.pitch = newPitch;
        }
    }
}
