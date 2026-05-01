using UnityEditor;
using UnityEngine;

namespace MapRooms
{
    [CustomEditor(typeof(Room))]
    public class RoomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Room room = (Room)target;

            EditorUtility.SetDirty(room);

            if (GUILayout.Button("Make Room"))
            {
                RoomObject[] roomObjects;
                FindFirstObjectByType<MapRoomSystem>().MakeRoom(out roomObjects);
                room.roomObjects = roomObjects;
            }

            if (GUILayout.Button("Load Room"))
            {
                FindFirstObjectByType<MapRoomSystem>().SpawnRoomImmediately(room);
            }

            if (GUILayout.Button("Unload Room"))
            {
                FindFirstObjectByType<MapRoomSystem>().RemoveCurrentRoom();
            }
        }
    }
}
