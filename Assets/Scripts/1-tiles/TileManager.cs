using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    [SerializeField] private TileBase deepSeaTile;
    [SerializeField] private TileBase mediumSeaTile;
    [SerializeField] private TileBase mountainTile;
    [SerializeField] private TileBase grassTile;

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

    public TileBase GetDeepSeaTile() => deepSeaTile;
    public TileBase GetMediumSeaTile() => mediumSeaTile;
    public TileBase GetMountainTile() => mountainTile;
    public TileBase GetGrassTile() => grassTile;
}
