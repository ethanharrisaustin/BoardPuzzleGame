using BoardGame;
using Cardboard;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MoveItMoveIt
{
    public class UI_PlayerTurnBoard : UI_PlayerTurns
    {
        public static UI_PlayerTurnBoard instance;

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (open) OpenUpdate();
        }

        protected override void OpenUpdate()
        {
            base.OpenUpdate();

            if (Board.inPlayMode)
            {
                CloseImmediately();
            }
        }

        public static void Show(CardboardHolderGO cardboardHolderGO)
        {
            if (AlreadyShowing(cardboardHolderGO)) return;

            instance.currentCardboardHolder = cardboardHolderGO;

            instance.AnimationOpen();

            instance.ShowCards();
        }

        public static bool AlreadyShowing(CardboardHolderGO cardboardHolderGO)
        {
            if (instance.currentCardboardHolder != null &&
                instance.currentCardboardHolder.PlayerPieceUniqueID() == cardboardHolderGO.PlayerPieceUniqueID())
                return true;

            return false;
        }

        public void EjectPlayerPiece()
        {
            if (!open || Board.inPlayMode) return;

            currentCardboardHolder.CollectCardboard();
            
            CloseImmediately();
        }

        public void OnTurnsChange()
        {
            if (currentCardboardHolder == null) return;

            Board.instance.SetUpPlayers(currentCardboardHolder, slots);
        }
    }
}