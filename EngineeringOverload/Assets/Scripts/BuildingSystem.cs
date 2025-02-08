using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class BuildingSystem : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GridManager gridManager;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;
    public GameObject gridVisual;
    public GameObject buildingUI; // Assign in Unity Inspector

    public int gridSize = 10;
    public float cellSize = 1.0f;

    private GameObject ghostObject;
    private Vector3Int[] occupiedCells;
    private bool canPlace = false;
    private int rotationAngle = 0;
    private bool isBuildingMode = false; // Track if building system is active

    void Start()
    {
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

        HandleGhostPlacement();
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
            CreateGhostObject();
        }
        else
        {
            if (ghostObject != null) Destroy(ghostObject);
        }
    }

    

    void CreateGhostObject()
    {
        if (ghostObject == null)
        {
            ghostObject = Instantiate(buildingPrefab);
            SetObjectTransparency(ghostObject, 0.5f);
        }
    }

    void HandleGhostPlacement()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3Int gridPosition = gridManager.WorldToGrid(mousePosition);

        ghostObject.transform.position = gridManager.GridToWorld(gridPosition);
        ghostObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);

        occupiedCells = gridManager.GetOccupiedCells(gridPosition, buildingPrefab, rotationAngle);
        canPlace = gridManager.CanPlace(occupiedCells);

        ChangeGhostMaterial(canPlace);
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotationAngle += 90;
            if (rotationAngle >= 360)
                rotationAngle = 0;

            ghostObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }

    void HandleBuildingPlacement()
    {
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            PlaceBuilding(occupiedCells, ghostObject.transform.position);
        }
    }

    void PlaceBuilding(Vector3Int[] occupiedCells, Vector3 position)
    {
        if (IsPlayerInside(position, buildingPrefab))
        {
            ChangeGhostMaterial(false);
            return;
        }

        GameObject newBuilding = Instantiate(buildingPrefab, position, ghostObject.transform.rotation);
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
        // Get the building's collider
        Collider buildingCollider = buildingPrefab.GetComponent<Collider>();
        if (buildingCollider == null)
        {
            Debug.LogError("Building prefab is missing a Collider!");
            return false;
        }

        // Calculate the building bounds at the desired position
        Vector3 buildingSize = buildingCollider.bounds.size;
        Vector3 center = position + Vector3.up * (buildingSize.y / 2); // Adjust for height

        // Check for any colliders inside the building area
        Collider[] colliders = Physics.OverlapBox(center, buildingSize / 2, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player")) // Make sure your player has the "Player" tag
            {
                return true; // Player is inside the building area
            }
        }

        return false;
    }

    void ChangeGhostMaterial(bool canPlace)
    {
        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();
        Material newMaterial = canPlace ? validPlacementMaterial : invalidPlacementMaterial;

        foreach (Renderer renderer in renderers)
        {
            renderer.material = newMaterial;
        }
    }

    void SetObjectTransparency(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Color color = renderer.material.color;
            color.a = alpha;

            Material transparentMaterial = new Material(renderer.material);
            transparentMaterial.color = color;
            renderer.material = transparentMaterial;
        }
    }

    Vector3 GetMouseWorldPosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) // Limit the ray distance
        {
            return hit.point;
        }

        return Vector3.zero; // If no valid position is found, return (0,0,0)
    }

}
