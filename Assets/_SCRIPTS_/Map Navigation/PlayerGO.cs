using MapRooms;
using UnityEngine;
using static Input;

namespace MapNavigation
{
    public class PlayerGO : MoveableObjectGO
    {
        public static PlayerGO instance;

        [Space, Header("Player Movement Settings")]
        public double moveToTileTime;
        public double timeBetweenMovements = -1;

        [HideInInspector] bool playerIsActive = true;

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        protected override void Start()
        {
            base.Start();

            timeBetweenMovements = MusicRhythmTimer.SecondsBetweenBeats();
        }

        public override void Init()
        {
            base.Init();

            playerIsActive = true;
        }

        bool wasPressed = false;
        protected override void Update()
        {
            base.Update();

            if (!playerIsActive) return;

            GetInputs(out bool holdingDown, out bool pressed);

            bool reallyIsPressed = holdingDown && !wasPressed;

            //if (!pressed && MusicRhythmTimer.HalfBeatIncreased() == false) return;

            if (objectIsMoving) return;

            if (reallyIsPressed) MovementHandler();

            wasPressed = holdingDown;
        }

        Direction direction;
        void GetInputs(out bool holdingDown, out bool pressed)
        {
            if (MovementAsDirection(favourX) == Direction.none) 
            {
                holdingDown = false;
                pressed = false;
                return;
            }

            bool directionWasNone = direction == Direction.none;

            direction = MovementAsDirection(favourX);

            holdingDown = true;
            pressed = directionWasNone;
        }

        //Direction previousDirection;
        double moveInputCooldown = 0f;
        bool favourX = false;
        void MovementHandler()
        {
            switch (direction)
            {
                case Direction.north:
                MoveNorthInput();
                favourX = false;
                break;

                case Direction.east:
                MoveEastInput();
                favourX = true;
                break;

                case Direction.south:
                MoveSouthInput();
                favourX = false;
                break;

                case Direction.west:
                MoveWestInput();
                favourX = true;
                break;

                default: return;
            }

            moveInputCooldown = timeBetweenMovements <= moveToTileTime ? moveToTileTime * 0.995f : timeBetweenMovements;

            direction = Direction.none;
        }

        public virtual void MoveNorthInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveNorth(out newFloorTile)) return;

            SetPositionTo(newFloorTile);
        }
        public virtual void MoveEastInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveEast(out newFloorTile)) return;

           SetPositionTo(newFloorTile);
        }
        public virtual void MoveSouthInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveSouth(out newFloorTile)) return;

            SetPositionTo(newFloorTile);
        }
        public virtual void MoveWestInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveWest(out newFloorTile)) return;

            SetPositionTo(newFloorTile);
        }

        public void DeactivatedPlayer()
        {
            playerIsActive = false;
        }
        public void ActivatePlayer()
        {
            playerIsActive = true;

            gameObject.SetActive(true);
        }

        public bool PlayerIsAbleToMove()
        {
            // FOR NOW
            return MusicRhythmTimer.BeatIncreased();

            //if (playerIsActive == false) return false;

            //return moveInputCooldown <= 0f;
        }
    }
}
