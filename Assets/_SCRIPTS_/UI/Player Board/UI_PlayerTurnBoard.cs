using Cardboard;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MoveItMoveIt
{
    public class UI_PlayerTurnBoard : MonoBehaviour
    {
        public static UI_PlayerTurnBoard instance;
        Camera cam;

        [SerializeField] CanvasGroup canvasGroup;

        CardboardHolderGO currentCardboardHolder;

        public const int numberTurnSlots = 5;

        UI_TurnSlot[] slots;

        void Awake()
        {
            instance = this;

            transform.localScale = Vector3.zero;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cam = Camera.main;

            slots = GetComponentsInChildren<UI_TurnSlot>();
        }

        float openTimer = 0f;
        bool open = false;

        void LateUpdate()
        {
            FollowPlayerPiece();

            if (open) openTimer += Time.deltaTime;
        }

        public static void Show(CardboardHolderGO cardboardHolderGO)
        {
            if (instance.currentCardboardHolder != null &&
                instance.currentCardboardHolder.PlayerPieceUniqueID() == cardboardHolderGO.PlayerPieceUniqueID())
                return;

            instance.currentCardboardHolder = cardboardHolderGO;

            instance.AnimationOpen();
        }

        void FollowPlayerPiece()
        {
            if (currentCardboardHolder == null) return;

            transform.position = cam.WorldToScreenPoint(currentCardboardHolder.transform.position + Vector3.up * 1.8f);
        }

        void AnimationOpen()
        {
            FollowPlayerPiece();

            transform.DOKill(false);
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);

            open = true;
        }

        void ShowCards()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                slots[i].SetSlotAsEmptyWithoutNotify();
            }

            string[] savedValues = currentCardboardHolder.GetPlayerPiece().cardboardItemObject.savedValues;

            if (savedValues == null) return;

            for (int i = 0; i < savedValues.Length; ++i)
            {
                slots[i].ShowCard(savedValues[i]);
            }
        }

        public void Close()
        {
            if (!open || openTimer < 0.2f) return;

            transform.DOKill(false);
            transform.localScale = Vector3.one;
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).SetUpdate(true);

            currentCardboardHolder = null;

            openTimer = 0f;
            open = false;
        }

        public void OnTurnsChange()
        {
            if (currentCardboardHolder == null) return;

            currentCardboardHolder.SetPlayerMovements(slots);
        }
    }
}