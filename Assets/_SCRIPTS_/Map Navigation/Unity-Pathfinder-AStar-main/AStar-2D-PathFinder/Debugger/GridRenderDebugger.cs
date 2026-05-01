using UnityEngine;
using MapRooms;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class GridRenderDebugger : MonoBehaviour
{
    public Color gridColor = Color.green;
    public Color fontColor = Color.white;
    public Color obstacleColor = Color.red;
    public bool showLabels = true;
    public int fontSize = 10;

    public GridCoordinateSystem2D grid;
    public MapToAStarGrid mapToAStarGrid;
    private ObstacleManager2D obstacleManager;

    private void OnEnable()
    {
        if (grid != null)
        {
            obstacleManager = grid.GetComponent<ObstacleManager2D>();
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null || obstacleManager == null) return;

        float cellSize = 1f;

        mapToAStarGrid.CreateAStarGrid();

        Vector3 origin = MapToAStarGrid.GridPosToWorldPosition(mapToAStarGrid.startPosOfGrid) - Vector3.one * .5f;
        Vector3 endOfGrid = MapToAStarGrid.GridPosToWorldPosition(mapToAStarGrid.endPosOfGrid) - Vector3.one * .5f;

        int xNum = Mathf.CeilToInt(Mathf.Abs(origin.x - endOfGrid.x));
        int yNum = Mathf.CeilToInt(Mathf.Abs(origin.z - endOfGrid.z));

        // Draw the grid
        Gizmos.color = gridColor;
        for (int y = 0; y <= yNum; y++)
            Gizmos.DrawLine(origin + new Vector3(0, 0, y * cellSize), origin + new Vector3(cellSize * xNum, 0, y * cellSize));
        

        for (int x = 0; x <= xNum; x++)
            Gizmos.DrawLine(origin + new Vector3(x * cellSize, 0, 0), origin + new Vector3(x * cellSize, 0, cellSize * yNum));


        // Draw obstacles
        Gizmos.color = obstacleColor;
        foreach (Vector2Int obstacle in obstacleManager.GetAllObstacles())
        {
            Vector3 worldPosition = MapToAStarGrid.GridPosToWorldPosition(obstacle);// grid.GridToWorld(obstacle);
            Gizmos.DrawCube(worldPosition  - Vector3.one * .5f + new Vector3(1f, 0.1f, 1f) * 1f * 0.5f, new Vector3(1f, 0.1f, 1f) * 1f * 0.8f);
        }

/* 
        // Draw labels
#if UNITY_EDITOR
        if (showLabels)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = fontColor;
            style.fontSize = fontSize;

            for (int x = 0; x < subdivisions; x++)
            {
                for (int y = 0; y < subdivisions; y++)
                {
                    Vector2 worldPos = grid.GridToWorld(new Vector2Int(x, y));
                    Vector3 screenPoint = Handles.matrix.MultiplyPoint(worldPos);
                    if (Camera.current != null)
                    {
                        screenPoint = Camera.current.WorldToScreenPoint(worldPos);
                        if (screenPoint.z > 0) // Only draw if point is in front of camera
                        {
                            Handles.Label(worldPos, $"({x},{y})", style);
                        }
                    }
                }
            }
        }
#endif

*/
    }
}
