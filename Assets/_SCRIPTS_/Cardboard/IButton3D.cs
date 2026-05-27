using UnityEngine;

public interface IButton3D 
{
    void MouseOver();
    void MouseOut();
    void Click();

    GameObject gameObject { get; }

    bool enabled { get; set;}
}

public interface IDraggable3D 
{
    void MouseDown();
    void MouseUp();
    void StartDrag(Vector2 worldPosition);
    void OnDrag(Vector2 worldPosition);
    void EndDrag(Vector2 worldPosition);

    GameObject gameObject { get; }

    bool enabled { get; set;}
}
