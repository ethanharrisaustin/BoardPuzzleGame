using System.Collections.Generic;
using MapNavigation;

#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
#endif

using UnityEngine;

namespace MapRooms
{
    public class RoomObjectPool : MonoBehaviour
    {
        public RoomObjectGO roomObjectGO = null;
        public GameObject prefab;

        public List<RoomObjectGO> pool = new List<RoomObjectGO>();

        RoomObject.FlySettings flyInSettings, flyOutSettings;

        bool hasSetFlySettings = false;

        public RoomObjectGO SpawnRoomObject(RoomObject roomObject)
        {
            if (!RoomObject.PrefabIsMatching(roomObject, prefab)) return null;

            RoomObjectGO newRoomObjectGO = GetRoomObjectGO();

            if (roomObjectGO == null) 
            {
                roomObjectGO = newRoomObjectGO;
            }

            SetUpFlySettings(newRoomObjectGO);

            newRoomObjectGO.transform.parent = transform;

            newRoomObjectGO.Spawn(roomObject, flyInSettings);

            return newRoomObjectGO;
        }

        public virtual void DestroyRoomObject(RoomObject roomObject)
        {
            if (!RoomObject.PrefabIsMatching(roomObject, prefab)) return;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (!RoomObject.Matching(roomObject, pool[i].roomObject)) continue;
                
                pool[i].gameObject.SetActive(false);
            }
        }

        public virtual void DestroyRoomObject(RoomObjectGO roomObjectGO)
        {
            if (roomObjectGO == null) return;

            roomObjectGO.gameObject.SetActive(false);
        }

        public virtual void DestroyAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count;)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    continue;
                }

                pool[i].gameObject.SetActive(false);

                ++i;
            }
        }

        public virtual void RemoveAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count;)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    continue;
                }

                pool[i].Remove(flyOutSettings, DestroyRoomObject);

                ++i;
            }
        }

        public void InitAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count;)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    continue;
                }

                pool[i].Init();

                ++i;
            }
        }

        protected virtual RoomObjectGO GetRoomObjectGO()
        {
            needsToRecalulateActives = true;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    i--;
                    continue;
                }
                
                if (pool[i].gameObject.activeSelf == false)
                {
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
                }
            }

            #if UNITY_EDITOR

            if (GetRoomObjectGOFromChild(out var roomObjectGO))  return roomObjectGO;

            RoomObjectGO newRoomObjectGO = PrefabUtility.InstantiatePrefab(prefab).GetComponentInChildren<RoomObjectGO>();

            #else
    
            RoomObjectGO newRoomObjectGO = Instantiate(prefab).GetComponentInChildren<RoomObjectGO>();

            #endif

            newRoomObjectGO.roomObjectPool = this;
            pool.Add(newRoomObjectGO);

            return newRoomObjectGO;
        }

        bool finishedFlyingIn = false;
        public void OnStartSpawning()
        {
            finishedFlyingIn = false;
        }
        public bool FinishedFlyingIn()
        {
            if (roomObjectGO != null && (roomObjectGO is EnemyGO || roomObjectGO is PlayerGO))
            {
                return true;
            }

            if (finishedFlyingIn) return true;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null) continue;

                if (pool[i].FinishedFlyingIn() == false) return false;
            }

            finishedFlyingIn = true;
            return true;
        }

        bool finishedFlyingOut = false;
        public void OnStartRemoving()
        {
            finishedFlyingOut = false;
        }
        public bool FinishedFlyingOut()
        {
            if (finishedFlyingOut) return true;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null) continue;

                if (pool[i].FinishedFlyingOut() == false) return false;
            }

            finishedFlyingOut = true;
            return true;
        }

        public int NumberActive()
        {
            int total = 0;
            for (int i = 0; i < pool.Count; ++i) 
            {
                if (pool[i] != null && pool[i].gameObject.activeSelf)
                {
                    total++;
                }
            }
           
            return total;
        }

        [HideInInspector] public bool needsToRecalulateActives = true;
        object previousActives = null;
        public T[] GetActives<T>() where T:RoomObjectGO
        {
            // Return cached list of actives if we haven't spawned / removed any
            if (!needsToRecalulateActives) return previousActives as T[];

            T[] result = new T[NumberActive()];

            int index = 0;
            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null) continue;

                if (pool[i].gameObject.activeSelf == false) continue;

                result[index] = pool[i] as T;

                ++index;
            }

            previousActives = result;

            return result;
        }

        void SetUpFlySettings(RoomObjectGO roomObjectGO)
        {
            if (hasSetFlySettings) return;

            flyInSettings = new RoomObject.FlySettings(roomObjectGO, true);
            flyOutSettings = new RoomObject.FlySettings(roomObjectGO, false);

            hasSetFlySettings = true;
        }

        #region  Unity Editor

        #if UNITY_EDITOR

        bool GetRoomObjectGOFromChild(out RoomObjectGO roomObjectGO)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                RoomObjectGO roGO;
                
                if (!transform.GetChild(i).TryGetComponent(out roGO))
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    continue;
                }

                if (RoomObjectGOInPool(roGO)) continue;

                pool.Add(roGO);

                roGO.gameObject.SetActive(true);

                roomObjectGO = roGO;

                return true;
            }

            roomObjectGO = null;
            return false;
        }

        bool RoomObjectGOInPool(RoomObjectGO roomObjectGO)
        {
            if (roomObjectGO == null) return false;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == roomObjectGO) return true;
            }
            return false;
        }

        #endif

        #endregion
    }
}