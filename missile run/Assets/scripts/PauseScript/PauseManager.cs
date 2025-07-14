using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject PauseBtn;
    [SerializeField] GameObject PauseMenu;

    public void ActivatePauseManager()
    {
        PauseBtn.SetActive(false);
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        PauseMenu.SetActive(false);
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;

        SoundManager.PauseAllLoopingSounds(); //  Pause audio
    }

    public void DeActivatePauseManager()
    {
        PauseBtn.SetActive(true);
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Time.timeScale = 1f;
        PauseMenu.SetActive(true);
        PauseMenu.SetActive(false);

        SoundManager.ResumeAllLoopingSounds(); //  Resume audio
    }

}
