using UnityEngine;
using MapRooms;
using MapNavigation;

namespace Cardboard
{
    public class CardboardItemGO : RoomObjectGO, IButton3D
    {
        public CardboardItemObject cardboardItemObject;

        public void MouseOver()
        {

        }

        public void MouseOut()
        {

        }

        public void Click()
        {
            UI_ItemBoard.AddItemToBoard(cardboardItemObject);

            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public override string ObjectFlyInCategory()
        {
            return "Items";
        }

        public string[] GetTurnSlotsIDS()
        {
            return cardboardItemObject.GetTurnSlotsIDS();
        }
    }
}