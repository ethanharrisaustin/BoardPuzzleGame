using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CamMoveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent mouseDown;

    

    Image[] graphic;

    public bool inactive = false;
    bool previouslyInactive;

    bool mouseIsDown = false;
    bool mouseIsOver = false;

    UI_CamMoveButtonsHandler handler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handler = GetComponentInParent<UI_CamMoveButtonsHandler>();
        graphic = GetComponentsInChildren<Image>();

        OnPointerExit(null);

        previouslyInactive = inactive;
    }

    // Update is called once per frame
    void Update()
    {
        if (previouslyInactive != inactive)
        {
            if (inactive) GoToInactive();
            else OnPointerExit(null);

            previouslyInactive = inactive;
        }

        if (mouseIsDown)
        {
            mouseDown.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseIsOver = true;

        if (inactive) { GoToInactive(); return; }

        GraphicTo(handler.overColour);
    }

    public void OnPointerExit(PointerEventData data)
    {
        mouseIsOver = false;

        if (inactive) { GoToInactive(); return; }

        GraphicTo(handler.normalColour);

        mouseIsDown = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (inactive) { GoToInactive(); return; }

        GraphicTo(handler.downColour);

        mouseIsDown = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (inactive) { GoToInactive(); return; }

        GraphicTo(mouseIsOver ? handler.overColour : handler.normalColour);

        mouseIsDown = false;
    }

    void GoToInactive()
    {
        GraphicTo(handler.inactiveColour);

        mouseIsDown = false;
    }

    void GraphicTo(Color colour)
    {
        for (int i = 0; i < graphic.Length; ++i)
        {
            graphic[i].DOKill(false);
            graphic[i].DOColor(colour, handler.fadeDuration);   
        }
    }
}
