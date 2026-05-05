using Cardboard;
using UnityEngine;
using UnityEngine.UI;


public class UI_DraggedItem : MonoBehaviour, IItem
{
    static UI_DraggedItem instance;

    [SerializeField] Image[] images;

    Vector2 mouseDownOffset;

    UI_Item_Base draggedItem;
 
    void Awake()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    public static UI_DraggedItem Get()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<UI_DraggedItem>();
        }

        return instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowMouse();
    }

    public void SetUpDrag(UI_Item_Base uI_Item_Base, Vector2 mouseDownOffset)
    {
        draggedItem = uI_Item_Base;
        draggedItem.dragging = true;
        draggedItem.mouseDown = true;

        this.mouseDownOffset = mouseDownOffset;

        gameObject.SetActive(true);

        Image[] imagesToCopy = uI_Item_Base.GetImages();

        DisableImages();

        for (int i = 0; i < imagesToCopy.Length; ++i)
        {
            images[i].enabled = true;
            
            CardboardExtras.MatchImageAToB(images[i], imagesToCopy[i]);
        }   

        transform.position = Input.mousePosition;
    }

    public void MouseUp()
    {
        draggedItem?.OnPointerUp(null);
    }

    void DisableImages()
    {
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].enabled = false;
        }
    }

    void FollowMouse()
    {
        if (!draggedItem.dragging)
        {
            OnStopDragging();

            return;
        }

        transform.position = Input.mousePosition - mouseDownOffset;

        draggedItem.OnDrag(this);
    }

    void OnStopDragging()
    {
        if (draggedItem.AddToCardboardHolder())
        {
            AddToCardboardHolder();
        }
        // Drop item back to item board
        else if (draggedItem.CancelDrag())
        {
            CancelDrag();
        } 
        // Use the item!
        else if (draggedItem.AddToTurnSlot())
        {
            AddToTurnSlot();
        }

        StopFollowingMouse();
    }

    void CancelDrag()
    {
        UI_ItemMoveTo.Get().SetUp(this);
    }

    void AddToTurnSlot()
    {
        UI_TurnSlot turnSlot = UI_TurnSlot.hoveredSlot;

        if (turnSlot == null) turnSlot = UI_TurnSlot.cardRectHoveredSlot;

        UI_Item_Base goToItem = GetDraggedItem();

        UI_ItemBoard.GetItemUI(goToItem.unique_id, out UI_Item_Base item);

        if (item.IsHidden()) item.ScaleInShow();

        UI_Card_Base card = draggedItem as UI_Card_Base;

        UI_ItemMoveTo.Get().SetUp(this, turnSlot.transform.position, () =>
        {
            turnSlot.AddCard(card);
        }, 0.5f);
    }

    void AddToCardboardHolder()
    {
        // NOTE - actual add to cardboard holder logic in UI_Item_Base.cs

        UI_Item_Base goToItem = GetDraggedItem();

        UI_ItemBoard.GetItemUI(goToItem.unique_id, out UI_Item_Base item);

        if (item.IsHidden()) item.ScaleInShow();
    }

    void StopFollowingMouse()
    {
        gameObject.SetActive(false);
        draggedItem = null;
    }

    public Image[] GetImages()
    {
        return images;
    }

    public static UI_Item_Base GetDraggedItem()
    {
        return Get().draggedItem;
    }

    public static T GetDraggedItem<T>() where T : UI_Item_Base
    {
        if (Get().draggedItem is T)
        {
            return Get().draggedItem as T;
        }

        return null;
    }

    public static bool IsDraggingItem<T>() where T : UI_Item_Base
    {
        if (Get().draggedItem == null) return false;

        if (Get().draggedItem is T) return true;

        return false;
    }

    public static bool IsDraggingItem()
    {
        if (Get().draggedItem == null) return false;
        
        return true;
    }
}
