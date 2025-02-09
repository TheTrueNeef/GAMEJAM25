using UnityEngine;

public class GridTest : MonoBehaviour
{
    public GameObject cube, blockPrefab;
    public Grid grid;
    public GridTestInput gridInput;

    private void Update()
    {
        Vector3 selectedPosition = gridInput.GetSelectedMapPosition();
        Vector3Int cellPosition = grid.WorldToCell(selectedPosition);
        cube.transform.position = grid.GetCellCenterWorld(cellPosition);

        if (gridInput.GetPlacementInput())
        {
            Instantiate(blockPrefab, cube.transform.position, Quaternion.identity);
        }
    }
}
