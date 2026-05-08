using Cardboard;
using MoveItMoveIt;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Cardboard Item Object", menuName = "Board Game/CardboardItemObject")]
public class CardboardItemObject : ScriptableObject, IItem
{
    public string unique_id;
    public GameObject icon;
    public GameObject roomObject;
    public bool isPlayerPiece, useOnce;

    [HideInInspector] public UI_Item_Base cached_ui_item;

    [HideInInspector] public string[] savedValues;

    public UI_Item_Base GetItemUI()
    {
        if (cached_ui_item == null)
        {
            Transform parent = UI_ItemBoard.instance.itemUiParent;

            cached_ui_item = Instantiate(icon, parent).GetComponent<UI_Item_Base>();

            cached_ui_item.unique_id = unique_id;

            cached_ui_item.cardboardItemObject = this;
        }
        
        cached_ui_item.gameObject.SetActive(true);
        cached_ui_item.transform.parent = UI_ItemBoard.instance.itemUiParent;
        cached_ui_item.transform.localScale = Vector3.one;
        cached_ui_item.GetComponentInChildren<Image>().enabled = true;

        return cached_ui_item;
    }

    public Image[] GetImages()
    {
        return GetItemUI().GetImages();
    }

    public CardboardItemGO GetItemGO()
    {
        CardboardItemGO cardboardItem = Instantiate(roomObject).GetComponent<CardboardItemGO>();

        return cardboardItem;
    }

    public Transform transform { get { return GetItemUI().transform; } }

    public string[] GetTurnSlotsIDS()
    {
        if (savedValues == null || savedValues.Length != UI_PlayerTurnBoard.numberTurnSlots)
            savedValues = new string[UI_PlayerTurnBoard.numberTurnSlots];
        
        return savedValues;
    }

    public bool UseOnce()
    {
        return isPlayerPiece || useOnce;
    }
}
