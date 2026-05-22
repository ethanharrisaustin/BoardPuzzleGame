using UnityEngine;
using UnityEngine.EventSystems;

public class UI_RaycastCatcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool mouseOver = false;

    #if UNITY_EDITOR

    public bool _mouseOver;

    void Update()
    {
        _mouseOver = mouseOver;
    }

    #endif

    void Awake()
    {
        mouseOver = false;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        mouseOver = false;
    }
}
