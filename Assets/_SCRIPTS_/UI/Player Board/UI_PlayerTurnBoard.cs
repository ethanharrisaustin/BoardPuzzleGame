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

        UI_TurnSlot[] slots;

        void Awake()
        {
            instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cam = Camera.main;

            slots = GetComponentsInChildren<UI_TurnSlot>();
        }

        void LateUpdate()
        {
            FollowPlayerPiece();
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

            transform.position = cam.WorldToScreenPoint(currentCardboardHolder.transform.position) + Vector3.up * 10f;
        }

        void AnimationOpen()
        {
            FollowPlayerPiece();

            transform.DOKill(false);
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }
}