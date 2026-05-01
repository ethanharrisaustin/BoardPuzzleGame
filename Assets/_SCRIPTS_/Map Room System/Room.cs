using UnityEngine;

namespace MapRooms
{
    [CreateAssetMenu(fileName = "New Room", menuName = "Map Room System/Map Room", order = 0)]
    [System.Serializable]

    public class Room : ScriptableObject
    {
        public string roomUniqueID;
        public RoomObject[] roomObjects;
    }
}