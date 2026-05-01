using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UI_ItemBoardAnimationHandler : MonoBehaviour
{
    public ObjectPool pool;

    public void MakeBoardItemsStartPos(List<CardboardItemObject> items, Vector2[] oldPositions)
    {
        pool.DestroyAll();

        for (int i = 0; i < oldPositions.Length; ++i)
        {
            UI_ItemAnimation newItem = pool.SpawnObject().GetComponent<UI_ItemAnimation>();

            newItem.SetUpAnimationStart(items[i + 1].GetItemUI(), oldPositions[i]);
        }
    }

    public async void MoveBoardItems(Vector2[] newPositions, Action onComplete)
    {
        pool.DestroyAll();

        for (int i = 0; i < newPositions.Length; ++i)
        {
            UI_ItemAnimation newItem = pool.SpawnObject().GetComponent<UI_ItemAnimation>();

            newItem.SetUpAnimationEnd(newPositions[i]);
        }

        await Task.Delay((int)(1000f * UI_ItemAnimation.animationTime));
       
        onComplete.Invoke(); 
    }
}
