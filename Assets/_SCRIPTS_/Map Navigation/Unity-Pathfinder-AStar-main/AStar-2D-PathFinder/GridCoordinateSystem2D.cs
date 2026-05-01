using System.Collections.Generic;
using MapRooms;
using UnityEngine;

[ExecuteAlways]
public class GridCoordinateSystem2D : MonoBehaviour
{
    public MapToAStarGrid mapToAStarGrid;
    public int subdivisions = 64;
    public float scale = 115f;

    public float CellSize => scale / subdivisions;
    public Vector2 Origin => (Vector2)transform.position - Vector2.one * (scale / 2f);

    private ObstacleManager2D obstacleManager;

    private void OnEnable()
    {
        obstacleManager = GetComponent<ObstacleManager2D>();
        if (obstacleManager == null)
        {
            obstacleManager = gameObject.AddComponent<ObstacleManager2D>();
        }
    }

    public bool ContainsWorldPosition(Vector2 worldPosition)
    {
        Vector2 local = worldPosition - Origin;
        return local.x >= 0 && local.x < scale && local.y >= 0 && local.y < scale;
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return MapToAStarGrid.GridPosToWorldPosition(gridPosition);
        //return Origin + new Vector2((gridPosition.x + 0.5f) * CellSize, (gridPosition.y + 0.5f) * CellSize);
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return MapToAStarGrid.WorldLocationToAStarGridPosition(worldPosition);

        /*
        Vector2 local = worldPosition - Origin;
        return new Vector2Int(
            Mathf.FloorToInt(local.x / CellSize),
            Mathf.FloorToInt(local.y / CellSize)
        );
        */
    }

    public bool IsInBounds(Vector2Int position)
    {
        return position.x >= mapToAStarGrid.startPosOfGrid.x
            && position.y >= mapToAStarGrid.startPosOfGrid.y
            && position.x < mapToAStarGrid.endPosOfGrid.x
            && position.y < mapToAStarGrid.endPosOfGrid.y;

        //return position.x >= 0 && position.y >= 0 && position.x < subdivisions && position.y < subdivisions;
    }

    public Vector2Int? FindNearestNonObstacle(Vector2Int targetGrid, int searchRadius = 5)
    {
        return obstacleManager.FindNearestNonObstacle(targetGrid, searchRadius, IsInBounds);
    }
}
