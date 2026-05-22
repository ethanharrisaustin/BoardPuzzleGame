using System.Threading.Tasks;
using Cardboard;
using DG.Tweening;
using UnityEngine;

namespace MapRooms
{
    public class WeightedPlatformGO : FloorTileGO
    {
        bool movedDown = false;

        public static bool platformIsMoving = false;

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            platformIsMoving = false;
        }
        
        protected override bool OnObjectEnter(Collider other)
        {
            if (!base.OnObjectEnter(other)) return false;

            if (ContainsCardboardHolder())
            {
                MoveDown();
            }

            return true;
        }

        async void MoveDown()
        {
            if (movedDown) return;

            transform.DOMoveY(transform.position.y - 1f, 0.4f);

            CardboardHolderGO cardboardHolderGO;

            GetObjectOnTile(out cardboardHolderGO);

            cardboardHolderGO.GetPlayer().canMove = false;

            BottomlessCageGO.instance.MoveUp();

            float timer = 0f;
            while(timer < 0.4f)
            {
                timer += Time.deltaTime;
                await Task.Yield();

                cardboardHolderGO.transform.position = GetPosition();
            }
        }
    }
}