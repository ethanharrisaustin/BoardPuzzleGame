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

            newItem.SetUpAnimationStart(items[i].GetItemUI(), oldPositions[i]);
        }
    }

    public async void MoveBoardItems(UI_ItemBoard.ItemEndPosition[] newPositions, Action onComplete)
    {
        for (int i = 0; i < newPositions.Length; ++i)
        {
            if (pool.GetObject(i) == null) continue;
            
            UI_ItemAnimation newItem = pool.GetObject(i).GetComponent<UI_ItemAnimation>();

            newItem.SetUpAnimationEnd(newPositions);
        }

        await Task.Delay((int)(1000f * UI_ItemAnimation.animationTime));
       
        onComplete.Invoke(); 
    }
}
