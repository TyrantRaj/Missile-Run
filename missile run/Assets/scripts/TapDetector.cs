using UnityEngine;
using System.Collections;

public class TapDetector : MonoBehaviour
{
    [Header("Tap Settings")]
    public float tapThreshold = 0.3f; // Max time between taps
    private int tapCount = 0;
    private Coroutine tapRoutine;

    [SerializeField] AllPowers powersScript;


    // Call this from the left/right buttons' OnClick()
    public void RegisterTap()
    {
        tapCount++;

        if (tapRoutine != null)
            StopCoroutine(tapRoutine);

        tapRoutine = StartCoroutine(DetectTap());
    }

    private IEnumerator DetectTap()
    {
        yield return new WaitForSeconds(tapThreshold);

        if (tapCount == 2)
        {
            OnDoubleTap();
        }
        else if (tapCount >= 3)
        {
            OnTripleTap();
        }

        tapCount = 0;
    }


    void OnDoubleTap()
    {
        Debug.Log("Double Tap Detected - Activate Slow Motion!");
        //powersScript.SlowMotion();
        
    }

    void OnTripleTap()
    {
        Debug.Log("Triple Tap Detected - Maybe Super Power?");
    }
}
