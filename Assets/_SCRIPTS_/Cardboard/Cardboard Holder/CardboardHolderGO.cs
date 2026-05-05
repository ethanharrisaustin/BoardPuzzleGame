using System.Collections.Generic;
using UnityEngine;
using MapRooms;
using MapNavigation;
using DG.Tweening;
using System.Threading.Tasks;

namespace Cardboard
{
    public class CardboardHolderGO : MoveableObjectGO, IButton3D
    {
        public List<CardboardItemGO> heldCardboard;

        public int assignedToPlayer = 1;

        [Space]

        [SerializeField] Transform[] itemGOPositions;

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

        public bool AddCardboard(CardboardItemObject cardboardItem)
        {
            if (heldCardboard.Count >= 2) return false;

            heldCardboard.Add(CreateCardboardItemGO(cardboardItem));

            return true;
        }

        CardboardItemGO CreateCardboardItemGO(CardboardItemObject cardboardItem)
        {
            GameObject newGO = Instantiate(cardboardItem.roomObject, transform);

            PositionCardboard(newGO.transform);

            newGO.GetComponentInChildren<IButton3D>().enabled = false;

            return newGO.GetComponent<CardboardItemGO>();
        }

        void PositionCardboard(Transform newItem)
        {
            if (heldCardboard.Count == 0)
            {
                PositionCardboardOnlyOne(newItem);
            }
            else
            {
                PositionCardboardTwo(newItem);
            }
        }

        void PositionCardboardOnlyOne(Transform newItem, float animTimeMultiplier = 1f)
        {
            SetPosition(newItem, itemGOPositions[0]);

            newItem.transform.DOScale(Vector3.one, 0.3f * animTimeMultiplier).SetEase(Ease.OutBack);
        }

        void PositionCardboardTwo(Transform newItem)
        {
            MoveFirstCardboardToFirst();

            SetPosition(newItem, itemGOPositions[2]);

            newItem.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(CombineItems);
        }

        void SetPosition(Transform newItem, Transform target)
        {
            newItem.transform.parent = target;
            newItem.transform.localPosition = target.localPosition;
            newItem.transform.localRotation = target.localRotation;
            newItem.transform.localScale = Vector3.zero;
        }

        void MoveFirstCardboardToFirst()
        {
            if (heldCardboard.Count < 1) return;

            MoveCardboardTo(heldCardboard[0].transform, itemGOPositions[1]);
        }

        void MoveFirstCardboardToCenter()
        {
            if (heldCardboard.Count < 1) return;

            MoveCardboardTo(heldCardboard[0].transform, itemGOPositions[0]);
        }

        void MoveCardboardTo(Transform cardboard, Transform target)
        {
            cardboard.DOLocalMove(target.localPosition, 0.25f).SetEase(Ease.InOutQuad);
        }

        async void CombineItems()
        {
            if (ItemCombinationHandler.Combine(heldCardboard, out var result))
            {
                CardboardItemGO newItem = result.GetItemGO();

                newItem.transform.localScale = Vector3.zero;

                newItem.enabled = false;

                // Scale down two current items
                for (int i = 0; i < heldCardboard.Count; ++i)
                {
                    heldCardboard[i].transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
                    {
                        Destroy(heldCardboard[i].gameObject);
                    });
                }

                heldCardboard.Clear();

                await Task.Delay((int)(1000f * 0.5f));

                PositionCardboardOnlyOne(newItem.transform, 1.3f);

                heldCardboard.Add(newItem);
            }
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
            if (heldCardboard.Count > 0)
            {
                CollectCardboard();
                return;
            }

            if (!collectable) return;

            // Collect logic
        }

        void CollectCardboard()
        {
            CardboardItemGO itemToCollect = heldCardboard[heldCardboard.Count - 1];

            heldCardboard.Remove(itemToCollect);

            itemToCollect.Click();           
        }
        
        bool isTweeningBounce = false;
        void HoverBounce()
        {
            if (isTweeningBounce) return;

            if (UI_DraggedItem.IsDraggingItem()) return;

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