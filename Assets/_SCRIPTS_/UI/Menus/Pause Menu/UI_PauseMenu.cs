using DG.Tweening;
using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    UI_PopUpMenu uI_PopUpMenu;

    [SerializeField] CanvasGroup backgroundToFade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uI_PopUpMenu = GetComponent<UI_PopUpMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        Time.timeScale = 0f;

        uI_PopUpMenu.Open();

        backgroundToFade.DOKill(false);
        backgroundToFade.DOFade(1f, 0.4f).SetEase(Ease.Linear).SetUpdate(true);
    }
}
