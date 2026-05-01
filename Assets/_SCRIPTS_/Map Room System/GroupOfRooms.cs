namespace MapRooms
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Group Of Rooms 1", menuName = "Map Room System/Group Of Map Rooms", order = 1)]
    public class GroupOfRooms : ScriptableObject
    {
        public Room[] rooms;
    }

}
