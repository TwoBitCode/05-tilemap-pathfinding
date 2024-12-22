using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileWeightConfig", menuName = "Pathfinding/Tile Weight Config")]
public class TileWeightConfig : ScriptableObject
{
    public List<TileWeightEntry> tileWeights;

    [System.Serializable]
    public class TileWeightEntry
    {
        public TileBase tile;
        public float weight;
    }

    public Dictionary<TileBase, float> GetTileWeightsDictionary()
    {
        var dict = new Dictionary<TileBase, float>();
        foreach (var entry in tileWeights)
        {
            if (entry.tile != null)
            {
                dict[entry.tile] = entry.weight;
            }
        }
        return dict;
    }
}
