using Cardboard;
using UnityEngine;

namespace MapRooms
{
    public class GroupOfCardboardHoldersGO : RoomObjectGO
    {
        public string[] itemsRequiredID;

        public override void GetValues(out string[] values)
        {
            values = itemsRequiredID;
        }


        public override void SetValues(string[] values)
        {
            itemsRequiredID = values;
        }
    }
}
