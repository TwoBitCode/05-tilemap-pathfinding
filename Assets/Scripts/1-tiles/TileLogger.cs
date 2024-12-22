using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component detects the tile the player is standing on and handles item tile interactions.
 */
public class TileLogger : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private Tilemap tilemap = null;

    [Header("Output")]
    [SerializeField] private Vector3Int cellPosition;
    [SerializeField] private TileBase tile = null;
    [SerializeField] private string tileName = null;

    void Update()
    {
        // Get the player's current cell position
        cellPosition = tilemap.WorldToCell(transform.position);
        tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            tileName = tile.name;

            // Check if the tile is an item tile and handle pickup
            HandleItemTile(tileName);
        }
    }

    private void HandleItemTile(string tileName)
    {
        AllowedTiles allowedTiles = GetComponent<AllowedTiles>();
        if (allowedTiles == null) return;

        switch (tileName)
        {
            case "BoatTile":
                allowedTiles.AddTile(TileManager.Instance.GetDeepSeaTile());
                allowedTiles.AddTile(TileManager.Instance.GetMediumSeaTile());
                Debug.Log("Boat collected! You can now sail on medium sea and deep sea.");
                break;

            case "GoatTile":
                allowedTiles.AddTile(TileManager.Instance.GetMountainTile());
                Debug.Log("Goat collected! You can now climb mountains.");
                break;

            case "PickaxeTile":
                PlayerInteraction.Instance.EnableTileModification(TileManager.Instance.GetMountainTile(), TileManager.Instance.GetGrassTile());
                Debug.Log("Pickaxe collected! You can now carve mountains into grass.");
                break;

            default:
                return; // Not an item tile
        }

        // Remove the tile from the tilemap after pickup
        RemoveTile(cellPosition);
    }


    private void RemoveTile(Vector3Int position)
    {
        tilemap.SetTile(position, null);
    }
}
