using Cardboard;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IButton3D, IDraggable3D
{
    public CardboardItemObject cardboardItemObject;

    FlatObjectWobble flatObjectWobble;

    void Awake()
    {
        flatObjectWobble = GetComponent<FlatObjectWobble>();
    }

    public void MouseOver()
    {
        if (flatObjectWobble != null) flatObjectWobble.DoWobble1();
    }

    public void MouseOut()
    {
        
    }

    public void Click()
    {
        UI_ItemBoard.AddItemToBoard(cardboardItemObject);

        gameObject.SetActive(false);
    }

    public void MouseDown()
    {
        
    }

    public void MouseUp()
    {
        
    }

    public void StartDrag(Vector2 pos)
    {
        UI_ItemBoard.StartDraggingItem(cardboardItemObject);

        gameObject.SetActive(false);
    }

    public void OnDrag(Vector2 pos)
    {
        
    }

    public void EndDrag(Vector2 pos)
    {
        UI_ItemBoard.StopDraggingItem(cardboardItemObject);
    }
}
