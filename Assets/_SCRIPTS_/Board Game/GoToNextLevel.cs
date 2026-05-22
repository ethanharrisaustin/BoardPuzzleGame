using BoardGame;
using MapRooms;
using UnityEngine;

public static class GoToNextLevel
{
    public static void NextLevel()
    {
        if (!MapRoomSystem.instance.GetNextRoom(out Room room)) return;

        MapRoomSystem.instance.SwapToRoom(room);

        Board.instance.SetUpBoard();

        UI_ItemBoard.ClearItems();
    }
}
