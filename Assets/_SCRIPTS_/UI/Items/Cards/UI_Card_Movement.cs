using UnityEngine;

namespace MoveItMoveIt
{
    public class UI_Card_Movement : UI_Card_Base
    {
        public enum Direction { up, right, down, left, none }

        [SerializeField] Direction direction;

        public override void PerformAction()
        {
            base.PerformAction();

            // Move the player piece in this direction!
        }
    }
}