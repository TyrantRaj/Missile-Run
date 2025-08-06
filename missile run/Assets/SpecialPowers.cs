using System.Collections;
using UnityEngine;
using TMPro;

public class SpecialPowers : MonoBehaviour
{
    PlayerMovement movement;

    private float boostedSpeed = 10f;
    private float duration = 10f;

    private float remainingTime = 0f;
    private Coroutine countdownCoroutine;

    [SerializeField] private TMP_Text countdownText; // Assign in Inspector (or find in Start)

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();

        if (countdownText == null)
        {
            GameObject txtObj = GameObject.Find("SP_Timer_Text");
            if (txtObj != null)
                countdownText = txtObj.GetComponent<TMP_Text>();
        }

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void SpeedUp()
    {
        if (movement != null)
        {
            movement.speed = boostedSpeed;
            remainingTime = duration;

            if (countdownCoroutine != null)
                StopCoroutine(countdownCoroutine);

            countdownCoroutine = StartCoroutine(CountdownCoroutine());
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        while (remainingTime > 0)
        {
            if (countdownText != null)
                countdownText.text = remainingTime.ToString("F1") + "s";

            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        movement.speed = movement.orginalSpeed;

        if (countdownText != null)
        {
            countdownText.text = "";
            countdownText.gameObject.SetActive(false);
        }
    }
}
