namespace MapRooms
{
    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    using UnityEngine;

    [CreateAssetMenu(fileName = "Group Of Rooms 1", menuName = "Map Room System/Group Of Map Rooms", order = 1)]
    public class GroupOfRooms : ScriptableObject
    {
        public Room[] rooms;

        #if UNITY_EDITOR
        void OnEnable()
        {
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/Room Object List Icon.png");
            if (icon != null)
            {
                EditorGUIUtility.SetIconForObject(this, icon);
            }
        }
        #endif
    }

}
