using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MapRooms
{
    public class WallGO : RoomObjectGO
    {
        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            Transform rotator = transform.GetChild(0);

            rotator.localEulerAngles = new Vector3(85f, 0f, 0f);

            float delay = (transform.position.x + transform.position.z + 5) * flySettings.delayMultiplier;
            delay += flySettings.initialDelay;

            delay += flySettings.fallTime * Random.Range(0.6f, 1.2f);

            rotator.DOLocalRotate(Vector3.zero, 0.6f).SetEase(Ease.OutBack).SetDelay(delay);

            base.Spawn(roomObject, flySettings);

            
        }

        public override string ObjectFlyInCategory()
        {
            return "Wall";
        }

    }
}