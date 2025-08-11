using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadScene(string scenename)
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        SceneManager.LoadScene(scenename);
    }

    public void LoadAchievements()
    {
        SceneManager.LoadScene("MissionScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // This will show in Editor
        Application.Quit();     // This works only in a built app
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();

    }
}
