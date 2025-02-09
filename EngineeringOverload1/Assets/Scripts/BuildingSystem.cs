using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class BuildingSystem : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject previewPrefab;
    public GridManager gridManager;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;
    public GameObject gridVisual;
    public GameObject buildingUI; // Assign in Unity Inspector

    public int gridSize = 10;
    public float cellSize = 1.0f;

    private GameObject previewObject;
    private Vector3Int[] occupiedCells;
    private bool canPlace = false;
    private int rotationAngle = 0;
    private bool isBuildingMode = false; // Track if building system is active
    private int groundLayerMask;

    void Start()
    {
        groundLayerMask = LayerMask.GetMask("GroundGrid"); // Ensure the correct layer is set

        if (gridVisual != null)
        {
            gridVisual.SetActive(false); // Start with the grid hidden
        }
        if (buildingUI != null)
        {
            buildingUI.SetActive(false); // Start with the UI hidden
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildingMode();
        }

        if (!isBuildingMode) return; // Disable all building logic when not in build mode

        HandlePreviewPlacement();
        HandleRotation();
        HandleBuildingPlacement();
    }

    void ToggleBuildingMode()
    {
        isBuildingMode = !isBuildingMode;

        if (gridVisual != null)
        {
            gridVisual.SetActive(isBuildingMode);
        }

        if (buildingUI != null)
        {
            buildingUI.SetActive(isBuildingMode);
        }

        if (isBuildingMode)
        {
            CreatePreviewObject();
        }
        else
        {
            if (previewObject != null) Destroy(previewObject);
        }
    }

    void CreatePreviewObject()
    {
        if (previewObject == null && previewPrefab != null)
        {
            previewObject = Instantiate(previewPrefab);
        }
    }

    void HandlePreviewPlacement()
    {
        if (previewObject == null) return;

        Vector3 mousePosition = GetMouseWorldPosition();
        if (mousePosition == Vector3.zero) return; // Prevent invalid placement

        Vector3Int gridPosition = gridManager.WorldToGrid(mousePosition);

        previewObject.transform.position = gridManager.GridToWorld(gridPosition);
        previewObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);

        occupiedCells = gridManager.GetOccupiedCells(gridPosition, previewPrefab, rotationAngle);
        canPlace = gridManager.CanPlace(occupiedCells);
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotationAngle += 90;
            if (rotationAngle >= 360)
                rotationAngle = 0;

            if (previewObject != null)
                previewObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }

    void HandleBuildingPlacement()
    {
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            PlaceBuilding(occupiedCells, previewObject.transform.position);
        }
    }

    void PlaceBuilding(Vector3Int[] occupiedCells, Vector3 position)
    {
        if (IsPlayerInside(position, buildingPrefab))
        {
            return;
        }

        GameObject newBuilding = Instantiate(buildingPrefab, position, previewObject.transform.rotation);
        Building buildingComponent = newBuilding.GetComponent<Building>();

        if (buildingComponent == null)
        {
            Debug.LogError("Building component is missing on the instantiated prefab!", newBuilding);
            return;
        }

        foreach (Vector3Int cell in occupiedCells)
        {
            gridManager.OccupyCell(cell);
        }

        buildingComponent.OnPlaced();
    }

    bool IsPlayerInside(Vector3 position, GameObject buildingPrefab)
    {
        Collider buildingCollider = buildingPrefab.GetComponentInChildren<Collider>();
        if (buildingCollider == null)
        {
            Debug.LogError("Building prefab is missing a Collider!");
            return false;
        }

        Vector3 buildingSize = buildingCollider.bounds.size;
        Vector3 center = position + Vector3.up * (buildingSize.y / 2);
        Collider[] colliders = Physics.OverlapBox(center, buildingSize / 2, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
