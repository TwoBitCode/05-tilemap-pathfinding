using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardMoverByTile : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private AllowedTiles allowedTiles = null;
    [SerializeField] private StartGameManager gameManager; // Reference to GameManager
    private bool isGameOver = false; // Flag to track game state

    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Update()
    {
        if (isGameOver) return; // Prevent further movement if the game is over

        Vector3 newPosition = transform.position + NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        if (allowedTiles.Contains(tileOnNewPosition))
        {
            transform.position = newPosition;
        }
        else if (tileOnNewPosition != null) // Trigger game over for invalid tiles
        {
            string tileName = tileOnNewPosition?.name ?? "unknown tile";
            Debug.Log($"Game Over! You cannot walk on {tileName}.");
            isGameOver = true;
            gameManager.ShowGameOver($"Game Over! You stepped on an invalid tile: {tileName}.");
        }
    }

    private Vector3 NewPosition()
    {
        Vector3 offset = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            offset = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            offset = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            offset = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            offset = Vector3.right;
        }

        return offset;
    }
}
