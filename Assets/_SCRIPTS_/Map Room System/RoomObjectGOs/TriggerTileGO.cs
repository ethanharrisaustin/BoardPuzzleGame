using System.Collections.Generic;
using MapNavigation;
using UnityEngine;

namespace MapRooms
{
    public class TriggerTileGO : RoomObjectGO
    {
        [SerializeField] protected BoxCollider tileTrigger;
        [SerializeField] protected LayerMask layerMask;

        [SerializeField]  protected List<RoomObjectGO> objectsOnTile = new List<RoomObjectGO>();

        protected void OnTriggerEnter(Collider other) { OnObjectEnter(other); }
        protected void OnTriggerStay(Collider other) { OnObjectEnter(other); }
        protected void OnTriggerExit(Collider other) { OnObjectExit(other); }
        protected void OnCollisionEnter(Collision other) { OnObjectEnter(other.collider); }
        protected void OnCollisionStay(Collision other) { OnObjectEnter(other.collider); }
        protected void OnCollisionExit(Collision other) { OnObjectExit(other.collider); }

        protected virtual void OnObjectEnter(Collider other)
        {
            RoomObjectGO roomObjectGO = GetRoomObjectGO(other);
            if (roomObjectGO == null || RoomObjectAlreadyOnTile(roomObjectGO)) return;

            objectsOnTile.Add(roomObjectGO);
        }

        protected virtual void OnObjectExit(Collider other)
        {
            RoomObjectGO roomObjectGO = GetRoomObjectGO(other);
            if (roomObjectGO == null || !RoomObjectAlreadyOnTile(roomObjectGO)) return;

            objectsOnTile.Remove(roomObjectGO);
        }

        protected bool RoomObjectAlreadyOnTile(RoomObjectGO roomObjectGO)
        {
            return objectsOnTile.Contains(roomObjectGO);
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
                if (roomObjectGO == this) continue;

                if (RoomObjectAlreadyOnTile(roomObjectGO)) continue;

                objectsOnTile.Add(roomObjectGO);
            }
        }

        protected Vector3 HitBoundingBoxPos()
        {
            return tileTrigger.transform.position + tileTrigger.center;
        }

        protected Vector3 HitBoundingBoxSize()
        {
            return new Vector3(transform.lossyScale.x * tileTrigger.size.x, transform.lossyScale.y * tileTrigger.size.y, transform.lossyScale.z * tileTrigger.size.z);
        }

        public bool ContainsPlayer()
        {
            for (int i = 0; i < objectsOnTile.Count; ++i)
            {
                if (objectsOnTile[i] is PlayerGO) return true;
            }

            return false;
        }

        public void AddToTile(RoomObjectGO roomObjectGO)
        {
            if (RoomObjectAlreadyOnTile(roomObjectGO)) return;

            objectsOnTile.Add(roomObjectGO);
        }

        public void RemoveToTile(RoomObjectGO roomObjectGO)
        {
            objectsOnTile.Remove(roomObjectGO);
        }
    }
}