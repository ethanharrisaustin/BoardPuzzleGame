using BoardGame;
using Cardboard;
using DG.Tweening;
using UnityEngine;

namespace MoveItMoveIt
{
    public class UI_PlayerTurns : MonoBehaviour
    {
        protected Camera cam;

        protected UI_TurnSlot[] slots;

        [SerializeField] protected CanvasGroup canvasGroup;

        protected CardboardHolderGO currentCardboardHolder;

        public const int numberTurnSlots = 5;

        protected bool open = false;
        protected float openTimer = 0f;

        protected virtual void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            cam = Camera.main;

            slots = GetComponentsInChildren<UI_TurnSlot>();
        }

        protected virtual void LateUpdate()
        {
            FollowPlayerPiece();

            if (open) OpenUpdate();
        }

        protected virtual void OpenUpdate()
        {
            openTimer += Time.deltaTime;
        }

        public void ForceClose()
        {
            transform.DOKill(false);
            transform.localScale = Vector3.one;
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).SetUpdate(true);

            currentCardboardHolder = null;

            openTimer = 0f;
            open = false;
        }

        public void Close()
        {
            if (!open || openTimer < 0.2f) return;

            ForceClose();
        }

        public void CloseImmediately()
        {
            transform.DOKill(false);
            transform.localScale = Vector3.zero;

            currentCardboardHolder = null;

            openTimer = 0f;
            open = false;
        }

        protected void ShowCards()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                slots[i].SetSlotAsEmptyWithoutNotify();
            }

            BoardGamePlayer boardGamePlayer = Board.instance.GetBoardGamePlayer(currentCardboardHolder);

            if (boardGamePlayer == null) return;
            
            string[] moveCards = boardGamePlayer.GetMovementCards();

            for (int i = 0; i < moveCards.Length; ++i)
            {
                slots[i].ShowCard(moveCards[i]);
            }
        }

        protected void FollowPlayerPiece()
        {
            if (currentCardboardHolder == null) return;

            transform.position = cam.WorldToScreenPoint(currentCardboardHolder.transform.position + Vector3.up * 1.8f);
        }

        protected virtual void AnimationOpen()
        {
            FollowPlayerPiece();

            transform.DOKill(false);
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);

            open = true;
        }
    }
}