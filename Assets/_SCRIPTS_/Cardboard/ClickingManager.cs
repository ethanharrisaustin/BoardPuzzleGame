using Unity.VisualScripting;
using UnityEngine;

public class ClickingManager : MonoBehaviour
{
    #region  Variables 

    [SerializeField] Camera cam;
    [SerializeField] LayerMask layerMask;

    public static ClickingManager instance;

    IButton3D previousButton;
    IButton3D currentButton;
    IButton3D mouseDownButton = null;

    IDragOnto previousDragOnto;
    IDragOnto currentDragOnto;

    IDraggable3D previousDraggable;
    IDraggable3D currentDraggable;
    

    #endregion

    #region Monobehaviour Functions

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (UI_CompletionMenu.isOpen || !UI_RaycastCatcher.mouseOver)
        {
            previousButton = null;
            currentButton = null;
            previousDragOnto = null;
            currentDragOnto = null;

            return;
        }
        
        RaycastHit hit = HitRaycast();

        GetHoveredButton(hit);
        GetHoveredDragOnto(hit);
        GetHoveredDraggable(hit);
    }

    #endregion

    #region Button 3D

    void GetHoveredButton(RaycastHit hit)
    {
        currentButton = GetButton3D(hit);

        if (previousButton != currentButton)
        {
            previousButton?.MouseOut();
            currentButton?.MouseOver();
        }

        previousButton = currentButton;
    }

    IButton3D GetButton3D(RaycastHit hitInfo)
    {
        if (hitInfo.transform == null)
        {
            return null;
        }

        IButton3D[] button3Ds = hitInfo.transform.GetComponentsInChildren<IButton3D>(false);

        for (int i = 0; i < button3Ds.Length; ++i)
        {
            if (button3Ds[i].enabled) return button3Ds[i];
        }
        

        button3Ds = hitInfo.transform.GetComponentsInParent<IButton3D>(false);

        for (int i = 0; i < button3Ds.Length; ++i)
        {
            if (button3Ds[i].enabled) return button3Ds[i];
        }
        
        return null;
    }

    public IButton3D HoveredButton()
    {
        return currentButton;
    }

    void MouseDownOnButton()
    {
         mouseDownButton = currentButton;
    }

    void MouseUpOnButton()
    {
        if (mouseDownButton != currentButton) return;
        
        currentButton?.Click();
    }


    #endregion

    #region Drag Onto

    void GetHoveredDragOnto(RaycastHit hit)
    {
        if (!UI_DraggedItem.IsDraggingItem())
        {
            currentDragOnto?.OnDragUnhover();
            previousDragOnto?.OnDragUnhover();

            previousDragOnto = null;
            currentDragOnto = null;

            return;
        }

        currentDragOnto = GetDragOnto(hit);

        if (previousDragOnto != currentDragOnto)
        {
            previousDragOnto?.OnDragHover(UI_DraggedItem.GetDraggedItem().cardboardItemObject);
            currentDragOnto?.OnDragUnhover();
        }

        previousDragOnto = currentDragOnto;
    }

    IDragOnto GetDragOnto(RaycastHit hitInfo)
    {
        IDragOnto dragOnto = GetDragUsingRaycastHit(hitInfo);
        if (dragOnto != null) return dragOnto;

        UI_DraggedItem.SetUpImageBoundaries();

        dragOnto = GetDragOntoUpperBoundary();
        if (dragOnto != null) return dragOnto;

        dragOnto = GetDragOntoLowerBoundary();
        if (dragOnto != null) return dragOnto;

        dragOnto = GetDragOntoLeftBoundary();
        if (dragOnto != null) return dragOnto;

        dragOnto = GetDragOntoRightBoundary();
        if (dragOnto != null) return dragOnto;

        return null;
    }

    IDragOnto GetDragUsingRaycastHit(RaycastHit hitInfo)
    {
        if (hitInfo.transform == null)
        {
            return null;
        }

        IDragOnto[] dragOntos = hitInfo.transform.GetComponentsInChildren<IDragOnto>(false);

        for (int i = 0; i < dragOntos.Length; ++i)
        {
            if (dragOntos[i].enabled) return dragOntos[i];
        }

        dragOntos = hitInfo.transform.GetComponentsInParent<IDragOnto>(false);

        for (int i = 0; i < dragOntos.Length; ++i)
        {
            if (dragOntos[i].enabled) return dragOntos[i];
        }

        return null;
    }

    IDragOnto GetDragOntoUpperBoundary()
    {
        RaycastHit hit = HitRaycast(CreateRay(UI_DraggedItem.UpperBoundary()));

        return GetDragUsingRaycastHit(hit);
    }

    IDragOnto GetDragOntoLowerBoundary()
    {
        RaycastHit hit = HitRaycast(CreateRay(UI_DraggedItem.LowerBoundary()));

        return GetDragUsingRaycastHit(hit);
    }

    IDragOnto GetDragOntoLeftBoundary()
    {
        RaycastHit hit = HitRaycast(CreateRay(UI_DraggedItem.LeftBoundary()));

        return GetDragUsingRaycastHit(hit);
    }

    IDragOnto GetDragOntoRightBoundary()
    {
        RaycastHit hit = HitRaycast(CreateRay(UI_DraggedItem.RightBoundary()));

        return GetDragUsingRaycastHit(hit);
    }

    public IDragOnto HoveredDragOnto()
    {
        return GetDragOnto(HitRaycast());
    }

    

    Ray CreateRay(Vector3 screenPosition)
    {
        Ray ray = GetCamera().ScreenPointToRay(screenPosition);

        return ray;
    }

    #endregion

    #region  Draggable

    void GetHoveredDraggable(RaycastHit hit)
    {
        if (UI_DraggedItem.IsDraggingItem())
        {
            currentDraggable?.MouseUp();
            previousDraggable?.MouseUp();

            currentDraggable = null;
            previousDraggable = null;

            return;
        }

        currentDraggable = GetDraggable3D(hit);

        if (previousDraggable != currentDraggable)
        {
            previousDraggable?.MouseUp();
        }

        previousDraggable = currentDraggable;
    }

    IDraggable3D GetDraggable3D(RaycastHit hitInfo)
    {
        if (hitInfo.transform == null)
        {
            return null;
        }

        IDraggable3D[] draggables = hitInfo.transform.GetComponentsInChildren<IDraggable3D>(false);

        for (int i = 0; i < draggables.Length; ++i)
        {
            if (draggables[i].enabled) return draggables[i];
        }

        draggables = hitInfo.transform.GetComponentsInParent<IDraggable3D>(false);

        for (int i = 0; i < draggables.Length; ++i)
        {
            if (draggables[i].enabled) return draggables[i];
        }

        return null;
    }

    Vector2 mouseDownPosition;
    bool mouseDownOnDraggable = false;
    IDraggable3D currentDragging;

    void MouseDownOnDraggable()
    {
        if (currentDraggable == null) 
        {
            mouseDownOnDraggable = false;
            return;
        }

        currentDraggable.MouseDown();

        mouseDownPosition = Input.mousePosition;

        mouseDownOnDraggable = true;
    }

    void MouseUpOnDraggable()
    {
        if (currentDragging != null)
        {
            currentDragging.EndDrag(Input.mousePosition);
            currentDragging = null;
        }

        mouseDownOnDraggable = false;

        if (currentDraggable == null) return;

        currentDraggable.MouseUp();
    }

    void MouseMoveOnDraggable(Vector2 mousePos)
    {
        if (currentDraggable == null) 
        {
            mouseDownOnDraggable = false;
            return;
        }

        if (!mouseDownOnDraggable) return;
        
        float mouseMoveDist = Vector2.Distance(mouseDownPosition, mousePos);

        if (mouseMoveDist > 2)
        {
            currentDraggable.StartDrag(mousePos);

            currentDragging = currentDraggable;
        }
    }

    #endregion

    #region All Interactables

    RaycastHit HitRaycast()
    {
        return HitRaycast(CreateRay());
    }

    RaycastHit HitRaycast(Ray ray)
    {
        Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

        return hitInfo;
    }

    Camera GetCamera()
    {
        return cam;
    }

    Ray CreateRay()
    {
        return GetCamera().ScreenPointToRay(Input.mousePosition);
    }

    #endregion   

    #region Mouse Events

    public void MouseDown()
    {
        MouseDownOnButton();
        MouseDownOnDraggable();
    }

    public void MouseUp()
    {
        MouseUpOnButton();
        MouseUpOnDraggable();
    }

    public void MouseMove(Vector2 _, Vector2 currentPosition)
    {
        MouseMoveOnDraggable(currentPosition);
    }

    #endregion    
}  
