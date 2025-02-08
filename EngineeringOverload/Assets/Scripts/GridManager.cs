using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int gridSize = 1;
    public bool showGrid = true;

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    public Vector3Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldPosition.x / gridSize),
            Mathf.RoundToInt(worldPosition.y / gridSize),
            Mathf.RoundToInt(worldPosition.z / gridSize)
        );
    }

    public Vector3 GridToWorld(Vector3Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * gridSize,
            gridPosition.y * gridSize,
            gridPosition.z * gridSize
        );
    }

    public bool CanPlace(Vector3Int[] cells)
    {
        foreach (var cell in cells)
        {
            if (occupiedCells.Contains(cell))
                return false;
        }
        return true;
    }

    public void OccupyCell(Vector3Int cell)
    {
        occupiedCells.Add(cell);
    }

    public void FreeCell(Vector3Int cell)
    {
        occupiedCells.Remove(cell);
    }

    public Vector3Int[] GetOccupiedCells(Vector3Int gridPosition, GameObject prefab, int rotation)
    {
        Bounds bounds = prefab.GetComponent<Renderer>().bounds;
        int width = Mathf.RoundToInt(bounds.size.x / gridSize);
        int height = Mathf.RoundToInt(bounds.size.z / gridSize);

        List<Vector3Int> cells = new List<Vector3Int>();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3Int offset = new Vector3Int(x, 0, z);

                if (rotation == 90) offset = new Vector3Int(-z, 0, x);
                if (rotation == 180) offset = new Vector3Int(-x, 0, -z);
                if (rotation == 270) offset = new Vector3Int(z, 0, -x);

                cells.Add(gridPosition + offset);
            }
        }

        return cells.ToArray();
    }
}
