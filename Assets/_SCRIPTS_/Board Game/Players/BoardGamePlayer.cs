using UnityEngine;
using Cardboard;

namespace BoardGame
{
    [System.Serializable]
    public class BoardGamePlayer : MonoBehaviour
    {
        public CardboardHolder assignedPlayerPiece;

        public BoardGamePlayer()
        {
            assignedPlayerPiece = null;
        }
    }
}