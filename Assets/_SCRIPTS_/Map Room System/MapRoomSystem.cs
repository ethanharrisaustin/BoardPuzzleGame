namespace MapRooms
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MapNavigation;
    using UnityEngine;
    using Saving;
    using System;

    public class MapRoomSystem : MonoBehaviour
    {
        #region  Variables
        public static MapRoomSystem instance;
        
        [Space]

        public RoomObjectFlyInSettings[] objectFlyInSettings;

        [Space]
        public GroupOfRooms groupOfRooms;
        List<RoomObjectPool> roomObjectPools = new List<RoomObjectPool>();

        Room currentRoom = null;

        //[SerializeField] RoomObjectGO floorTile;

        #endregion
        
        #region Monobehaviour Functions
        
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            roomObjectPools.Clear();

            for (int i = 0; i < transform.childCount; ++i)
            {
                RoomObjectPool roomObjectPool = transform.GetChild(i).GetComponent<RoomObjectPool>();

                if (roomObjectPool != null) roomObjectPools.Add(roomObjectPool);
            }

            SpawnRoom(groupOfRooms.rooms[0]);
        }

        #endregion

        #region Spawning in Rooms

        public void SpawnRoomImmediately(Room room)
        {
            RemoveCurrentRoom();

            // Spawn and set positions
            for (int i = 0; i < room.roomObjects.Length; ++i)
            {
                RoomObjectPool roomObjectPool = GetRoomObjectPool(room.roomObjects[i]);

                if (roomObjectPool == null) continue;

                roomObjectPool.SpawnRoomObject(room.roomObjects[i]);
            }
        }

        public async void SpawnRoom(Room room, string entranceToPlacePlayer = "")
        {
            if (currentRoom == room) return;

            RemoveCurrentRoom();

            RoomObjectSave[] roomObjectSaves = LoadRoom(room);

            // Spawn and set positions
            for (int i = 0; i < room.roomObjects.Length; ++i)
            {
                RoomObjectPool roomObjectPool = GetRoomObjectPool(room.roomObjects[i]);

                if (roomObjectPool == null) continue;

                RoomObjectGO roomObjectGO = roomObjectPool.SpawnRoomObject(room.roomObjects[i]);

                if (roomObjectGO == null) continue;

                RoomObjectSave roomObjectSave = GetRoomObjectSave(roomObjectSaves, roomObjectGO);

                if (roomObjectSave == null) continue;

                roomObjectGO.LoadRoomObject(roomObjectSave);
            }

            currentRoom = room;

            // Place player
            if (entranceToPlacePlayer != "")
            {
                DoorRoomTransitionGO doorRoomTransitionGO = GetDoorRoomTransitionGO(entranceToPlacePlayer);

                if (doorRoomTransitionGO != null)
                {
                    PlayerGO.instance.transform.position = doorRoomTransitionGO.targetPosition;
                }
            }

            // Set inside the object pool that we have started spawning objects in 
            OnStartSpawning();

            // While objects are spawning in, wait 
            while(!FinishedFlyingIn()) await Task.Yield();

            // Once they have all been spawned, initialise them!
            for (int i = 0; i < roomObjectPools.Count; ++i) roomObjectPools[i].InitAllRoomObjects();

            CalculateRoomStartAndEndPos();

            //PlayerGO.instance?.ActivatePlayer();

            //MapToAStarGrid.instance?.CreateAStarGrid();
        }

        

        bool finishedFlyingIn = false;
        void OnStartSpawning()
        {
            finishedFlyingIn = false;
            for (int i = 0; i < roomObjectPools.Count; ++i) roomObjectPools[i].OnStartSpawning();
        }
        public bool FinishedFlyingIn()
        {
            if (finishedFlyingIn) return true;

            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i].FinishedFlyingIn() == false) return false;
            }

            finishedFlyingIn = true;
            return true;
        }

        public bool FinishedFlying()
        {
            return FinishedFlyingIn() && FinishedFlyingOut();
        }

        #endregion

        #region Swapping to Rooms

        public void SwapToRoom(string roomUniqueID)
        {
            for (int i = 0; i < groupOfRooms.rooms.Length; ++i)
            {
                if (groupOfRooms.rooms[i].roomUniqueID == roomUniqueID)
                {
                    SwapToRoom(groupOfRooms.rooms[i]);
                    return;
                }
            }
        }
        public async void SwapToRoom(Room room)
        {
            if (currentRoom == room) return;

            SaveCurrentRoom();

            RemoveAllRoomObjects();

            OnStartRemoving();
            
            // While objects are spawning in, wait 
            while(!FinishedFlyingOut()) await Task.Yield();

            SpawnRoom(room, currentRoom.roomUniqueID);
        }

        bool finishedFlyingOut = false;
        void OnStartRemoving()
        {
            finishedFlyingOut = false;
            for (int i = 0; i < roomObjectPools.Count; ++i) roomObjectPools[i].OnStartRemoving();
        }
        public bool FinishedFlyingOut()
        {
            if (finishedFlyingOut) return true;

            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i].FinishedFlyingOut() == false) return false;
            }

            finishedFlyingOut = true;
            return true;
        }

        void RemoveAllRoomObjects()
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i] == null) continue;

                roomObjectPools[i].RemoveAllRoomObjects();
            }
        }

        #endregion

        #region Door transtion

        DoorRoomTransitionGO GetDoorRoomTransitionGO(string roomUniqueID)
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i].prefab.GetComponent<DoorRoomTransitionGO>() == null) continue;

                for (int x = 0; x < roomObjectPools[i].pool.Count; ++x)
                {
                    DoorRoomTransitionGO doorRoomTransitionGO = roomObjectPools[i].pool[x].GetComponent<DoorRoomTransitionGO>();

                    if (doorRoomTransitionGO.roomUniqueID == roomUniqueID) return doorRoomTransitionGO;
                }
            }

            return null;
        }

        #endregion

        #region Room size

        List<RoomObjectGO> floorTiles = new List<RoomObjectGO>();
        [SerializeField] RoomObjectPool floorTilePool;

        Vector3 startPos, endPos;

   

        bool calculatingRoomSize = false;

        async void CalculateRoomStartAndEndPos()
        {
            if (calculatingRoomSize) return;

            calculatingRoomSize = true;

            floorTiles = floorTilePool.pool;

            startPos = await StartPosOfRoom();

            endPos = await EndPosOfRoom();

            calculatingRoomSize = false;
        }

        async Task<Vector3> StartPosOfRoom()
        {
            float smallestX = Mathf.Infinity;
            float smallestZ = Mathf.Infinity;

            int counter = 0;
            for (int i = 0; i < floorTiles.Count; ++i)
            {
                counter ++;

                if (counter > 30)
                {
                    counter = 0;
                    await Task.Yield();
                }

                if (floorTiles[i] == null) continue;
                
                Vector3 tilePosition = floorTiles[i].GetPosition();
                if (tilePosition.x < smallestX) smallestX = tilePosition.x;
                if (tilePosition.z < smallestZ) smallestZ = tilePosition.z;
            }    

            return new Vector3(smallestX, 0, smallestZ);
        }

        async Task<Vector3> EndPosOfRoom()
        {
            float largestX = Mathf.NegativeInfinity;
            float largestZ = Mathf.NegativeInfinity;

            int counter = 0;
            for (int i = 0; i < floorTiles.Count; ++i)
            {
                counter ++;

                if (counter > 30)
                {
                    counter = 0;
                    await Task.Yield();
                }

                if (floorTiles[i] == null) continue;
                
                Vector3 tilePosition = floorTiles[i].GetPosition();
                if (tilePosition.x > largestX) largestX = tilePosition.x;
                if (tilePosition.z > largestZ) largestZ = tilePosition.z;
            }    

            return new Vector3(largestX, 0, largestZ);
        }

        public static bool CalculatingRoomSize()
        {
            return instance.calculatingRoomSize;
        }

        public static Vector3 RoomStartPos()
        {
            return instance.startPos;
        }

        public static Vector3 RoomEndPos()
        {
            return instance.endPos;
        }

        #endregion

        #region General

        RoomObjectPool GetRoomObjectPool(RoomObject roomObject)
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObject == null || roomObject.prefab == null)
                {
                    Debug.Log("This room object is null");
                    return null;
                }

                if (roomObjectPools[i] == null || roomObjectPools[i].prefab == null)
                {
                    roomObjectPools.RemoveAt(i);
                    i--;
                    continue;
                }

                if (!RoomObject.PrefabIsMatching(roomObject, roomObjectPools[i].prefab)) continue;

                return roomObjectPools[i];
            }


            #if UNITY_EDITOR

            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).TryGetComponent(out RoomObjectPool roomObjectPool))
                {
                    if (!RoomObject.PrefabIsMatching(roomObject, roomObjectPool.prefab)) continue;

                    roomObjectPools.Add(roomObjectPool);



                    return roomObjectPool;
                }
            }

            #endif

            GameObject newPoolParent = new GameObject(roomObject.prefab.name + " Pool");
            newPoolParent.transform.parent = transform;

            bool isUnary = roomObject.prefab.GetComponent<UnaryObjectGO>() != null;
            
            RoomObjectPool newRoomObjectPool;

            if (isUnary) newRoomObjectPool = newPoolParent.AddComponent<UnaryObjectsHolder>();
            else         newRoomObjectPool = newPoolParent.AddComponent<RoomObjectPool>();

            roomObjectPools.Add(newRoomObjectPool);

            newRoomObjectPool.prefab = roomObject.prefab;

            return newRoomObjectPool;
        }

        public void RemoveCurrentRoom()
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                roomObjectPools[i].DestroyAllRoomObjects();
            }

            #if UNITY_EDITOR

            RoomObjectGO[] activeRoomObjectGOs = FindObjectsByType<RoomObjectGO>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < activeRoomObjectGOs.Length; ++i)
            {
                if (activeRoomObjectGOs[i].IgnoreInRoomMaking()) continue;
                
                activeRoomObjectGOs[i].gameObject.SetActive(false);
            }

            #endif
        }

        #if UNITY_EDITOR

        public bool MakeRoom(out RoomObject[] roomObjects)
        {
            RoomObjectGO[] allRoomObjectGOs = FindObjectsByType<RoomObjectGO>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            List<RoomObject> roomObjectsList = new List<RoomObject>();

            for (int i = 0; i < allRoomObjectGOs.Length; ++i)
            {
                RoomObject newRoomObject = allRoomObjectGOs[i].GetRoomObject();

                if (newRoomObject == null) continue;

                if (ListContainsThisObject(roomObjectsList, newRoomObject)) continue;

                roomObjectsList.Add(newRoomObject);
            }

            roomObjects = roomObjectsList.ToArray();

            return true;
        }

        bool ListContainsThisObject(List<RoomObject> roomObjectsList, RoomObject c_roomObject)
        {
            for (int i = 0; i < roomObjectsList.Count; ++i)
            {
                if (RoomObject.Matching(roomObjectsList[i], c_roomObject)) return true;
            }

            return false;
        }

        

        #endif

        public bool GetNextRoom(out Room room)
        {
            for (int i = 0; i < groupOfRooms.rooms.Length - 1; ++i)
            {
                if (groupOfRooms.rooms[i] == currentRoom)
                {
                    room = groupOfRooms.rooms[i + 1];
                    return true;
                }
            }

            room = null;
            return false;
        }
        
        #endregion

        #region Saving Room States

        public void SaveCurrentRoom()
        {
            return;
            if (currentRoom == null) return;

            List<RoomObjectSave> roomObjectSaves = new List<RoomObjectSave>();

            LoopThroughRoomObjectPools((RoomObjectGO roomObjectGO) =>
            {
                if (roomObjectGO.GetRoomObjectSave(out RoomObjectSave roomObjectSave)) roomObjectSaves.Add(roomObjectSave);
            });

            SaveManager roomSaveManager = Saving.Room.GetSaveManager();
            roomSaveManager.SetRoomObjectArray(currentRoom.roomUniqueID, roomObjectSaves.ToArray());
        }

        RoomObjectSave[] LoadRoom(Room room)
        {
            SaveManager roomSaveManager = Saving.Room.GetSaveManager();
            
            return roomSaveManager.GetRoomObjectArray(room.roomUniqueID, null);
        }

        void LoopThroughRoomObjectPools(Action<RoomObjectGO> action)
        {
            for (int x = 0; x < roomObjectPools.Count; ++x)
            for (int y = 0; y < roomObjectPools[x].pool.Count; ++y)
            {
                if (roomObjectPools[x].pool[y].gameObject.activeSelf == false) continue;

                action.Invoke(roomObjectPools[x].pool[y]);
            }
        }

        RoomObjectSave GetRoomObjectSave(RoomObjectSave[] roomObjectSaves, RoomObjectGO roomObjectGO)
        {
            if (roomObjectSaves == null) return null;

            for (int i = 0; i < roomObjectSaves.Length; ++i)
            {
                if (RoomObjectSave.RoomObjectSaveID(roomObjectGO.roomObject) !=  roomObjectSaves[i].saveId) continue;

                return roomObjectSaves[i];
            }

            FindAnyObjectByType<GameObject>();

            return null;
        }

        #endregion
   
        #region Getting objects
        
        // Called on any OnDisable from any RoomObjectGO script
        public static void OnRoomObjectWasDeactivated()
        {
            if (instance == null) return;

            instance.cachedGetObjectGOs.Clear();
        }

        List<GetActiveRoomObjectGOsCached> cachedGetObjectGOs = new List<GetActiveRoomObjectGOsCached>();
        public static T[] GetRoomObjectGOs<T>() where T : RoomObjectGO
        {
            return GetRoomObjectGOs<T>(out _);
        }

        public static T[] GetRoomObjectGOs<T>(out bool recalculated) where T : RoomObjectGO
        {
            recalculated = false;

            // Return nothing is Awake() hasn't been called yet
            if (instance == null) return new T[0];

            /* 
            // Return cached list of these objects
            for (int i = 0; i < instance.cachedGetObjectGOs.Count; ++i)
            {
                if (instance.cachedGetObjectGOs[i].type != typeof(T)) continue;

                return instance.cachedGetObjectGOs[i].cashedList as T[];
            }
            */
            recalculated = true;

            // Recalculate result
            List<T> result = new List<T>();

            for (int i = 0; i < instance.roomObjectPools.Count; ++i)
            {
                if (!instance.roomObjectPools[i].roomObjectGO is T) continue;

                result.AddRange(instance.roomObjectPools[i].GetActives<T>());
            }

            instance.cachedGetObjectGOs.Add(new GetActiveRoomObjectGOsCached(typeof(T), result));

            return result.ToArray();
        }

        class GetActiveRoomObjectGOsCached
        {
            public Type type;
            public object cashedList;

            public GetActiveRoomObjectGOsCached(Type type, object cashedList)
            {
                this.type = type;
                this.cashedList = cashedList;
            }
        }

        #endregion
    }
}