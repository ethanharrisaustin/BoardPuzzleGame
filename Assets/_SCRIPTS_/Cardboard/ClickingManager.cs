using UnityEngine;

public class ClickingManager : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    public static ClickingManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    IButton3D previousButton;
    IButton3D currentButton;

    IDragOnto previousDragOnto;
    IDragOnto currentDragOnto;

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
    }

    void GetHoveredButton(RaycastHit hit)
    {
        currentButton = GetButton3DVersion2(hit);

        if (previousButton != currentButton)
        {
            previousButton?.MouseOut();
            currentButton?.MouseOver();
        }

        previousButton = currentButton;
    }

    void GetHoveredDragOnto(RaycastHit hit)
    {
        if (!UI_DraggedItem.IsDraggingItem())
        {
            currentDragOnto?.OnDragUnhover();
            previousDragOnto?.OnDragUnhover();

            previousDragOnto = null;
            currentDragOnto = null;
        }

        currentDragOnto = GetDragOntoVersion2(hit);

        if (previousDragOnto != currentDragOnto)
        {
            previousDragOnto?.OnDragHover(UI_DraggedItem.GetDraggedItem().cardboardItemObject);
            currentDragOnto?.OnDragUnhover();
        }

        previousDragOnto = currentDragOnto;
    }

    RaycastHit HitRaycast()
    {
        Ray ray = CreateRay();

        Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

        return hitInfo;
    }

    Camera cam;
    Camera GetCamera()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        return cam;
    }

    Ray CreateRay()
    {
        return GetCamera().ScreenPointToRay(Input.mousePosition);
    }

    IButton3D GetButton3D(RaycastHit hitInfo)
    {
        if (hitInfo.transform == null)
        {
            return null;
        }

        IButton3D button3D = hitInfo.transform.GetComponentInChildren<IButton3D>(false);

        if (button3D != null && !button3D.enabled) button3D = null;

        if (button3D == null) button3D = hitInfo.transform.GetComponentInParent<IButton3D>(false);

        if (button3D != null && !button3D.enabled) button3D = null;

        return button3D;
    }

    IButton3D GetButton3DVersion2(RaycastHit hitInfo)
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


    IDragOnto GetDragOnto(RaycastHit hitInfo)
    {
        if (hitInfo.transform == null)
        {
            return null;
        }

        IDragOnto dragOnto = hitInfo.transform.GetComponentInChildren<IDragOnto>(false);

        if (dragOnto != null && !dragOnto.enabled) dragOnto = null;

        dragOnto ??= hitInfo.transform.GetComponentInParent<IDragOnto>(false);

        if (dragOnto != null && !dragOnto.enabled) dragOnto = null;

        return dragOnto;
    }

    IDragOnto GetDragOntoVersion2(RaycastHit hitInfo)
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

    IButton3D mouseDownButton = null;
    public void MouseDown()
    {
        mouseDownButton = currentButton;
    }

    public void MouseUp()
    {
        if (mouseDownButton != currentButton) return;
        
        currentButton?.Click();
    }

    public IButton3D HoveredButton()
    {
        return currentButton;
    }

    public IDragOnto HoveredDragOnto()
    {
        return GetDragOnto(HitRaycast());
    }
}  
