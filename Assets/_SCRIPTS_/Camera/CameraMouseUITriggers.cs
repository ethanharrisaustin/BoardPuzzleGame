using MapRooms;
using UnityEngine;

public class CameraMouseUITriggers : MonoBehaviour
{
    [SerializeField] CameraPositionTracker positionTracker;

    [SerializeField] RectTransform mousePos;
    [SerializeField] RectTransform leftBarrier;
    [SerializeField] RectTransform rightBarrier;
    [SerializeField] RectTransform upBarrier;
    [SerializeField] RectTransform downBarrier;

    [Space]

    [SerializeField] float atBottomRightTimeThreshold = 0.2f;

    [Space]

    [SerializeField] Transform cameraPositionTrackerParent;

    public static CameraMouseUITriggers instance;

    bool mouseAtRight, mouseAtLeft, mouseAtUp, mouseAtDown;

    float mouseAtRightTimer, mouseAtDownTimer = 0f;

    public bool MouseAtRight { get { return mouseAtRight /*&& Reached(mouseAtRightTimer) */&& positionTracker.CanHoverRight(); } }
    public bool MouseAtLeft { get { return mouseAtLeft && positionTracker.CanHoverLeft(); } }
    public bool MouseAtDown { get { return mouseAtDown && Reached(mouseAtDownTimer) && positionTracker.CanHoverDown(); } }
    public bool MouseAtUp { get { return mouseAtUp && positionTracker.CanHoverUp(); } }

    bool Reached(float timer)
    {
        if (mouseDelta == Vector2.zero && mouseAt0Timer > 0.05f) return true;

        return timer > atBottomRightTimeThreshold;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    Vector2 mouseDelta;
    float mouseAt0Timer = 0f;

    // Update is called once per frame
    void LateUpdate()
    {
        mouseDelta = (Vector2)mousePos.position - Input.mousePosition;
        mousePos.position = Input.mousePosition;

        if (mouseDelta == Vector2.zero)
        {
            mouseAt0Timer += Time.deltaTime;
        }
        else
        {
            mouseAt0Timer = 0f;
        }

        SetMousePositions();
        SetTimers();
    }

    void SetMousePositions()
    {
        mouseAtRight = RectExtensions.IsOverlapping(mousePos, rightBarrier);
        mouseAtLeft = RectExtensions.IsOverlapping(mousePos, leftBarrier);
        mouseAtUp = RectExtensions.IsOverlapping(mousePos, upBarrier);
        mouseAtDown = RectExtensions.IsOverlapping(mousePos, downBarrier);
    }

    void SetTimers()
    {
        mouseAtRightTimer = mouseAtRight ? mouseAtRightTimer + Time.deltaTime : 0f;
        mouseAtDownTimer = mouseAtDown ? mouseAtDownTimer + Time.deltaTime : 0f;
    }

}
