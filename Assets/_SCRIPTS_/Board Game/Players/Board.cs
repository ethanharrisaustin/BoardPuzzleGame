using UnityEngine;

namespace BoardGame
{
    public class Board : MonoBehaviour
    {
        public BoardSetup boardSetup; // Temp! °C

        public BoardGamePlayer[] players;

        void Awake()
        {
            SetUpBoard(boardSetup);
        }

        public void SetUpBoard(BoardSetup boardSetup)
        {
            players = new BoardGamePlayer[boardSetup.numberOfPlayers];
        }
    }
}