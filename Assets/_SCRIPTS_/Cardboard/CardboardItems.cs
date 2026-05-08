using MoveItMoveIt;
using UnityEngine;

namespace Cardboard
{
    public class CardboardItems : MonoBehaviour
    {
        static CardboardItemObject[] items;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            items = Resources.LoadAll<CardboardItemObject>("");
        }

        public static CardboardItemObject GetItem(string unique_id)
        {
            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i].unique_id != unique_id) continue;

                return items[i];
            }

            return null;
        }

        public static UI_Card_Base GetCardItem(string unique_id)
        {
            if (string.IsNullOrEmpty(unique_id)) return null;

            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i].unique_id != unique_id) continue;

                return items[i].GetItemUI().GetComponent<UI_Card_Base>();
            }
            
            return null;
        }
    }
}