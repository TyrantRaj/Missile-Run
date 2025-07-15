using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public void pause()
    {
        Time.timeScale = 0f;
    }

    public void resume()
    {
        Time.timeScale = 1f;
    }
}
