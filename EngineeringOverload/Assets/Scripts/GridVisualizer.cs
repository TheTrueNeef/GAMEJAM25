using UnityEngine;
using UnityEngine.Tilemaps;

public class GridVisualizer : MonoBehaviour
{
    private Tilemap tilemap;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void ToggleGrid(bool state)
    {
        tilemap.gameObject.SetActive(state);
    }
}
