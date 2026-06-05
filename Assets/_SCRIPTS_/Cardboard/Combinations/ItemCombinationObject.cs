using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Cardboard
{
    [CreateAssetMenu(fileName = "Item Combination Object", menuName = "Board Game/Item Combination")]
    public class ItemCombinationObject : ScriptableObject
    {
        public CardboardItemObject itemA;
        public CardboardItemObject itemB;
        public CardboardItemObject result;

        #if UNITY_EDITOR
        void OnEnable()
        {
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/Item Combination Object.png");
            if (icon != null)
            {
                EditorGUIUtility.SetIconForObject(this, icon);
            }
        }
        #endif

        public bool CanCombine(List<CardboardItemGO> cardboardItemGOs)
        {
            if (cardboardItemGOs.Count != 2) return false;

            if (itemA.IsGroupOfItems() || itemB.IsGroupOfItems())
            {
                return CanCombineGroupOfItems(cardboardItemGOs);
            }

            return CanCombine(cardboardItemGOs, itemA, itemB);
        }

        static bool CanCombine(List<CardboardItemGO> cardboardItemGOs, CardboardItemObject itemA, CardboardItemObject itemB)
        {
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

        bool CanCombineGroupOfItems(List<CardboardItemGO> cardboardItemGOs)
        {
            CardboardItemObject[] itemList;
            CardboardItemObject item;

            if (itemA.IsGroupOfItems() && itemB.IsGroupOfItems())
            {
                Debug.LogError("Item combination contains two groups of items instead of two items, or one item and a group of items.");
                return false;
            }

            if (itemA.IsGroupOfItems())
            {
                itemList = itemA.GetGroupOfItems();
                item = itemB;
            }
            else
            {
                itemList = itemB.GetGroupOfItems();
                item = itemA;
            }

            for (int i = 0; i < itemList.Length; ++i)
            {
                if (itemList[i].IsGroupOfItems())
                {
                    Debug.LogError("A group of items contains another group of items. This is not yet supported.");
                    return false;
                }

                if (CanCombine(cardboardItemGOs, itemList[i], item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}