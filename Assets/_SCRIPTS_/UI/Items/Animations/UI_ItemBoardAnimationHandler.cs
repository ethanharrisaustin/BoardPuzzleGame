using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cardboard;

public class UI_ItemBoardAnimationHandler : MonoBehaviour
{
    public ObjectPool pool;

    public UI_ItemAnimation scaleInAnimation;

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

    public void ScaleInNewItem(UI_Item_Base itemUI)
    {
        Transform firstSlotPos = UI_ItemBoard.instance.firstSlotPosition;

        UI_ItemAnimation newItem = scaleInAnimation;

        newItem.gameObject.SetActive(true);
        
        newItem.SetUpAnimationStart(itemUI, firstSlotPos.position);

        newItem.DoScaleIn();
    }
}
