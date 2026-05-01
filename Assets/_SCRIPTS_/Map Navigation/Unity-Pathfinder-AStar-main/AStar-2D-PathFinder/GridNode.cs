using UnityEngine;

public class GridNode
{
    public Vector2Int GridPosition;
    public Vector3 WorldPosition;
    public float GCost = float.MaxValue;
    public float HCost;
    public float FCost => GCost + HCost;
    public GridNode Parent;

    public GridNode(Vector2Int gridPos, Vector3 worldPos)
    {
        GridPosition = gridPos;
        WorldPosition = worldPos;
    }
}
