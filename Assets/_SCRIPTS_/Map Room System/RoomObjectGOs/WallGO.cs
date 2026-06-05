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

            if (flySettings == null)
            {
                base.Spawn(roomObject, flySettings);
                return;
            }
            
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

        #if UNITY_EDITOR

        public void SetToUprightPos()
        {
            Transform rotator = transform.GetChild(0);

            rotator.DOKill(false);

            rotator.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        
        #endif
    }
}