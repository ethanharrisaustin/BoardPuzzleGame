using System.Collections.Generic;
using Combat;
using MapNavigation;
using UnityEngine;

namespace MapRooms
{
    public class EnemyGO : ObjectWithHealthGO
    {
        #region  Variables

        public enum EnemyPhase {  walking, attacking, stunned };
        public EnemyPhase currentPhase = EnemyPhase.walking; // Set this to 'attacking'
        [HideInInspector] public EnemyPhase previousPhase = EnemyPhase.stunned;
        public bool canMoveDiagonally = false;
        public float moveTime = 0.2f;
        public int beatsBeforeAttack = 4;
        public Element elementToAttackWith;

        #endregion

        #region Update

        protected override void Update()
        {
            base.Update();

            if (currentPhase != previousPhase)
            {
                OnChangedPhase();
                previousPhase = currentPhase;
            }

            switch (currentPhase)
            {
                case EnemyPhase.walking:
                WalkingPhase();
                break;

                case EnemyPhase.attacking:
                AttackingPhase();
                break;

                case EnemyPhase.stunned:
                StunnedPhase();
                break;
            }
        }

        void OnChangedPhase()
        {
            switch (currentPhase)
            {
                case EnemyPhase.walking:
                OnStartedWalkingPhase();
                break;

                case EnemyPhase.attacking:
                OnStartedAttackingPhase();
                break;

                case EnemyPhase.stunned:
                OnStartedStunnedPhase();
                break;
            }
        }

        #endregion

        #region Walking Phase

        protected virtual void OnStartedWalkingPhase()
        {
            
        }

        bool isMovingThisTime = false;
        protected virtual void WalkingPhase()
        {
            if (!MusicRhythmTimer.BeatIncreased()) return;

            isMovingThisTime = ! isMovingThisTime;

            if (!isMovingThisTime) return;

            MoveTowardsPlayer();
        }

        protected virtual void MoveTowardsPlayer()
        {
            GridPathfinding.allowDiagonalMovement = canMoveDiagonally;
            
            Vector3 playerWorldPos = ClosestPosFromPlayer();

            List<Vector3> pathToPlayer = GridPathfinding.FindPathWorld(GetCenterPosition(), playerWorldPos );

            if (pathToPlayer.Count <= 1) return;

            MoveToPos(pathToPlayer[1]);
        }

        protected void MoveToPos(Vector3 position)
        {
            Input.Direction directionToMove = DirectionFromAToB(transform.position, position);

            FloorTileGO floorTileGO;

            switch(directionToMove)
            {
                case Input.Direction.north:
                    if (!CanMoveNorth(out floorTileGO)) return;
                    break;

                case Input.Direction.east:
                    if (!CanMoveEast(out floorTileGO)) return;
                    break;

                case Input.Direction.south:
                    if (!CanMoveSouth(out floorTileGO)) return;
                    break;

                case Input.Direction.west:
                    if (!CanMoveWest(out floorTileGO)) return;
                    break;

                default: return;
            }

            floorTileGO.AddToTile(this);
            GetFloorTileCentre().RemoveToTile(this);
            SetPositionTo(floorTileGO);
        }

        protected Vector3 ClosestPosFromPlayer()
        {
            Vector3[] adjacentTiles = PlayerAdjacentTiles();

            float shortestDistance = Mathf.Infinity;
            float currentDistance;
            Vector3 chosenTile = PlayerGO.instance.GetPosition();
            for (int i = 0; i < adjacentTiles.Length; ++i)
            {
                Vector2Int tilePos = MapToAStarGrid.WorldLocationToAStarGridPosition(adjacentTiles[i]);

                if (MapToAStarGrid.instance.TileIsObstacle(tilePos)) continue;

                currentDistance = Vector3.Distance(adjacentTiles[i], GetPosition());

                if (currentDistance >= shortestDistance) continue;

                shortestDistance = currentDistance;

                chosenTile = adjacentTiles[i];
            }

            return chosenTile;
        }

        Vector3[] PlayerAdjacentTiles()
        {
            if (canMoveDiagonally)
            {
                return PlayerGO.instance.GetAdjacentTiles();
            }
            else
            {
                return PlayerGO.instance.GetAdjacentCardinalTiles();
            }
        }

        #endregion

        #region  Attacking Phase

        protected virtual void OnStartedAttackingPhase()
        {
            EnemyAttackRing ring = EnemyAttackRings.GetEnemyAttackRing(this);

            ring.SetElement(this, elementToAttackWith);
            ring.SetMaxBeat(beatsBeforeAttack);
        }

        protected virtual void AttackingPhase()
        {
            if (!MusicRhythmTimer.BeatIncreased()) return;

            EnemyAttackRing ring = EnemyAttackRings.GetEnemyAttackRing(this);

            bool attackPlayer = ring.IncreaseBeat();

            if (attackPlayer)
            {
                Debug.Log("Attacking player");
                ring.SetMaxBeat(beatsBeforeAttack); // Resetting it back to 0
            }
        }

        #endregion

        #region Stunned Phase

        protected virtual void OnStartedStunnedPhase()
        {
            
        }

        protected virtual void StunnedPhase()
        {
            
        }

        #endregion
    }
}