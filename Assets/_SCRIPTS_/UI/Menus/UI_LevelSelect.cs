using DG.Tweening;
using MapRooms;
using TMPro;
using UnityEngine;

public class UI_LevelSelect : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] TMP_Text selectedLevelTxt;
    [SerializeField] TMP_Text completedTxt;

    static UI_LevelSelect instance;

    void Awake()
    {
        instance = this;
    }

    public static UI_LevelSelect Get()
    {   
        if (instance == null) instance = FindFirstObjectByType<UI_LevelSelect>(FindObjectsInactive.Include);
        return instance;
    }

    void OnEnable()
    {
        canvasGroup.alpha = 0f;
    }

    public void ShowLevel(LevelNodeGO levelNodeGO)
    {
        canvasGroup.DOKill(false);
        canvasGroup.DOFade(1f, 0.3f);

        if (levelNodeGO.isBonusLevel)
        {
            selectedLevelTxt.text = "Bonus Level " + levelNodeGO.level_id;
        }
        else
        {
            selectedLevelTxt.text = "Level " + levelNodeGO.level_id;
        }

        if (levelNodeGO.Completed())
        {
            completedTxt.text = "COMPLETED";
        }
        else if (levelNodeGO.Unlocked())
        {
            completedTxt.text = "NOT COMPLETED";
        }
        else
        {
            completedTxt.text = "LOCKED";
        }
    }
}
