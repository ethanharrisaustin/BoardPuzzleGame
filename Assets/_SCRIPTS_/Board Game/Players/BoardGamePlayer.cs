using UnityEngine;
using Cardboard;
using MoveItMoveIt;

namespace BoardGame
{
    [System.Serializable]
    public class BoardGamePlayer
    {
        public string playerID;

        public string[] movementCards;

        public string[] GetMovementCards()
        {
            if (movementCards == null || movementCards.Length != UI_PlayerTurnBoard.numberTurnSlots)
            {
                movementCards = new string[UI_PlayerTurnBoard.numberTurnSlots];
            }

            return movementCards;
        }

        public BoardGamePlayer()
        {
            
        }

        public BoardGamePlayer(CardboardHolderGO cardboardHolderGO)
        {
            playerID = cardboardHolderGO.GetPlayerPiece().cardboardItemObject.unique_id;
        }
    }
}