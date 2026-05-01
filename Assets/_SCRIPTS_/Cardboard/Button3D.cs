using UnityEngine;

public interface IButton3D 
{
    void MouseOver();
    void MouseOut();
    void Click();

    GameObject gameObject { get; }
}

public interface IDraggable3D 
{
    void MouseDown();
    void MouseUp();
    void StartDrag(Vector3 worldPosition);
    void OnDrag(Vector3 worldPosition);
    void EndDrag(Vector3 worldPosition);
}
