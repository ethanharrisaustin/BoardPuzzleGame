using Cardboard;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public CardboardItemObject cardboardItemObject;

    public void Click()
    {
        UI_ItemBoard.AddItemToBoard(cardboardItemObject);

        Destroy(gameObject);
    }
}
