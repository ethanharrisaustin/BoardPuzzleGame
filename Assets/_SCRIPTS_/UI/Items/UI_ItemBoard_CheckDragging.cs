using UnityEngine;

public class UI_ItemBoard_CheckDragging : MonoBehaviour
{
    [SerializeField] bool isOverlapping;
    [SerializeField] float distanceAllowenceX = 0.1f;
    [SerializeField] float distanceAllowenceY = 0.1f;
    [SerializeField] RectTransform mousePosRect;
    [SerializeField] RectTransform background;

    bool mouseDown = false;
    Vector2 mouseDownPos;

    public static UI_ItemBoard_CheckDragging instance;

    UI_Item_Base itemBeingDragged;
    UI_Item_Base overlappedItem;
    UI_Item_Base previousOverlappedItem;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        isOverlapping = RectExtensions.IsOverlapping(background, mousePosRect);
    }

    // Called from Input.cs
    public void MouseDown()
    {
        if (!isOverlapping) return;

        mouseDown = true;
        mouseDownPos = Input.mousePosition;

        if (overlappedItem != null)
        {
            overlappedItem.OnPointerDown(null);
        }
    }

    // Called from Input.cs
    public void MouseUp()
    {
        mouseDown = false;

        if (itemBeingDragged != null)
        {
            itemBeingDragged.OnPointerUp(null);
            itemBeingDragged = null;
        }
        else if (overlappedItem != null)
        {
            overlappedItem.OnPointerUp(null);
        }
    }
    
    // Called from Input.cs
    public void MouseMove()
    {
        overlappedItem = OverlappedItem();

        if (previousOverlappedItem != overlappedItem)
        {
            if (previousOverlappedItem != null) previousOverlappedItem.MouseExitForgiving();
            if (overlappedItem != null) overlappedItem.MouseEnterForgiving();

            previousOverlappedItem = overlappedItem;
        }

        if (isOverlapping == false) mouseDown = false;

        if (!mouseDown) return;

        mouseDown = false;

        if (UI_DraggedItem.IsDraggingItem()) return;

        if (Vector2.Distance(Input.mousePosition, mouseDownPos) > 0 && overlappedItem != null)
        {
            itemBeingDragged = overlappedItem;
            itemBeingDragged.OnPointerDown(null);
            itemBeingDragged.OnPointerStartDrag();

            overlappedItem = null;
            previousOverlappedItem = null;
        }
    }

    UI_Item_Base OverlappedItem()
    {
        UI_Item_Base itemUI = UI_ItemBoard.ClosestItemToMouse();

        if (itemUI == null) return null;

        float distanceX = Mathf.Abs(itemUI.transform.position.x - mousePosRect.position.x);
        float distanceY = Mathf.Abs(itemUI.transform.position.y - mousePosRect.position.y);

        if (distanceX > Screen.width * distanceAllowenceX
            ||
            distanceY > Screen.height * distanceAllowenceY) return null;


        return itemUI;
    }
}
