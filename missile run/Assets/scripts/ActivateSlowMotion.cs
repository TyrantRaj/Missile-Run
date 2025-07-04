using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivateSlowMotion : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider slideProgress;
    [SerializeField] private AllPowers PowerScript;

    [SerializeField] private float depletionRate = 0.3f; // Speed of slider decrease
    [SerializeField] private float rechargeRate = 0.1f;  // Speed of slider recharge
    private bool isHolding = false;
    private bool powerDepleted = false;

    void Update()
    {
        if (isHolding )
        {
            slideProgress.value -= depletionRate * Time.unscaledDeltaTime;

            if (slideProgress.value <= 0)
            {
                slideProgress.value = 0;
                //powerDepleted = true;
                isHolding = false;
                PowerScript.DisableSlowMotion();
                Debug.Log("Power depleted - Slow motion disabled");
            }
        }
        else
        {
            if (slideProgress.value < 1f)
            {
                slideProgress.value += rechargeRate * Time.unscaledDeltaTime;

                if (slideProgress.value >= 1f)
                {
                    slideProgress.value = 1f;
                    //powerDepleted = false; // Re-enable usage
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!powerDepleted)
        {
            isHolding = true;
            PowerScript.EnableSlowMotion();
            Debug.Log("Button down - Slow motion enabled");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHolding)
        {
            PowerScript.DisableSlowMotion();
            Debug.Log("Button released - Slow motion disabled");
        }

        isHolding = false;
    }
}
