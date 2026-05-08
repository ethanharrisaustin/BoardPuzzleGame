using System.Collections.Generic;
using Cardboard;
using MoveItMoveIt;
using UnityEngine;

namespace BoardGame
{
    public class Board : MonoBehaviour
    {
        public static Board instance;
        public BoardSetup boardSetup; // Temp! °C

        public List<BoardGamePlayer> players = new List<BoardGamePlayer>();

        void Awake()
        {
            instance = this;
            SetUpBoard(boardSetup);
        }

        public void SetUpBoard(BoardSetup boardSetup)
        {
            players.Clear();
        }

        public void SetUpPlayers(CardboardHolderGO cardboardHolder, UI_TurnSlot[] turnSlots)
        {
            BoardGamePlayer boardGamePlayer = GetBoardGamePlayer(cardboardHolder);

            if (boardGamePlayer == null) return;

            string[] cards = boardGamePlayer.GetMovementCards();

            for (int i = 0; i < cards.Length; ++i)
            {
                cards[i] = turnSlots[i].CardsUniqueID();
            }

            boardGamePlayer.movementCards = cards;
        }

        public BoardGamePlayer GetBoardGamePlayer(CardboardHolderGO cardboardHolder)
        {
            if (!cardboardHolder.ContainsPlayerCharacter()) return null;

            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].playerID == cardboardHolder.GetPlayerPiece().cardboardItemObject.unique_id)
                {
                    return players[i];
                }
            }

            BoardGamePlayer boardGamePlayer = new BoardGamePlayer(cardboardHolder);

            players.Add(boardGamePlayer);

            return boardGamePlayer;
        }

        
    }
}