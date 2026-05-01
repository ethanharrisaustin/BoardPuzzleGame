using DG.Tweening;
using UnityEngine;

namespace MapRooms
{
    [CreateAssetMenu(fileName = "RoomObject Fly In Settings", menuName = "Map Room System/RoomObject Fly In Settings", order = 3)]
    public class RoomObjectFlyInSettings : ScriptableObject
    {
        public string objectCategory;
        public AnimationCurve fallInCurve;
        public float fallTime, startYPos, initialDelay, delayMultiplier;

        [Space]
        public AnimationCurve exitCurve;
        public float exitTime, exitInitialDelay, exitDelayMultiplier;

        public static RoomObjectFlyInSettings GetRoomObjectFlyInSettings(string objectCategory)
        {
            MapRoomSystem mapRoom = MapRoomSystem.instance;

            if (mapRoom== null) return null;

            for (int i = 0; i < mapRoom.objectFlyInSettings.Length; ++i)
            {
                if (mapRoom.objectFlyInSettings[i].objectCategory == objectCategory) return mapRoom.objectFlyInSettings[i];
            }

            return null;
        }

        public static bool GetRoomObjectFlyInSettings(string objectCategory, out AnimationCurve curve, out float fallTime, out float startYPos, out float initialDelay, out float delayMultiplier)
        {
            MapRoomSystem mapRoom = MapRoomSystem.instance;
            curve = null; fallTime = 0; startYPos = 0; initialDelay = 0; delayMultiplier = 0;

            if (mapRoom == null) return false;

            for (int i = 0; i < mapRoom.objectFlyInSettings.Length; ++i)
            {
                if (mapRoom.objectFlyInSettings[i].objectCategory != objectCategory) continue;

                curve = mapRoom.objectFlyInSettings[i].fallInCurve;
                fallTime = mapRoom.objectFlyInSettings[i].fallTime;
                startYPos = mapRoom.objectFlyInSettings[i].startYPos;
                initialDelay = mapRoom.objectFlyInSettings[i].initialDelay;
                delayMultiplier = mapRoom.objectFlyInSettings[i].delayMultiplier;

                return true;
            }

            return false;
        }

        public static bool GetRoomObjectFlyOutSettings(string objectCategory, out AnimationCurve curve, out float exitTime, out float endYPos, out float initialDelay, out float delayMultiplier)
        {
            MapRoomSystem mapRoom = MapRoomSystem.instance;
            curve = null; exitTime = 0; endYPos = 0; initialDelay = 0; delayMultiplier = 0;

            if (mapRoom == null) return false;

            for (int i = 0; i < mapRoom.objectFlyInSettings.Length; ++i)
            {
                if (mapRoom.objectFlyInSettings[i].objectCategory != objectCategory) continue;

                curve = mapRoom.objectFlyInSettings[i].exitCurve;
                exitTime = mapRoom.objectFlyInSettings[i].exitTime;
                endYPos = mapRoom.objectFlyInSettings[i].startYPos;
                initialDelay = mapRoom.objectFlyInSettings[i].exitInitialDelay;
                delayMultiplier = mapRoom.objectFlyInSettings[i].exitDelayMultiplier;

                return true;
            }

            return false;
        }
    }
}