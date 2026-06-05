using BoardGame;
using DG.Tweening;
using MapRooms;
using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    UI_PopUpMenu uI_PopUpMenu;

    [SerializeField] CanvasGroup backgroundToFade;
    [SerializeField] GameObject canvasGO;

    PopUpMenuAnimationCurves curves;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uI_PopUpMenu = GetComponent<UI_PopUpMenu>();

        curves = PopUpMenuAnimationCurves.instance;
    }

    public void PauseGame()
    {
        uI_PopUpMenu = GetComponent<UI_PopUpMenu>();

        curves = PopUpMenuAnimationCurves.instance;

        canvasGO.SetActive(true);

        Time.timeScale = 0f;

        uI_PopUpMenu.Open();

        backgroundToFade.DOKill(false);
        backgroundToFade.DOFade(1f, curves.openTime).SetEase(Ease.Linear).SetUpdate(true);

        backgroundToFade.interactable = true;
    }

    public void ResumeGame()
    {
        uI_PopUpMenu.Close();

        backgroundToFade.DOKill(false);
        backgroundToFade.DOFade(1f, curves.closeTime).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            canvasGO.SetActive(false);
            Time.timeScale = 1f;
        });
    }

    public void RestartGame()
    {
        uI_PopUpMenu.Close();

        backgroundToFade.DOKill(false);
        backgroundToFade.DOFade(1f, curves.closeTime).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            canvasGO.SetActive(false);
            Time.timeScale = 1f;

            MapRoomSystem.instance.RespawnCurrentRoom();

            Board.instance.SetUpBoard();

            UI_ItemBoard.ClearItems();
        });
    }

    public void ExitGame()
    {
        uI_PopUpMenu.Close();

        backgroundToFade.DOKill(false);
        backgroundToFade.DOFade(1f, curves.closeTime).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            canvasGO.SetActive(false);
            Time.timeScale = 1f;

            MapRoomSystem.instance.SpawnLevelSelectRoom();

            Board.instance.SetUpBoard();

            UI_ItemBoard.ClearItems();
        });
    }
}
