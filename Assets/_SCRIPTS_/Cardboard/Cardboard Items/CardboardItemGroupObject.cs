using UnityEngine;


namespace Cardboard
{
    [CreateAssetMenu(fileName = "Cardboard Item Group", menuName = "Board Game/Cardboard Item Group")]
    public class CardboardItemGroupObject : CardboardItemObject
    {
        public CardboardItemObject[] cardboardItemObjects;

        public override bool IsGroupOfItems()
        {
            return true;
        }

        public override CardboardItemObject[] GetGroupOfItems()
        {
            return cardboardItemObjects;
        }
    }
}