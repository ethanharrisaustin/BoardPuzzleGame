namespace MapRooms
{
    [System.Serializable]
    public class RoomObjectSave
    {
        public string saveId;
        public string[] saveValues;

        public RoomObjectSave(RoomObject roomObjectGO, string[] saveValues)
        {
            SetUp(roomObjectGO, saveValues);
        }
        public void SetUp(RoomObject roomObjectGO, string[] saveValues)
        {
            saveId = RoomObjectSaveID(roomObjectGO);

            this.saveValues = saveValues;
        }
        public static string RoomObjectSaveID(RoomObject roomObject)
        {
            if (string.IsNullOrEmpty(roomObject.cachedRoomSaveID)) roomObject.cachedRoomSaveID = roomObject.position.ToString() + roomObject.rotation.ToString();

            return roomObject.cachedRoomSaveID;
        }
    }
}