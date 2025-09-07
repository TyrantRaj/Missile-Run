using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadScene(string scenename)
    {
        Time.timeScale = 1f;
        StartCoroutine(PlaySoundAndLoadScene(scenename));
    }

    private IEnumerator PlaySoundAndLoadScene(string scenename)
    {
        // Play the button sound and get its clip
        AudioClip clip = SoundManager.PlaySoundAndReturn(SoundManager.Sound.ButtonClick);

        // Wait for clip length if sound exists
        if (clip != null)
            yield return new WaitForSeconds(0.5f);

        // Now change scene
        SceneManager.LoadScene(scenename);
    }

    public void LoadAchievements()
    {
        StartCoroutine(PlaySoundAndLoadScene("MissionScene"));
        //SceneManager.LoadScene("MissionScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // This will show in Editor
        Application.Quit();     // Works only in a built app
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

    }
}
