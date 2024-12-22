using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile. If the player steps
 * on a disallowed tile, the game ends and a "Game Over" message is displayed.
 */
public class KeyboardMoverByTile : KeyboardMover
{
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] GameObject gameOverPanel = null; // Reference to the Game Over UI Panel

    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Update()
    {
        if (gameOverPanel.activeSelf) return; // Stop movement after Game Over

        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        if (tileOnNewPosition == null)
        {
            Debug.LogError("No tile detected under the new position!");
            return;
        }

        if (allowedTiles.Contains(tileOnNewPosition))
        {
            transform.position = newPosition; // Move the player
        }
        else
        {
            Debug.LogError($"Game Over! You cannot walk on {tileOnNewPosition.name}.");
            TriggerGameOver(); // Trigger Game Over logic
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true); // Display the Game Over Panel
    }
}
