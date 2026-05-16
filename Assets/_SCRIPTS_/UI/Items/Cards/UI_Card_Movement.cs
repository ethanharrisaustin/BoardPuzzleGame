using BoardGame;
using Cardboard;
using UnityEngine;

namespace MoveItMoveIt
{
    public class UI_Card_Movement : UI_Card_Base
    {
        public enum Direction { up, right, down, left, none }

        [SerializeField] Direction direction;

        public override bool PerformAction(BoardGamePlayer player)
        {
            if (!base.PerformAction(player)) return false;

            CardboardHolderGO holderGO = player.GetCardboardHolder();

            // Move the player piece in this direction!
            switch (direction)
            {
                case Direction.up:
                    return MoveUp(holderGO);
                
                case Direction.down:
                    return MoveDown(holderGO);

                case Direction.right:
                    return MoveRight(holderGO);
                
                case Direction.left:
                    return MoveLeft(holderGO);
            }

            return false;
        }

        bool MoveUp(CardboardHolderGO holderGO)
        {
            if (!holderGO.CanMoveNorth(out var floorTile)) return false;

            holderGO.SetPositionTo(floorTile);

            return true;
        }

        bool MoveRight(CardboardHolderGO holderGO)
        {
            if (!holderGO.CanMoveEast(out var floorTile)) return false;

            holderGO.SetPositionTo(floorTile);

            return true;
        }

        bool MoveDown(CardboardHolderGO holderGO)
        {
            if (!holderGO.CanMoveSouth(out var floorTile)) return false;

            holderGO.SetPositionTo(floorTile);

            return true;
        }

        bool MoveLeft(CardboardHolderGO holderGO)
        {
            if (!holderGO.CanMoveWest(out var floorTile)) return false; 

            holderGO.SetPositionTo(floorTile);

            return true;
        }
    }
}