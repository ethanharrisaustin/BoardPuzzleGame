using Cardboard;
using UnityEngine;
using UnityEngine.UI;
using MoveItMoveIt;


public class UI_DraggedItem : MonoBehaviour, IItem
{
    static UI_DraggedItem instance;

    public static Vector3 position { get { return instance.transform.position; } }

    [SerializeField] Image[] images;

    Vector2 mouseDownOffset;

    public UI_Item_Base draggedItem;

    public string unique_id 
    { 
        get 
        { 
            if (draggedItem != null) 
            {
                return draggedItem.unique_id; 
            } 
            else
            {
                return "";
            };
        }
    }
 
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
        if (draggedItem != null)
            draggedItem.OnPointerUp(null);
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
        CheckToOpenCardboardHolderPlayerTurns();
        
        if (UI_CompletionMenu.isOpen)
        {
            if (draggedItem != null) draggedItem.dragging = false;
            CancelDrag();
            return;
        }

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
        draggedItem.draggingFromItems = false;
        draggedItem.dragging = false;

        if (draggedItem.AddToTurnSlot())
        {
            AddToTurnSlot();
        }
        else if (draggedItem.DropOntoObject())
        {
            EndDragAndScaleItemBackIn();
        }
        // Drop item back to item board
        else if (draggedItem.CancelDrag())
        {
            CancelDrag();
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

        UI_ItemBoard.AddItemToBoardWithScaleAnimation(goToItem.cardboardItemObject);

        UI_Card_Base card = draggedItem as UI_Card_Base;

        UI_ItemMoveTo.Get().SetUp(this, turnSlot.transform.position, () =>
        {
            turnSlot.AddCard(card);
        }, 0.5f);
    }

    void EndDragAndScaleItemBackIn()
    {
        // NOTE - actual add to cardboard holder logic in UI_Item_Base.cs

        UI_Item_Base goToItem = GetDraggedItem();

        if (goToItem.cardboardItemObject.UseOnce())
        {
            UI_ItemBoard.RemoveFromItemBoard(goToItem.unique_id);
        }
        else
        {
            UI_ItemBoard.AddItemToBoardWithScaleAnimation(draggedItem.cardboardItemObject);   
        }
    }

    void StopFollowingMouse()
    {
        gameObject.SetActive(false);
        draggedItem = null;
    }

    bool CheckToOpenCardboardHolderPlayerTurns()
    {
        if (draggedItem is not UI_Card_Movement) return false;

        IDragOnto dragOnto = ClickingManager.instance.HoveredDragOnto();

        if (dragOnto is not CardboardHolderGO) return false;

        CardboardHolderGO hoveredCardboardHolder = dragOnto as CardboardHolderGO;

        if (!hoveredCardboardHolder.ContainsPlayerCharacter()) return false;

        if (UI_PlayerTurnBoard.AlreadyShowing(hoveredCardboardHolder)) return false;

        UI_PlayerTurnBoard.Show(hoveredCardboardHolder);
        
        return true;
    }

    public void ForceStopDraggingItem(out bool addedToDragOnto)
    {
        addedToDragOnto = false;

        if (draggedItem.AddToTurnSlot())
        {
            AddToTurnSlot();
        }
        else if (draggedItem.DropOntoObject())
        {
            EndDragAndScaleItemBackIn();

            addedToDragOnto = true;
        }
        // Drop item back to item board
        else if (draggedItem.CancelDrag())
        {
            //CancelDrag();
        } 

        StopFollowingMouse();
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

    static Rect[] imageRects;
    public static void SetUpImageBoundaries()
    {
        imageRects = new Rect[NumberImagesUsed()];

        for (int i = 0; i < imageRects.Length; ++i)
        {
            imageRects[i] = instance.images[i].rectTransform.rect;
        }
    }

    public static Vector2 UpperBoundary()
    {
        Vector3 highestUpperBoundary = Vector2.zero;

        for (int i = 0; i < imageRects.Length; ++i)
        {
            if (imageRects[i].yMax <= highestUpperBoundary.y) continue;

            highestUpperBoundary.y = imageRects[i].yMax;
        }

        return position + highestUpperBoundary;
    }

    public static Vector2 LowerBoundary()
    {
        Vector3 lowestBoundary = Vector2.zero;

        for (int i = 0; i < imageRects.Length; ++i)
        {
            if (imageRects[i].yMin >= lowestBoundary.y) continue;

            lowestBoundary.y = imageRects[i].yMin;
        }

        return position + lowestBoundary;
    }

    public static Vector2 LeftBoundary()
    {
        Vector3 leftestBoundary = Vector2.zero;

        for (int i = 0; i < imageRects.Length; ++i)
        {
            if (imageRects[i].xMin >= leftestBoundary.x) continue;

            leftestBoundary.x = imageRects[i].xMin;
        }

        return position + leftestBoundary;
    }

    public static Vector2 RightBoundary()
    {
        Vector3 rigtestBoundary = Vector2.zero;

        for (int i = 0; i < imageRects.Length; ++i)
        {
            if (imageRects[i].xMax <= rigtestBoundary.x) continue;

            rigtestBoundary.x = imageRects[i].xMax;
        }

        return position + rigtestBoundary;
    }

    static int NumberImagesUsed()
    {
        for (int i = 0; i < instance.images.Length; ++i)
        {
            if (instance.images[i].enabled == false) return i;
        }

        return instance.images.Length;
    }
}
