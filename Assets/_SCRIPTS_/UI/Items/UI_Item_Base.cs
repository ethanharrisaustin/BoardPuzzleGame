using System.Threading.Tasks;
using Cardboard;
using DG.Tweening;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item_Base : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IItem
{
    public Color[] originalColour { get; private set; }
    Image[] images;

    public static float normalBrightness = 0.8f;
    public static float highlightedBrightness = 1f;
    public static float pressedBrightness = 0.7f;

    protected bool mouseOver { get; private set; }
    protected bool mouseOverForgiving { get; private set; }
    public bool mouseDown;

    Vector2 mouseDownPosition;
    Vector2 prevMousePos;
    Vector2 mouseDownOffset;

    public bool dragging;

    public bool draggingFromItems = false;

    float draggingFromItemsTimer = 0f;
    public bool wasDraggingFromItems { get { return draggingFromItemsTimer > 0f; } }

    float hidingForSeconds = 0f;

    public CardboardItemObject cardboardItemObject;

    public string unique_id 
    { 
        get 
        { 
            if (cardboardItemObject != null) 
            {
                return cardboardItemObject.unique_id; 
            } 
            else
            {
                return "";
            };
        }
    }

    protected virtual void Awake()
    {
        mouseOver = false;
        mouseDown = false;
        dragging = false;
        mouseOverForgiving = false;
        
        images = GetComponentsInChildren<Image>(true);

        originalColour = new Color[images.Length];

        for (int i = 0; i < originalColour.Length; ++i)
        {
            originalColour[i] = images[i].color;
        }

        SetItemBrightness(normalBrightness);
    }

    protected virtual void Update()
    {
        draggingFromItemsTimer -= Time.deltaTime;

        if (draggingFromItems) draggingFromItemsTimer = 0.3f;

        if (hidingForSeconds > 0f)
        {
            Hide();
            hidingForSeconds -= Time.deltaTime;
        }

        Vector2 cMousePos = Input.mousePosition;

        if (cMousePos != prevMousePos)
        {
            OnPointerMove(cMousePos);
        }

        if (dragging)
        {
            FollowMouse();
        }

        prevMousePos = cMousePos;

        if (mouseOver || mouseOverForgiving)
        {
            if (!SomethingDragging()) UI_ItemLabel.instance.ShowLabel(cardboardItemObject.itemName);
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        SetItemBrightness(highlightedBrightness);

        if (!mouseOverForgiving) 
        {
            if (!SomethingDragging()) AudioManager.Play("UI Pluck");
            SetItemBrightness(highlightedBrightness);
        }
    }

    public void MouseEnterForgiving()
    {
        mouseOverForgiving = true;

        if (!mouseOver) 
        {
            SetItemBrightness(highlightedBrightness);

            if (!SomethingDragging()) AudioManager.Play("UI Pluck");
        }
    }

    public void MouseExitForgiving()
    {
        mouseOverForgiving = false;

        if (!mouseOver)
        {
            SetItemBrightness(normalBrightness);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;

        if (!mouseOverForgiving)
            SetItemBrightness(normalBrightness);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        mouseDownPosition = Input.mousePosition;
        mouseDownOffset = (Vector3)Input.mousePosition - transform.position;
        mouseDown = true;
        SetItemBrightness(pressedBrightness);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;

        if (mouseOver)
            SetItemBrightness(highlightedBrightness);
        else
            SetItemBrightness(normalBrightness);
        
        dragging = false;
        draggingFromItems = false;
    }

    public virtual void OnPointerMove(Vector2 mousePosition)
    {
        if (dragging)
        {
            FollowMouse();
            return;
        }
        
        if (mouseDown == false) return;

        const float dragAllowance = -1;

        if (DistanceFromMouseDownPos() > dragAllowance)
        {
            OnPointerStartDrag();
        }
    }

    public virtual void OnPointerStartDrag()
    {
        if (dragging) return;

        dragging = true;
       
        UI_DraggedItem.Get().SetUpDrag(this, mouseDownOffset);

        draggingFromItems = true;

        Hide();
    }

    public virtual void OnPointerEndDrag()
    {
        dragging = false;

        draggingFromItems = false;
    }

    float DistanceFromMouseDownPos()
    {
        return Vector2.Distance(mouseDownPosition, Input.mousePosition);
    }

    void FollowMouse()
    {
        if (draggingFromItems)
        {

            Hide();
        }

        if (!mouseDown)
        {
            OnPointerEndDrag();
            return;
        }
    }

    public virtual void SetItemBrightness(float brightness)
    {
        for (int i = 0; i < originalColour.Length; ++i)
        {
            images[i].color = new Color(
                originalColour[i].r * brightness,
                originalColour[i].g * brightness,
                originalColour[i].b * brightness,
                originalColour[i].a);
        }
    }

    public virtual void SetItemAlpha(float alpha)
    {
        for (int i = 0; i < originalColour.Length; ++i)
        {
            images[i].color = new Color(
                originalColour[i].r,
                originalColour[i].g,
                originalColour[i].b,
                originalColour[i].a * alpha);
        }
    }

    public virtual void Hide()
    {
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].enabled = false;
        }
    }

    public virtual void HideFor(float seconds)
    {
        hidingForSeconds = seconds;
        Hide();
    }

    public virtual void Show()
    {
        hidingForSeconds = 0f;
        
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].enabled = true;
        }
    }

    public virtual void ScaleInShow()
    {
        Show();

        transform.localScale = Vector2.zero;

        transform.DOScale(Vector2.one, 0.3f).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    public Image[] GetImages()
    {
        return images;
    }   

    /// <summary>
    /// Returns true if this matches other
    /// </summary>
    public bool Matching(UI_Item_Base other)
    {
        return unique_id == other.unique_id;
    }

    public virtual bool CancelDrag()
    {
        return true;
    }

    public virtual bool AddToTurnSlot()
    {
        return false;
    }

    public virtual bool AddToCardboardHolder()
    {
        IButton3D hoveredButton = ClickingManager.instance.HoveredButton();

        if (hoveredButton == null)
        {
            return false;
        }

        if (hoveredButton is CardboardHolderGO == false)
        {
            return false;
        }

        CardboardHolderGO cardboardHolder = hoveredButton as CardboardHolderGO;

        return cardboardHolder.AddCardboard(cardboardItemObject);
    }

    public virtual bool DropOntoObject()
    {
        IDragOnto dragOnto = ClickingManager.instance.HoveredDragOnto();

        if (dragOnto == null) return false;

        return dragOnto.OnDropDraggedItem(cardboardItemObject);
    }

    public virtual void OnDrag(UI_DraggedItem draggedItem)
    {
        
    }

    public bool IsHidden()
    {
        return images[0].enabled == false;
    }

    public bool SomethingDragging()
    {
        return dragging || UI_DraggedItem.IsDraggingItem();
    }
}
