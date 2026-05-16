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

        public int playerTurnIndex = 0;

        public Vector3 startPos, startRot;

        public bool canMove = true;

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

        public CardboardHolderGO GetCardboardHolder()
        {
            return CardboardHolderGO.GetCardboardHolderWithPiece(playerID);
        }

        public bool CanDoMove()
        {
            if (UI_CompletionMenu.isOpen) return false;
            
            if (AllMovementCardsAreEmpty()) return false;
            
            return canMove;
        }

        bool AllMovementCardsAreEmpty()
        {
            for (int i = 0; i < movementCards.Length; ++i)
            {
                if (!string.IsNullOrEmpty(movementCards[i])) return false;
            }

            return true;
        }
    }
}