using BoardGame;
using Cardboard;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MoveItMoveIt
{
    public class UI_PlayerTurnsInGame : UI_PlayerTurns
    {
        public static UI_PlayerTurnsInGame instance;


        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        public static void Show(CardboardHolderGO cardboardHolderGO, int highlightSlot)
        {
            if (instance.currentCardboardHolder != null &&
                instance.currentCardboardHolder.PlayerPieceUniqueID() == cardboardHolderGO.PlayerPieceUniqueID())     
            {
                instance.HighlightSlot(highlightSlot);
                return;
            }

            instance.currentCardboardHolder = cardboardHolderGO;

            instance.AnimationOpen();

            instance.ShowCards();

            instance.HighlightSlot(highlightSlot);
        }

        public static void ShowRedHighlight(int highlightSlot)
        {
            for (int i = 0; i < instance.slots.Length; ++i) instance.slots[i].Unhighlight();

            instance.slots[highlightSlot].ShowImpossibleMove();
        }

        void HighlightSlot(int slotToHighlight)
        {
            for (int i = 0; i < slots.Length; ++i) slots[i].Unhighlight();

            slots[slotToHighlight].Highlight();
        }

        public void OnTurnsChange()
        {
            if (currentCardboardHolder == null) return;

            Board.instance.SetUpPlayers(currentCardboardHolder, slots);
        }

        protected override void AnimationOpen()
        {
            FollowPlayerPiece();

            transform.DOKill(false);
            transform.localScale = Vector3.one;
            
            open = true;
        }
    }
}