using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(1, 1); // Default building size
    private int rotationIndex = 0; // Tracks rotation state
    public Collider col;

    public Vector3Int[] GetOccupiedCells(Vector3Int gridPosition)
    {
        Vector3Int[] occupiedCells = new Vector3Int[size.x * size.y];
        int index = 0;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                occupiedCells[index] = gridPosition + new Vector3Int(x, 0, y);
                index++;
            }
        }

        return occupiedCells;
    }

    public void RotateBuilding()
    {
        rotationIndex = (rotationIndex + 1) % 4; // Cycles through 0°, 90°, 180°, 270°
        transform.rotation = Quaternion.Euler(0, rotationIndex * 90, 0);

        // Swap width & height when rotating 90° or 270°
        if (rotationIndex % 2 == 1)
        {
            size = new Vector2Int(size.y, size.x);
        }
    }
    public void OnPlaced()
    {
        col.enabled = true;
    }
}
