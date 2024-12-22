using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AllowedTiles : MonoBehaviour
{
    [Header("Initial Allowed Tiles")]
    [SerializeField] private TileBase[] defaultAllowedTiles = null;

    private List<TileBase> allowedTileList;

    void Awake()
    {
        // Initialize the allowedTileList with the default tiles
        allowedTileList = new List<TileBase>(defaultAllowedTiles);
        Debug.Log("Initial allowed tiles: " + string.Join(", ", allowedTileList));
    }

    public bool Contains(TileBase tile)
    {
        return allowedTileList.Contains(tile);
    }

    public void AddTile(TileBase tile)
    {
        if (!allowedTileList.Contains(tile))
        {
            allowedTileList.Add(tile);
            Debug.Log($"Tile '{tile.name}' added to allowed tiles.");
        }
    }

    public void ResetToDefault()
    {
        // Reset to the default allowed tiles
        allowedTileList = new List<TileBase>(defaultAllowedTiles);
        Debug.Log("Allowed tiles reset to default.");
    }

    public TileBase[] Get()
    {
        return allowedTileList.ToArray();
    }
}
