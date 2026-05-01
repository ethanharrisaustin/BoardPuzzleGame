namespace MapRooms
{
    public class UnaryObjectsHolder : RoomObjectPool
    {
        public override void DestroyAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null) continue;

                Destroy(pool[i].gameObject);
            }

            pool.Clear();
        }

        public override void DestroyRoomObject(RoomObject roomObject)
        {
            if (!RoomObject.PrefabIsMatching(roomObject, prefab)) return;

            for (int i = 0; i < pool.Count;)
            {
                if (!RoomObject.Matching(roomObject, pool[i].roomObject)) 
                {
                    ++i;
                    continue;
                }
                
                Destroy(pool[i].gameObject);
                pool.RemoveAt(i);
            }
        }

        public override void DestroyRoomObject(RoomObjectGO roomObjectGO)
        {
            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null) continue;

                if (!RoomObjectGO.Matching(roomObjectGO, pool[i])) continue;
                
                pool.RemoveAt(i);

                --i;
            }

            Destroy(roomObjectGO.gameObject);
        }
    }
}