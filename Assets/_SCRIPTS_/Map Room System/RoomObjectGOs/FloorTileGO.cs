using UnityEngine;

namespace MapRooms
{
    public class FloorTileGO : TriggerTileGO
    {
        public override string ObjectFlyInCategory()
        {
            return "FloorTileGO";
        }

        public static FloorTileGO GetFloorTileGO(Collider collider)
        {
            FloorTileGO floorTileGO = collider.GetComponentInChildren<FloorTileGO>();
            if (floorTileGO == null) floorTileGO = collider.GetComponentInParent<FloorTileGO>();

            return floorTileGO;
        }

        public static FloorTileGO GetFloorTileGO(Collider[] collider)
        {
            for (int i = 0; i < collider.Length; ++i)
            {
                FloorTileGO floorTileGO = GetFloorTileGO(collider[i]);

                if (floorTileGO != null) return floorTileGO;
            }

            return null;
        }

        public bool IsEmpty()
        {
            #if UNITY_EDITOR
            // If we haven't started playing yet 
            if (MapRoomSystem.instance == null)
            {
                Init(); 
            }
            #endif

            return objectsOnTile.Count <= 0;
        }

        public override void Init()
        {
            base.Init();

            Collider[] hitColliders = Physics.OverlapBox(HitBoundingBoxPos(), HitBoundingBoxSize(), transform.rotation, layerMask);

            objectsOnTile.Clear();

            for (int i = 0; i < hitColliders.Length; ++i)
            {
                RoomObjectGO roomObjectGO = GetRoomObjectGO(hitColliders[i]);

                if (roomObjectGO == null) continue;
                if (roomObjectGO is FloorTileGO) continue;
                if (roomObjectGO is MapNavigation.PlayerGO) continue;

                if (RoomObjectAlreadyOnTile(roomObjectGO)) continue;

                objectsOnTile.Add(roomObjectGO);
            }
        }

        Vector3 floorPosOffset = new Vector3(0f, 0.45f, 0f);
        public override Vector3 GetPosition()
        {
            return base.GetPosition() + floorPosOffset;
        }
    }
}