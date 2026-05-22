using UnityEngine;

public class UI_CamMoveButtonsHandler : MonoBehaviour
{
    [SerializeField] CameraPositionTracker positionTracker;

    [SerializeField] UI_CamMoveButton leftBtn, rightBtn, upBtn, downBtn;

    public Color normalColour, overColour, downColour, inactiveColour;
    public float fadeDuration = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetButtonsActive();
    }

    void SetButtonsActive()
    {
        leftBtn.inactive = !positionTracker.CanHoverLeft();
        rightBtn.inactive = !positionTracker.CanHoverRight();
        upBtn.inactive = !positionTracker.CanHoverUp();
        downBtn.inactive = !positionTracker.CanHoverDown();
    }
}
