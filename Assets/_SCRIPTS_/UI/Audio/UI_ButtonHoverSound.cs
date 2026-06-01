using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonHoverSound : MonoBehaviour
{
    Button button;
    CanvasGroup canvasGroup;

    void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponentInParent<CanvasGroup>();

        button.onClick.AddListener(ClickSound);
    }

    public void HoverSound()
    {
        if (button != null && button.interactable == false) return;
        if (canvasGroup != null && canvasGroup.interactable == false) return;

        AudioManager.Play("UI Pluck");
    }

    public void ClickSound()
    {
        AudioManager.Play("UI Pluck 2");
    }
}
