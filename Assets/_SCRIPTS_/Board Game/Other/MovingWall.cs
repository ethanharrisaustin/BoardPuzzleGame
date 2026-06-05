using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace MapRooms
{
    public class MovingWall : MonoBehaviour
    {
        [SerializeField] Transform wallMoveToPos;

        static List<MovingWall> movingWalls = new List<MovingWall>();

        public static MovingWall Get()
        {
            if (movingWalls.Count == 0) return null;

            return movingWalls[0];
        }

        public static MovingWall GetClosest(Vector3 worldPosition)
        {
            if (movingWalls.Count == 0) return null;

            if (movingWalls.Count == 1) return movingWalls[0];

            float min_distance = Mathf.Infinity;
            float c_distance = Mathf.Infinity;
            MovingWall result = null;

            for (int i = 0; i < movingWalls.Count; ++i)
            {
                if (movingWalls[i] == null) continue;

                c_distance = Vector3.Distance(movingWalls[i].transform.position, worldPosition);

                if (c_distance < min_distance)
                {
                    min_distance = c_distance;
                    result = movingWalls[i];
                }
            }

            return result;
        }



        void OnEnable()
        {
            movingWalls.Add(this);
        }

        void OnDisable()
        {
            movingWalls.Remove(this);
        }

        public void MoveWall()
        {
            float worldXPos = wallMoveToPos.position.x;
            float worldZPos = wallMoveToPos.position.z;

            transform.DOKill(false);

            transform.DOMoveZ(worldZPos, 1f).OnComplete(() =>
            {
                transform.DOMoveX(worldXPos, 1f);
            });
        }
    }

}