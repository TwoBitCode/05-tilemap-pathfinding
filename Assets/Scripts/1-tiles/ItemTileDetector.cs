using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemTileDetector : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase grassTile; // Grass tile to replace helper tiles

    private string currentHelper = null; // Tracks the current active helper

    void Update()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            HandleHelperTile(tile.name, cellPosition); // Detect tile by name
        }
    }

    void HandleHelperTile(string tileName, Vector3Int cellPosition)
    {
        AllowedTiles allowedTiles = GetComponent<AllowedTiles>();
        if (allowedTiles == null) return;

        // If the player steps on a helper tile
        switch (tileName)
        {
            case "BoatTile":
                if (currentHelper != "Boat")
                {
                    ResetHelper(allowedTiles); // Remove the previous helper's effects
                    allowedTiles.AddTile(TileManager.Instance.GetDeepSeaTile()); // Add deep sea tiles
                    allowedTiles.AddTile(TileManager.Instance.GetMediumSeaTile()); // Add medium sea tiles
                    Debug.Log("Boat collected! You can now sail on medium sea and deep sea.");
                    currentHelper = "Boat";

                    // Disable tile modification behavior for Pickaxe
                    PlayerInteraction.Instance.DisableTileModification();
                }
                break;

            case "GoatTile":
                if (currentHelper != "Goat")
                {
                    ResetHelper(allowedTiles); // Remove the previous helper's effects
                    allowedTiles.AddTile(TileManager.Instance.GetMountainTile()); // Add mountain tiles
                    Debug.Log("Goat collected! Mountains are now passable.");
                    currentHelper = "Goat";

                    // Disable tile modification behavior for Pickaxe
                    PlayerInteraction.Instance.DisableTileModification();
                }
                break;

            case "PickaxeTile":
                if (currentHelper != "Pickaxe")
                {
                    ResetHelper(allowedTiles); // Remove the previous helper's effects
                    allowedTiles.AddTile(TileManager.Instance.GetMountainTile()); // Add mountains to allowed tiles
                    PlayerInteraction.Instance.EnableTileModification(TileManager.Instance.GetMountainTile(), TileManager.Instance.GetGrassTile());
                    Debug.Log("Pickaxe collected! You can now step on mountains and modify them into grass.");
                    currentHelper = "Pickaxe";
                }
                break;

            default:
                return; // Not a helper tile
        }

        // Replace the helper tile with grass and remove it from the map
        tilemap.SetTile(cellPosition, grassTile);
    }

    void ResetHelper(AllowedTiles allowedTiles)
    {
        // Clear all previously added special tiles
        allowedTiles.ResetToDefault();
        currentHelper = null;
    }
}
