using System.Collections.Generic;
using UnityEngine;

namespace Cardboard
{
    public class ItemCombinationHandler : MonoBehaviour
    {
        static ItemCombinationObject[] combinations;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            combinations = Resources.LoadAll<ItemCombinationObject>("");
        }

        public static bool Combine(List<CardboardItemGO> items, out CardboardItemObject result)
        {
            for (int i = 0; i < combinations.Length; ++i)
            {
                if (combinations[i].CanCombine(items))
                {
                    result = combinations[i].result;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}