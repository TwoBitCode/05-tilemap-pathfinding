using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AllowedTiles : MonoBehaviour
{
    [Header("Initial Allowed Tiles")]
    [SerializeField] private TileBase[] defaultAllowedTiles = null;

    private HashSet<TileBase> allowedTileSet; // Use HashSet for faster lookup

    void Awake()
    {
        InitializeAllowedTiles();
    }

    // Initialize or reinitialize the allowed tiles to the default set
    private void InitializeAllowedTiles()
    {
        allowedTileSet = new HashSet<TileBase>(defaultAllowedTiles);
        Debug.Log("Initial allowed tiles: " + string.Join(", ", allowedTileSet));
    }

    public bool Contains(TileBase tile)
    {
        return allowedTileSet.Contains(tile);
    }

    public void AddTile(TileBase tile)
    {
        if (tile != null && !allowedTileSet.Contains(tile))
        {
            allowedTileSet.Add(tile);
            Debug.Log($"Tile '{tile.name}' added to allowed tiles.");
        }
    }

    public void ResetToDefault()
    {
        InitializeAllowedTiles(); // Reset to the default tiles
        Debug.Log("Allowed tiles have been reset to default.");
    }

    public TileBase[] Get()
    {
        return new List<TileBase>(allowedTileSet).ToArray();
    }
}
