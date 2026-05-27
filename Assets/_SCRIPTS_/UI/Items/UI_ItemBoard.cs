using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cardboard;

public class UI_ItemBoard : MonoBehaviour
{  
    public static List<CardboardItemObject> items = new List<CardboardItemObject>();

    public Transform firstSlotPosition;
    public Transform itemUiParent;

    public static UI_ItemBoard instance;

    [SerializeField] UI_ItemBoardAnimationHandler animationHandler;
    [SerializeField] CanvasGroup realGrid, animationGrid;


    void Awake()
    {
        items.Clear();

        instance = this;
    }

    public static void ClearItems()
    {
        items.Clear();

        instance.UpdateUI();
    }

    public static void AddItemToBoard(CardboardItemObject cardboardItemObject)
    {
        AudioManager.Play("Item Collect");

        if (AlreadyInItemBoard(cardboardItemObject)) 
        {
            JustDoGoToAnim(cardboardItemObject);
            return;
        }

        items.Insert(0, cardboardItemObject);

        instance.UpdateUI();

        UI_ItemMoveTo.Get().SetUp(
            cardboardItemObject, 
            Input.mousePosition,
            instance.firstSlotPosition.position,
            null, 
            1f);
    }

    public static void AddItemToBoardWithScaleAnimation(CardboardItemObject cardboardItemObject)
    {
        if (GetItemUI(cardboardItemObject.unique_id, out UI_Item_Base item))
        {
            if (item.IsHidden()) item.ScaleInShow();

            return;
        }

        items.Insert(0, cardboardItemObject);

        UI_Item_Base itemUI = cardboardItemObject.GetItemUI();

        itemUI.gameObject.SetActive(false);
        itemUI.transform.parent = null;

        instance.animationHandler.ScaleInNewItem(itemUI);

        instance.UpdateUI();
    }

    public static void AddItemToBoardWithoutNotify(CardboardItemObject cardboardItemObject)
    {
        if (AlreadyInItemBoard(cardboardItemObject)) return;

        items.Insert(0, cardboardItemObject);
    }

    public static void StartDraggingItem(CardboardItemObject cardboardItemObject)
    {
        bool needsToHide = !AlreadyInItemBoard(cardboardItemObject);

        UI_Item_Base item = cardboardItemObject.GetItemUI();

        if (needsToHide)
        {
            item.transform.parent = null;
            item.gameObject.SetActive(false);
        }

        UI_DraggedItem.Get().SetUpDrag(item, Vector2.zero);
    }

    public static void StopDraggingItem(CardboardItemObject cardboardItemObject)
    {
        bool addedToDragOnto = false;

        if (UI_DraggedItem.Get().draggedItem != null)
            UI_DraggedItem.Get().ForceStopDraggingItem(out addedToDragOnto);
        
        if (!addedToDragOnto) AddItemToBoard(cardboardItemObject);
    }

    static void JustDoGoToAnim(CardboardItemObject cardboardItemObject)
    {
        if (!GetItemUI(cardboardItemObject.unique_id, out UI_Item_Base item)) return;

        UI_ItemMoveTo.Get().SetUp(
            cardboardItemObject, 
            Input.mousePosition,
            item.transform.position,
            null, 
            1f);
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

    public static void ShowAndResetItem(string unique_id)
    {
        if (!GetItemUI(unique_id, out var item)) return;

        item.Show();
        item.dragging = false;
        item.mouseDown = false;
    }

    public class ItemEndPosition
    {
        public Vector2 endPosition;
        public CardboardItemObject item;

        public ItemEndPosition(CardboardItemObject item, Vector2 endPosition)
        {
            this.endPosition = endPosition;
            this.item = item;
        }
    }   

    List<CardboardItemObject> oldListOfItems = new List<CardboardItemObject>();
    async void UpdateUI()
    {
        GetOldListOfItems();

        animationHandler.MakeBoardItemsStartPos(oldListOfItems, MakeStartPositions(oldListOfItems));

        realGrid.alpha = 0;
        animationGrid.alpha = 1;

        // Actually update them
        RemoveAllItemsUI();
        ShowItemsInOrder();

        // Let grid layout update
        GetComponent<RectTransform>().ForceUpdateRectTransforms();

        await Task.Yield();
        await Task.Yield();

        // Do the movement animation
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

    List<CardboardItemObject> GetOldListOfItems()
    {
        oldListOfItems.Clear();

        UI_Item_Base[] uI_Item_Bases = GetComponentsInChildren<UI_Item_Base>(false);

        for (int i = 0; i < uI_Item_Bases.Length; ++i)
        {
            UI_Item_Base itemUI = uI_Item_Bases[i];

            oldListOfItems.Add(itemUI.cardboardItemObject);
        }

        return oldListOfItems;
    }

    Vector2[] MakeStartPositions(List<CardboardItemObject> oldItems)
    {
        Vector2[] returnValue = new Vector2[oldItems.Count];

        for (int i = 0; i < returnValue.Length; ++i)
        {
            returnValue[i] = oldItems[i].GetItemUI().transform.position;
        }
        
        return returnValue;
    }

    static ItemEndPosition[] GetItemEndPositions()
    {
        ItemEndPosition[] result = new ItemEndPosition[items.Count];
        for (int i = 0; i < items.Count; ++i)
        {
            result[i] = new ItemEndPosition(items[i], items[i].GetItemUI().transform.position);
        }
        return result;
    }

    void RemoveAllItemsUI()
    {
        UI_Item_Base[] uI_Item_Bases = GetComponentsInChildren<UI_Item_Base>(false);

        for (int i = 0; i < uI_Item_Bases.Length; ++i)
        {
            UI_Item_Base itemUI = uI_Item_Bases[i];

            itemUI.gameObject.SetActive(false);
            itemUI.transform.parent = null;
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

    public static int RemoveFromItemBoard(string unique_id)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (unique_id != items[i].unique_id) continue;
            
            if (items[i].cached_ui_item != null)
            {
                items[i].cached_ui_item.gameObject.SetActive(false);
                items[i].cached_ui_item.transform.parent = null;
            }
            
            items.RemoveAt(i);

            instance.UpdateUI();

            return i;
        }

        return 0;
    }

    public static UI_Item_Base ClosestItemToMouse()
    {
        Vector2 mousePos = Input.mousePosition;

        instance.GetOldListOfItems();

        float currentDistance;
        float smallestDistance = Mathf.Infinity;
        CardboardItemObject closestItem = null;
        for (int i = 0; i < instance.oldListOfItems.Count; ++i)
        {
            currentDistance = Vector2.Distance(mousePos, instance.oldListOfItems[i].transform.position);

            if (currentDistance < smallestDistance)
            {
                smallestDistance = currentDistance;
                closestItem = instance.oldListOfItems[i];
            }
        }

        if (closestItem == null) return null;

        return closestItem.GetItemUI();
    }
}

