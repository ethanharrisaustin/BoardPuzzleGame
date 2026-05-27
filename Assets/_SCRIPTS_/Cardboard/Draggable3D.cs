using UnityEngine;
using UnityEngine.Events;

public class Draggable3D : MonoBehaviour, IDraggable3D
{
    public UnityEvent mouseDown;
    public UnityEvent mouseUp;
    public UnityEvent<Vector3> startDrag, onDrag, endDrag;

    public void MouseDown()
    {
        mouseDown.Invoke();
    }

    public void MouseUp()
    {
        mouseUp.Invoke();
    }

    public void StartDrag(Vector2 pos)
    {
        startDrag.Invoke(pos);
    }

    public void OnDrag(Vector2 pos)
    {
        onDrag.Invoke(pos);
    }

    public void EndDrag(Vector2 pos)
    {
        endDrag.Invoke(pos);
    }
}