using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    public void ActivatePauseManager()
    {
        PauseMenu.SetActive(false);
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DeActivatePauseManager()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }
}
