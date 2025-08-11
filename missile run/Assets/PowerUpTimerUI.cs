using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerUpTimerUI : MonoBehaviour
{
    [Header("References (assign in prefab)")]
    //public TMP_Text nameText;    
    public TMP_Text timerText;   
    public Image iconImage;      

    public void SetIcon(Sprite s)
    {
        if (iconImage != null)
        {
            iconImage.sprite = s;
            iconImage.enabled = s != null;
        }
    }

    /*public void SetName(string name)
    {
        if (nameText != null) nameText.text = name;
    }*/

    public void UpdateTimerText(float seconds)
    {
        if (timerText != null) timerText.text = $"{Mathf.Max(0f, seconds):F1}s";
    }
}
