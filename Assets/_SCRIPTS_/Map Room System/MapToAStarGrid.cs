using UnityEngine;

namespace MapRooms
{
    public class MapToAStarGrid : MonoBehaviour
    {
        public static MapToAStarGrid instance;

        public ObstacleManager2D obstacleManager2D;

        FloorTileGO[] floorTiles;
        public Vector2Int startPosOfGrid, endPosOfGrid;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            //CreateAStarGrid();
            //UpdateObstacles();
        }

        // To be called after a room transition has completed
        public void CreateAStarGrid()
        {
            floorTiles = MapRoomSystem.GetRoomObjectGOs<FloorTileGO>();  

            if (instance == null)
            {
                floorTiles = FindObjectsByType<FloorTileGO>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            }

            if (floorTiles == null)
            {
                //Debug.Log("Floor tiles array is null");
                return;
            }

            //Debug.Log("Length of floor tiles! " + floorTiles.Length);

            startPosOfGrid = GetStartPositionOfGrid();
            endPosOfGrid = GetEndPositionOfGrid();

            UpdateObstacles();
        }

        void UpdateObstacles()
        {
            obstacleManager2D.ClearAllObstacles();

            if (floorTiles == null) return;

            for (int x = startPosOfGrid.x; x < endPosOfGrid.x; ++x)
            {
                for (int y = startPosOfGrid.y; y < endPosOfGrid.y; ++y)
                {
                    Vector2Int c_gridPos = new Vector2Int(x, y);

                    if (TileIsObstacle(c_gridPos))
                    {
                        obstacleManager2D.SetObstacle(c_gridPos, true);

                        //Debug.Log("Setting obstacles");
                    }
                }
            }
        }

        Vector2Int GetStartPositionOfGrid()
        {
            float smallestX = Mathf.Infinity;
            float smallestZ = Mathf.Infinity;

            for (int i = 0; i < floorTiles.Length; ++i)
            {
                if (floorTiles[i] == null) continue;
                
                Vector3 tilePosition = floorTiles[i].GetPosition();
                if (tilePosition.x < smallestX) smallestX = tilePosition.x;
                if (tilePosition.z < smallestZ) smallestZ = tilePosition.z;
            }    

            return WorldLocationToAStarGridPosition(smallestX, smallestZ);
        }

        Vector2Int GetEndPositionOfGrid()
        {
            float largestX = Mathf.NegativeInfinity;
            float largestZ = Mathf.NegativeInfinity;

            for (int i = 0; i < floorTiles.Length; ++i)
            {
                if (floorTiles[i] == null) continue;
                
                Vector3 tilePosition = floorTiles[i].GetPosition();
                if (tilePosition.x > largestX) largestX = tilePosition.x;
                if (tilePosition.z > largestZ) largestZ = tilePosition.z;
            }    

            return WorldLocationToAStarGridPosition(largestX + 1, largestZ + 1);
        }

        bool TileIsObstacle(Vector3 worldPosition)
        {
            FloorTileGO floorTileGO = GetTile(worldPosition);

            return floorTileGO != null && !floorTileGO.IsEmpty();
        }

        public bool TileIsObstacle(Vector2Int gridPosition)
        {
            FloorTileGO floorTileGO = GetTile(gridPosition);

            if (floorTileGO == null) return true;

            //return false;
            return !floorTileGO.IsEmpty();
        }

        FloorTileGO GetTile(Vector3 worldPosition)
        {
            for (int i = 0; i < floorTiles.Length; ++i)
            {
                if (WorldLocationToAStarGridPosition(floorTiles[i].GetPosition()) == WorldLocationToAStarGridPosition(worldPosition))
                {
                    return floorTiles[i];
                }
            } 

            return null;
        }

        FloorTileGO GetTile(Vector2Int gridPosition)
        {
            for (int i = 0; i < floorTiles.Length; ++i)
            {
                if (floorTiles[i] == null) continue;

                if (WorldLocationToAStarGridPosition(floorTiles[i].GetPosition()) == gridPosition)
                {
                    return floorTiles[i];
                }
            } 

            return null;
        }

        public static Vector2Int WorldLocationToAStarGridPosition(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
        }

        public static Vector2Int WorldLocationToAStarGridPosition(float positionX, float positionZ)
        {
            return new Vector2Int(Mathf.RoundToInt(positionX), Mathf.RoundToInt(positionZ));
        }

        public static Vector3 GridPosToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(gridPosition.x, 1f, gridPosition.y);
        }
    }
}