using Cardboard;
using DG.Tweening;
using JetBrains.Annotations;
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
    public bool mouseDown;

    Vector2 mouseDownPosition;
    Vector2 prevMousePos;
    Vector2 mouseDownOffset;

    public bool dragging;

    public string unique_id;

    public CardboardItemObject cardboardItemObject;

    protected virtual void Awake()
    {
        mouseOver = false;
        mouseDown = false;
        dragging = false;
        
        images = GetComponentsInChildren<Image>();

        originalColour = new Color[images.Length];

        for (int i = 0; i < originalColour.Length; ++i)
        {
            originalColour[i] = images[i].color;
        }

        SetItemBrightness(normalBrightness);
    }

    protected virtual void Update()
    {
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

        if (mouseOver)
        {
            UI_ItemLabel.instance.ShowLabel(cardboardItemObject.itemName);
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;

        SetItemBrightness(highlightedBrightness);

        AudioManager.Play("UI Pluck");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;

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
    }

    public virtual void OnPointerMove(Vector2 mousePosition)
    {
        if (dragging)
        {
            FollowMouse();
            return;
        }
        
        if (mouseDown == false) return;

        const float dragAllowance = 10;

        if (DistanceFromMouseDownPos() > dragAllowance)
        {
            OnPointerStartDrag();
        }
    }

    public virtual void OnPointerStartDrag()
    {
        dragging = true;

        Hide();

        UI_DraggedItem.Get().SetUpDrag(this, mouseDownOffset);
    }

    public virtual void OnPointerEndDrag()
    {
        dragging = false;
    }

    float DistanceFromMouseDownPos()
    {
        return Vector2.Distance(mouseDownPosition, prevMousePos);
    }

    void FollowMouse()
    {
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

    public virtual void Show()
    {
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
}
