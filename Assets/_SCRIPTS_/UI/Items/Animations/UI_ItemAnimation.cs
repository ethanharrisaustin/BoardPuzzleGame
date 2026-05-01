using System;
using Cardboard;
using DG.Tweening;
using UnityEngine;

public class UI_ItemAnimation : MonoBehaviour
{
   public static float animationTime = 0.3f;

    public void SetUpAnimationStart(UI_Item_Base item, Vector2 startPosition)
    {
        transform.position = startPosition;

        CardboardExtras.MatchImageAToB(transform, item.transform);
    }

    public void SetUpAnimationEnd(Vector2 endPosition)
    {
        transform.DOMove(endPosition, animationTime).SetEase(Ease.OutQuad).SetUpdate(true);
    }
}