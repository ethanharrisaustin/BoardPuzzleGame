using DG.Tweening;
using UnityEngine;

namespace MapRooms
{
    public class BottomlessCageGO : RoomObjectGO
    {
        public static BottomlessCageGO instance;

        [SerializeField] Transform cage;

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        public void MoveUp()
        {
            cage.DOMoveY(cage.position.y + 1f, 0.4f);
        }
    }
}