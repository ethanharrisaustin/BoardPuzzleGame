using UnityEngine;

namespace MapRooms
{
    public class BoardGameFloorTileGO : FloorTileGO
    {
        public override bool IgnoreInRoomMaking()
        {
            return true;
        }

        public override Vector3 GetPosition()
        {
            return base.GetPosition() - floorPosOffset;
        }
    }
}