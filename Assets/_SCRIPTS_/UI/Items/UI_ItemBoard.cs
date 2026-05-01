using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemBoard : MonoBehaviour
{  
    public static List<CardboardItemObject> items = new List<CardboardItemObject>();

    [SerializeField] Transform firstSlotPosition;
    public Transform itemUiParent;

    public static UI_ItemBoard instance;

    [SerializeField] UI_ItemBoardAnimationHandler animationHandler;
    [SerializeField] CanvasGroup realGrid, animationGrid;


    void Awake()
    {
        items.Clear();

        instance = this;
    }

    public async static void AddItemToBoard(CardboardItemObject cardboardItemObject)
    {
        if (AlreadyInItemBoard(cardboardItemObject)) return;

        UI_ItemMoveTo.Get().SetUp(
            cardboardItemObject, 
            Input.mousePosition,
            instance.firstSlotPosition.position,
            null, 
            1f);

        Vector2[] startPositions = GetItemStartPositions();

        items.Insert(0, cardboardItemObject);

        instance.UpdateUI(startPositions);
    }

    public static bool AlreadyInItemBoard(CardboardItemObject cardboardItemObject)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (cardboardItemObject.unique_id == items[i].unique_id)
            {
                return true;
            }
        }

        return false;
    }

    async void UpdateUI(Vector2[] startPositions)
    {
        animationHandler.MakeBoardItemsStartPos(items, startPositions);

        realGrid.alpha = 0;
        animationGrid.alpha = 1;

        // Actually update them
        RemoveAllItemsUI();

        ShowItemsInOrder();

        GetComponent<RectTransform>().ForceUpdateRectTransforms();

        await Task.Yield();
        await Task.Yield();

        animationHandler.MoveBoardItems(GetItemEndPositions(), () =>
        {
            realGrid.alpha = 1;
            animationGrid.alpha = 0;
        });
    }

    static Vector2[] GetItemStartPositions()
    {
        Vector2[] result = new Vector2[items.Count];
        for (int i = 0; i < items.Count; ++i)
        {
            result[i] = items[i].GetItemUI().transform.position;
        }
        return result;
    }

    static Vector2[] GetItemEndPositions()
    {
        Vector2[] result = new Vector2[items.Count - 1];
        for (int i = 1; i < items.Count; ++i)
        {
            result[i - 1] = items[i].GetItemUI().transform.position;
        }
        return result;
    }

    void RemoveAllItemsUI()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].cached_ui_item == null) continue;

            items[i].cached_ui_item.gameObject.SetActive(false);
            items[i].cached_ui_item.transform.parent = null;
        }
    }

    void ShowItemsInOrder()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            UI_Item_Base itemUI = items[i].GetItemUI();

            itemUI.transform.parent = itemUiParent;
        }
    }

    public static bool GetItemUI(string unique_id, out UI_Item_Base item)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            item = items[i].cached_ui_item;

            if (item == null) continue;

            if (item.unique_id != unique_id) continue;

            return true;
        }

        item = null;
        return false;
    }

    public static Vector2 GetItemUIPosition(string unique_id)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            UI_Item_Base item = items[i].cached_ui_item;

            if (item == null) continue;

            if (item.unique_id != unique_id) continue;

            return item.transform.position;
        }

        return instance.firstSlotPosition.position;
    }
}
