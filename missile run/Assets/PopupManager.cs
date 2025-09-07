using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;       // The popup panel
    public TextMeshProUGUI popupText;   // The popup text
    public Button closeButton;          // The close button

    void Start()
    {
        popupPanel.SetActive(false);
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void ShowPopup(string message)
    {
        popupPanel.SetActive(true);
        popupText.text = message;

        // Reset scale & alpha
        popupPanel.transform.localScale = Vector3.zero;
        CanvasGroup cg = popupPanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = popupPanel.AddComponent<CanvasGroup>();
        cg.alpha = 0;

        // Animate in (scale + fade)
        LeanTween.scale(popupPanel, Vector3.one, 0.3f).setEaseOutBack();
        LeanTween.alphaCanvas(cg, 1f, 0.3f);
    }

    void ClosePopup()
    {
        CanvasGroup cg = popupPanel.GetComponent<CanvasGroup>();

        // Animate out (scale + fade)
        LeanTween.scale(popupPanel, Vector3.zero, 0.2f).setEaseInBack();
        LeanTween.alphaCanvas(cg, 0f, 0.2f)
            .setOnComplete(() => popupPanel.SetActive(false));
    }
}
