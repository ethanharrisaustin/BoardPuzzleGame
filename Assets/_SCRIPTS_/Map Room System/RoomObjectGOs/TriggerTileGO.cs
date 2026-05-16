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

        protected virtual bool OnObjectEnter(Collider other)
        {
            RoomObjectGO roomObjectGO = GetRoomObjectGO(other);
            if (roomObjectGO == null || RoomObjectAlreadyOnTile(roomObjectGO)) return false;

            objectsOnTile.Add(roomObjectGO);

            return true;
        }

        protected virtual bool OnObjectExit(Collider other)
        {
            RoomObjectGO roomObjectGO = GetRoomObjectGO(other);
            if (roomObjectGO == null || !RoomObjectAlreadyOnTile(roomObjectGO)) return false;

            objectsOnTile.Remove(roomObjectGO);

            return true;
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

        public bool GetObjectOnTile<T>(out T roomObject) where T : RoomObjectGO
        {
            for (int i = 0; i < objectsOnTile.Count; ++i)
            {
                if (!objectsOnTile[i] is T) continue;
                
                roomObject = objectsOnTile[i] as T;
                return true;
            }

            roomObject = null;
            return false;
        }

        public bool GetObjectsOnTile<T>(out T[] roomObjects) where T : RoomObjectGO
        {
            List<T> values = new List<T>();

            for (int i = 0; i < objectsOnTile.Count; ++i)
            {
                if (!objectsOnTile[i] is T) continue;
                
                values.Add(objectsOnTile[i] as T);
            }

            roomObjects = values.ToArray();
            return roomObjects.Length > 0;
        }
    }
}