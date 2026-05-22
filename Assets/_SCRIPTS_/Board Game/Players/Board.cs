using System.Collections.Generic;
using System.Collections;
using Cardboard;
using MoveItMoveIt;
using UnityEngine;
using DG.Tweening;

namespace BoardGame
{
    public class Board : MonoBehaviour
    {
        public static Board instance;

        public List<BoardGamePlayer> players = new List<BoardGamePlayer>();

        const float waitTimeBetweenMovements = 0.4f;

        public static bool inPlayMode = false;

        WaitForSeconds waitForTimeBetweenMoves = new WaitForSeconds(waitTimeBetweenMovements);
        WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

        void Awake()
        {
            instance = this;
            SetUpBoard();
        }

        public void SetUpBoard()
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
            if (cardboardHolder == null || !cardboardHolder.ContainsPlayerCharacter()) return null;

            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].playerID == cardboardHolder.GetPlayerPiece().cardboardItemObject.unique_id)
                {
                    players[i].playerTurnIndex = i;
                    return players[i];
                }
            }

            BoardGamePlayer boardGamePlayer = new BoardGamePlayer(cardboardHolder);

            boardGamePlayer.playerTurnIndex = players.Count;

            players.Add(boardGamePlayer);

            return boardGamePlayer;
        }

        public void StartBoardGame()
        {
            if (inPlayMode) return;

            SetStartPositions();

            UI_CassetteControls.instance.Play();

            StartCoroutine(BoardGameRoutine());
        }

        public void StopBoardgame()
        {
            if (!inPlayMode) return;

            StopCoroutine(BoardGameRoutine());

            inPlayMode = false;

            ResetPieces();

            UI_CassetteControls.instance.Stop();
        }

        void SetStartPositions()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                CardboardHolderGO holder = players[i].GetCardboardHolder();

                if (holder == null) continue;

                players[i].startPos = holder.transform.position;
                players[i].startRot = holder.transform.eulerAngles;
            }
        }

        void ResetPieces()
        {
            for (int i = 0; i < players.Count; ++i)
            {
                CardboardHolderGO holder = players[i].GetCardboardHolder();

                if (holder == null) continue;

                holder.transform.DOKill(false);

                holder.transform.position = players[i].startPos;
                holder.transform.eulerAngles = players[i].startRot;

                players[i].canMove = true;

                holder.OnResetPiece();
            }
        }

        IEnumerator BoardGameRoutine()
        {
            inPlayMode = true;

            int playersTurn = 0;

            while (true)
            {
                if (!inPlayMode) break;

                BoardGamePlayer player = FindPlayer(ref playersTurn);

                if (player == null) break;

                CardboardHolderGO cardboardHolderGO = player.GetCardboardHolder();

                if (cardboardHolderGO == null) break;

                for (int i = 0; i < player.movementCards.Length; ++i)
                {
                    UI_Card_Base card = CardboardItems.GetCardItem(player.movementCards[i]);

                    if (card == null) continue;

                    bool performedAction = card.PerformAction(player);

                    if (performedAction == false) { OnCannotPerformAction(ref player, i); break; }
                    
                    UI_PlayerTurnsInGame.Show(cardboardHolderGO, i);
                    
                    yield return waitForTimeBetweenMoves;
                }

                yield return waitForOneSecond;

                IncreasePlayersTurn(ref playersTurn);
            }

            OnEndBoardGame();

            inPlayMode = false;
        }

        void OnEndBoardGame()
        {
            if (inPlayMode)
            {
                if (BoardGameWin())
                {
                    // Open win menu
                }
                else
                {
                    ResetPieces();
                }
            }

            UI_PlayerTurnsInGame.instance.CloseImmediately();

            UI_CassetteControls.instance.Stop();
        }

        void OnCannotPerformAction(ref BoardGamePlayer player, int i)
        {
            player.canMove = false;

            if (player.GetCardboardHolder().shrunken)
            {
                UI_PlayerTurnsInGame.instance.CloseImmediately();
            }
            else
            {
                UI_PlayerTurnsInGame.ShowRedHighlight(i);
            }
        }

        BoardGamePlayer GetPlayer(ref int playersTurn)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].playerTurnIndex != playersTurn) continue;

                IncreasePlayersTurn(ref playersTurn);

                return players[i];
            } 

            IncreasePlayersTurn(ref playersTurn);

            return null;
        }

        BoardGamePlayer FindPlayer(ref int playersTurn)
        {
            int counter = 0;
            int maxAmount = players.Count * 2;

            while (counter < maxAmount)
            {
                BoardGamePlayer player = GetPlayer(ref playersTurn);

                if (player.CanDoMove()) return player;

                counter ++;
            }

            return null;
        }

        void IncreasePlayersTurn(ref int playersTurn)
        {
            playersTurn++;

            if (playersTurn >= players.Count) playersTurn = 0;
        }

        bool BoardGameWin()
        {
            return false;
        }
    }
}