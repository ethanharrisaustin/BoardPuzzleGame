using System;
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
}