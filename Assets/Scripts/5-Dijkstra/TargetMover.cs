using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TargetMover : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private AllowedTiles allowedTiles = null;
    [SerializeField] private TileWeightConfig tileWeightConfig = null;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int maxIterations = 1000;
    [SerializeField] private UIMessageManager uiMessageManager;
    [SerializeField] private GameObject uiPanel; // Reference to the UI panel (e.g., start panel)

    private Vector3 targetInWorld;
    private Vector3Int targetInGrid;
    public bool atTarget = true;
    private TilemapGraph tilemapGraph = null;
    private float timeBetweenSteps;
    private Dictionary<TileBase, float> tileWeights;

    public void SetTarget(Vector3 newTarget)
    {
        if (uiPanel != null && uiPanel.activeSelf)
        {
            Debug.Log("Click ignored because UI panel is active.");
            return;
        }

        if (IsClickOverUI())
        {
            Debug.Log("Click detected over UI. Ignoring...");
            return;
        }

        Vector3Int gridPosition = tilemap.WorldToCell(newTarget);

        if (!tilemap.HasTile(gridPosition))
        {
            Debug.LogWarning("Invalid Target: Clicked outside the tilemap.");
            if (uiMessageManager != null)
            {
                uiMessageManager.ShowMessage("Invalid click! Please click on a valid tile.");
            }
            return;
        }

        Vector3Int currentGridPosition = tilemap.WorldToCell(transform.position);

        if (gridPosition == currentGridPosition)
        {
            Debug.LogWarning("Invalid Target: You are already on this tile.");
            if (uiMessageManager != null)
            {
                uiMessageManager.ShowMessage("Invalid target! You are already on this tile.");
            }
            return;
        }

        TileBase clickedTile = tilemap.GetTile(gridPosition);
        if (!allowedTiles.Contains(clickedTile))
        {
            Debug.LogWarning($"Invalid Target: Tile '{clickedTile?.name}' is not allowed.");
            if (uiMessageManager != null)
            {
                uiMessageManager.ShowMessage($"Invalid tile: '{clickedTile?.name}'. Movement not allowed.");
            }
            return;
        }

        targetInWorld = tilemap.GetCellCenterWorld(gridPosition);
        targetInGrid = gridPosition;
        atTarget = false;

        Debug.Log($"Valid target set to {gridPosition} ({clickedTile?.name}).");
        StartCoroutine(MoveTowardsTheTarget());
    }

    public Vector3 GetTarget()
    {
        return targetInWorld;
    }

    private bool IsClickOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    protected virtual void Start()
    {
        if (tileWeightConfig == null)
        {
            Debug.LogError("Tile Weight Config is missing! Please assign it in the Inspector.");
            return;
        }

        tileWeights = tileWeightConfig.GetTileWeightsDictionary();

        if (tileWeights == null || tileWeights.Count == 0)
        {
            Debug.LogError("Tile weights are empty or invalid! Please configure the Tile Weight Config.");
            return;
        }

        tilemapGraph = new TilemapGraph(tilemap, allowedTiles.Get(), tileWeights);
        timeBetweenSteps = 1 / speed;

        Debug.Log("TargetMover initialized and waiting for game start.");
    }

    IEnumerator MoveTowardsTheTarget()
    {
        while (!atTarget)
        {
            yield return new WaitForSeconds(timeBetweenSteps);
            if (!atTarget)
                MakeOneStepTowardsTheTarget();
        }
    }

    private void MakeOneStepTowardsTheTarget()
    {
        Vector3Int startNode = tilemap.WorldToCell(transform.position);
        Vector3Int endNode = targetInGrid;

        List<Vector3Int> shortestPath = Dijkstra.GetPath(
            tilemapGraph,
            startNode,
            endNode,
            tilemapGraph.GetEdgeWeight,
            maxIterations
        );

        if (shortestPath.Count >= 2)
        {
            Vector3Int nextNode = shortestPath[1];
            transform.position = tilemap.GetCellCenterWorld(nextNode);

            if (nextNode == targetInGrid)
            {
                atTarget = true;
                Debug.Log("Reached the target!");
            }
        }
        else
        {
            Debug.LogError($"No path found between {startNode} and {endNode}");
            atTarget = true;
        }
    }
}
