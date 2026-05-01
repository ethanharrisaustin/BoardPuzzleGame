using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;
using Cardboard;

public class UI_ItemMoveTo : MonoBehaviour
{
    static UI_ItemMoveTo instance;

    [SerializeField] Image[] images;

    UI_Item_Base goToItem;

    void Awake()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    public static UI_ItemMoveTo Get()
    {
        if (instance == null)
            instance = FindFirstObjectByType<UI_ItemMoveTo>();
        
        return instance;
    }



    void SetUp(IItem draggedItem)
    {
        gameObject.SetActive(true);

        Image[] imagesToCopy = draggedItem.GetImages();

        DisableImages();

        for (int i = 0; i < imagesToCopy.Length; ++i)
        {
            images[i].enabled = imagesToCopy[i].enabled;
            
            CardboardExtras.MatchImageAToB(images[i], imagesToCopy[i]);
        }   

        transform.position = draggedItem.transform.position;
    }

    public void SetUp(UI_DraggedItem draggedItem, Action onComplete = null)
    {
        SetUp(draggedItem as IItem);

        UI_Item_Base goToItem = UI_DraggedItem.GetDraggedItem();

        MoveTo(UI_ItemBoard.GetItemUIPosition(goToItem.unique_id), () =>
        {
            onComplete?.Invoke();

            goToItem.Show();
        });
    }

    public void SetUp(UI_DraggedItem draggedItem, Vector2 moveToPos, Action onComplete = null, float timeTakenMultiplier = 1f)
    {
        SetUp(draggedItem, draggedItem.transform.position, moveToPos, onComplete, timeTakenMultiplier);
    }

    public void SetUp(IItem item, Vector2 startPos, Vector2 moveToPos, Action onComplete = null, float timeTakenMultiplier = 1f)
    {
        SetUp(item);

        transform.position = startPos;

        MoveTo(moveToPos, onComplete, timeTakenMultiplier);
    }

    void DisableImages()
    {
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].enabled = false;
        }
    }

    void MoveTo(Vector2 position, Action onComplete, float timeTakenMultiplier = 1f)
    {
        transform.DOKill(false);

        transform.DOMove(position, 0.4f * timeTakenMultiplier).SetEase(Ease.OutExpo).SetUpdate(false).OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
