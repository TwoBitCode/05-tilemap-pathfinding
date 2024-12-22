using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    [SerializeField] private Tilemap tilemap;
   

    private TileBase modifiableTile;
    private TileBase replacementTile;
    private Vector3Int lastTilePosition; // Tracks the last tile position the player was on

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableTileModification(TileBase modifiable, TileBase replacement)
    {
        modifiableTile = modifiable;
        replacementTile = replacement;
        Debug.Log($"Enabling modification: {modifiable?.name} → {replacement?.name}");
    }

    public void DisableTileModification()
    {
        modifiableTile = null;
        replacementTile = null;
        Debug.Log("Tile modification disabled.");
    }

    private void Update()
    {
        Vector3Int currentTilePosition = tilemap.WorldToCell(transform.position);
        TileBase currentTile = tilemap.GetTile(currentTilePosition);

        // Check if the player moved to a new tile
        if (currentTilePosition != lastTilePosition)
        {
            TileBase lastTile = tilemap.GetTile(lastTilePosition);


            // Replace the last tile if it was modifiable
            if (lastTile == modifiableTile)
            {
                tilemap.SetTile(lastTilePosition, replacementTile);
                Debug.Log($"Replaced {lastTile.name} with {replacementTile.name} at {lastTilePosition}");
            }

            lastTilePosition = currentTilePosition; // Update the last position
        }
    }



    public bool IsTileAllowed(TileBase tile)
    {
        AllowedTiles allowedTiles = GetComponent<AllowedTiles>();
        if (allowedTiles == null)
        {
            Debug.LogError("AllowedTiles component not found on the GameObject!");
            return false;
        }

        return allowedTiles.Contains(tile);
    }
}
