#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MapRooms
{
    [CreateAssetMenu(fileName = "New Room", menuName = "Map Room System/Map Room", order = 0)]
    [System.Serializable]

    public class Room : ScriptableObject
    {
        public string roomUniqueID;
        public RoomObject[] roomObjects;

        #if UNITY_EDITOR
        void OnEnable()
        {
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/Room Object Icon.png");
            if (icon != null)
            {
                EditorGUIUtility.SetIconForObject(this, icon);
            }
        }
        #endif
    }
}