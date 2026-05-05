using UnityEngine;
using Cardboard;

namespace BoardGame
{
    [System.Serializable]
    public class BoardGamePlayer : MonoBehaviour
    {
        public CardboardHolderGO assignedPlayerPiece;

        public BoardGamePlayer()
        {
            assignedPlayerPiece = null;
        }
    }
}