using System;
using System.Threading.Tasks;
using Cardboard;
using DG.Tweening;
using UnityEngine;

public class UI_ItemAnimation : MonoBehaviour
{
   public static float animationTime = 0.3f;

   CardboardItemObject refToItem;

    public void SetUpAnimationStart(UI_Item_Base item, Vector2 startPosition)
    {
        transform.position = startPosition;

        refToItem = item.cardboardItemObject;

        CardboardExtras.MatchImageAToB(transform, item.transform);
    }

    public void SetUpAnimationEnd(Vector2 endPosition)
    {
        transform.DOKill(false);
        transform.DOMove(endPosition, animationTime).SetEase(Ease.OutQuad).SetUpdate(true);
    }
    public void SetUpAnimationEnd(UI_ItemBoard.ItemEndPosition[] newPositions)
    {
        for (int i = 0; i < newPositions.Length; ++i)
        {
            if (newPositions[i].item.unique_id != refToItem.unique_id) continue;

            transform.DOMove(newPositions[i].endPosition, animationTime).SetEase(Ease.OutQuad).SetUpdate(true);
            break;
        }
    }

    public void DoScaleIn()
    {
        transform.localScale = Vector3.zero;

        transform.DOKill(false);
        transform.DOScale(1f, animationTime).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(DeactivateInThreeFrames);
    }

    void DeactivateInThreeFrames()
    {
        DeactivateInThreeFramesAsync();
    }

    async void DeactivateInThreeFramesAsync()
    {
        for (int i = 0; i < 4; ++i)
            await Task.Yield();
        
        gameObject.SetActive(false);
    }
}