using DG.Tweening;
using UnityEngine;
using MapRooms;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup mainMenuGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        ShowMainMenuWithDelay();
    }

    public void HideMainMenu()
    {
        mainMenuGroup.DOKill(false);
        mainMenuGroup.DOFade(0f, 0.3f);
        mainMenuGroup.interactable = false;
    }

    public void ShowMainMenuWithDelay()
    {
        mainMenuGroup.DOKill(false);
        mainMenuGroup.DOFade(1f, 0.5f).OnComplete(() =>  mainMenuGroup.interactable = true).SetDelay(0.7f);
    }

    public void ShowMainMenu()
    {
        mainMenuGroup.DOKill(false);
        mainMenuGroup.DOFade(1f, 0.3f).OnComplete(() =>  mainMenuGroup.interactable = true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        HideMainMenu();

        MapRoomSystem.instance.SpawnLevelSelectRoom();
    }
}
