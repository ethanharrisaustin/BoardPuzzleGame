using System.Collections.Generic;
using UnityEngine;

namespace Cardboard
{
    [CreateAssetMenu(fileName = "Item Combination Object", menuName = "Board Game/Item Combination")]
    public class ItemCombinationObject : ScriptableObject
    {
        public CardboardItemObject itemA;
        public CardboardItemObject itemB;
        public CardboardItemObject result;

        public bool CanCombine(List<CardboardItemGO> cardboardItemGOs)
        {
            if (cardboardItemGOs.Count != 2) return false;

            if (itemA.unique_id == cardboardItemGOs[0].cardboardItemObject.unique_id
                &&
                itemB.unique_id == cardboardItemGOs[1].cardboardItemObject.unique_id)
            {
                return true;
            }

            if (itemB.unique_id == cardboardItemGOs[0].cardboardItemObject.unique_id
                &&
                itemA.unique_id == cardboardItemGOs[1].cardboardItemObject.unique_id)
            {
                return true;
            }

            return false;
        }
    }
}