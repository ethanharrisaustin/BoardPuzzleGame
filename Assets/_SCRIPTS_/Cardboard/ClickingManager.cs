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

    // Update is called once per frame
    void LateUpdate()
    {
        GetHoveredButton();

        if (previousButton != currentButton)
        {
            previousButton?.MouseOut();
            currentButton?.MouseOver();
        }

        previousButton = currentButton;
    }

    void GetHoveredButton()
    {
        Ray ray = CreateRay();

        Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

        currentButton = GetButton3D(hitInfo);
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

        button3D ??= hitInfo.transform.GetComponentInParent<IButton3D>(false);

        if (button3D != null && !button3D.enabled) button3D = null;

        return button3D;
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
}  
