using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AllPowers : MonoBehaviour
{
    [SerializeField] Image FillImg;
    [SerializeField] Sprite OrdinarySprite;
    [SerializeField] Sprite duringReduceSprite;

    private bool isPowerActive = false;

    public float slowMoDuration = 5f;
    public Slider progressBar;

    public void EnableSlowMotion()
    {
        //FillImg.sprite = duringReduceSprite;
        Time.timeScale = 0.3f; // Example: slow motion
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void DisableSlowMotion()
    {
        //FillImg.sprite = OrdinarySprite;
        Time.timeScale = 1f; // Normal speed
        Time.fixedDeltaTime = 0.02f;
    }
}
