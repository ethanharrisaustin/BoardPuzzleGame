using System.Collections.Generic;
using UnityEngine;
using MapRooms;
using MapNavigation;
using DG.Tweening;

namespace Cardboard
{
    public class CardboardHolder : MoveableObjectGO, IButton3D
    {
        public List<CardboardItemGO> heldCardboard;

        public int assignedToPlayer = 1;

        [Space]

        Transform itemGOPosition;

        [SerializeField] float bounceHeight = 0.5f;
        [SerializeField] float bounceTime = 0.5f;
        [SerializeField] AnimationCurve bounceCurve;

        [SerializeField] bool collectable = false;
        [SerializeField] CardboardItemObject itemObject;

        
        protected override void Start()
        {
            base.Start();

            heldCardboard.Clear();
            heldCardboard.AddRange(GetHeldCardboards());
        }

        protected CardboardItemGO[] GetHeldCardboards()
        {
            return GetComponentsInChildren<CardboardItemGO>();
        }

        public void AddCardboard(CardboardItemObject cardboardItem)
        {
            if (heldCardboard.Count >= 2) return;

            heldCardboard.Add(CreateCardboardItemGO(cardboardItem));
        }

        CardboardItemGO CreateCardboardItemGO(CardboardItemObject cardboardItem)
        {
            GameObject newGO = Instantiate(cardboardItem.roomObject, transform);

            newGO.transform.localPosition = itemGOPosition.localScale;

            newGO.transform.DOScale(Vector2.one, 0.3f).SetEase(Ease.OutBack);

            return newGO.GetComponent<CardboardItemGO>();
        }

        public void MouseOver()
        {
            if (!collectable) return;

            HoverBounce();
        }

        public void MouseOut()
        {

        }

        public void Click()
        {
            if (!collectable) return;

            // Collect logic
        }
        
        bool isTweeningBounce = false;
        void HoverBounce()
        {
            if (isTweeningBounce) return;

            transform.DOKill(false);

            SnapTo(GetFloorTileCentre().GetPosition()); // Reset position to current tile

            isTweeningBounce = true;

            transform.DOMoveY(transform.position.y + bounceHeight, bounceTime).SetEase(bounceCurve).OnComplete(() =>
            {
                isTweeningBounce = false;
            });
        }
    }
}