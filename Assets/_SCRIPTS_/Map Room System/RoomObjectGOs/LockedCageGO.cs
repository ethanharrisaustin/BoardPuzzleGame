using Cardboard;
using DG.Tweening;
using UnityEngine;

namespace MapRooms
{
    public class LockedCageGO : RoomObjectGO, IDragOnto
    {
        public static LockedCageGO instance;

        [SerializeField] Transform hinge;
        [SerializeField] BoxCollider boxCollider;
        [SerializeField] CardboardItemObject keyItem;

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            hinge.localRotation = Quaternion.identity;
            boxCollider.enabled = true;
        }

        public void OpenGate()
        {
            hinge.DOLocalRotate(Vector3.up * -140f, 0.5f).SetEase(Ease.InOutQuad);
            boxCollider.enabled = false;
        }

        public void OnDragHover(CardboardItemObject cardboardItemObject)
        {
            
        }

        public void OnDragUnhover()
        {
            
        }

        public bool OnDropDraggedItem(CardboardItemObject cardboardItemObject)
        {
            if (keyItem.unique_id != cardboardItemObject.unique_id)
            {
                return false;
            }

            OpenGate();

            return true;
        }
    }
}
