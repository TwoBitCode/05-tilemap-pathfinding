using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TilemapGraph : IGraph<Vector3Int>
{
    private Tilemap tilemap;
    private Dictionary<TileBase, float> tileWeights;

    // Updated Constructor to Include Tile Weights
    public TilemapGraph(Tilemap tilemap, TileBase[] allowedTiles, Dictionary<TileBase, float> tileWeights)
    {
        this.tilemap = tilemap;
        this.tileWeights = tileWeights;
    }

    // Directions for neighbors
    static Vector3Int[] directions = {
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 1, 0),
    };

    // Returns all valid neighbors
    public IEnumerable<Vector3Int> Neighbors(Vector3Int node)
    {
        foreach (var direction in directions)
        {
            Vector3Int neighborPos = node + direction;
            TileBase neighborTile = tilemap.GetTile(neighborPos);

            if (neighborTile != null) // Include all tiles
                yield return neighborPos;
        }
    }

    // Returns the weight (cost) of moving to a neighboring tile
    public float GetEdgeWeight(Vector3Int from, Vector3Int to)
    {
        TileBase tile = tilemap.GetTile(to);
        if (tile != null && tileWeights.ContainsKey(tile))
        {
            return tileWeights[tile]; // Return the weight from the dictionary
        }
        return float.MaxValue; // Treat unknown tiles as impassable
    }
}
