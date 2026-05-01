using UnityEngine;

namespace MapRooms
{
    [System.Serializable]
    public class RoomObject
    {
        public Vector3 position, scale, rotation;

        public GameObject prefab;

        public string[] values;

        public string cachedRoomSaveID; // Set in RoomObjectSave

        public RoomObject(GameObject prefab, Vector3 position, Vector3 scale, Vector3 rotation, string[] values)
        {
            this.prefab = prefab;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.values = values;
        }

        public static bool Matching(RoomObject a, RoomObject b)
        {
            //if (a == null && b == null) return true;

            if (a == null || b == null) return false;

            return a.position == b.position && a.scale == b.scale && a.rotation == b.rotation;
        }

        public static bool PrefabIsMatching(RoomObject a, RoomObject b)
        {
            //if (a == null && b == null) return true;

            if (a == null || b == null) return false;

            return a.prefab.name == b.prefab.name;
        }

        public static bool PrefabIsMatching(RoomObject a, GameObject prefab)
        {
            //if (a == null && prefab == null) return true;

            if (a == null || prefab == null) return false;

            return a.prefab.name == prefab.name;
        }

        public class FlySettings
        {
            public AnimationCurve curve;
            public float fallTime;
            public float startYPos;
            public float initialDelay;
            public float delayMultiplier;

            public FlySettings(RoomObjectGO roomObjectGO, bool flyingIn)
            {
                if (flyingIn)
                {
                    RoomObjectFlyInSettings.GetRoomObjectFlyInSettings(
                        roomObjectGO.ObjectFlyInCategory(),
                        out curve,
                        out fallTime,
                        out startYPos,
                        out initialDelay,
                        out delayMultiplier
                    );
                }
                else
                {
                    RoomObjectFlyInSettings.GetRoomObjectFlyOutSettings(
                        roomObjectGO.ObjectFlyInCategory(),
                        out curve,
                        out fallTime,
                        out startYPos,
                        out initialDelay,
                        out delayMultiplier
                    );
                }
            }
        }
    }
}