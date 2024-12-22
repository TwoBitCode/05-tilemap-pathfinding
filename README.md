# 🗺️ Tilemap Pathfinding Game

This project demonstrates 2D scene construction using tilemaps and pathfinding algorithms in Unity. It builds on concepts from **Unity Week 5: Two-dimensional Scene-building and Pathfinding**, incorporating both **BFS** and **Dijkstra's algorithm** for navigation.

📖 **Foundational text explanations** are available [here](https://github.com/gamedev-at-ariel/gamedev-5782) in folder 07.

---

### Play the Game on Itch.io
You can play the game directly from our [Itch.io page](https://twobitcode.itch.io/05-tilemap-pathfinding).


## 🕹️ Gameplay Overview

### **Part 1: Tilemap Movement with Helpers**
- **🎯 Goal**: Navigate the tilemap and collect items to unlock movement on specific tiles.
- **🛠️ Power-Ups**:
  - 🛶 **Boat**: Allows movement across water tiles.
  - 🐐 **Goat**: Enables traversal of mountain tiles.
  - ⛏️ **Pickaxe**: Transforms mountain tiles into grass.
- **⚠️ Rules**:
  - Without power-ups, the player can only walk on **grass** and **swamp** tiles.
  - Stepping on an invalid tile ends the game and transitions to the next part.

---

### **Part 2: Click-Based Movement with Dijkstra**
- **🎯 Goal**: Click on a valid target tile to navigate the map using the fastest path.
- **🛠️ Features**:
  - Each tile type has a specific movement cost.
  - The game uses **Dijkstra’s algorithm** to calculate the least-cost path.
  - Invalid clicks display a warning but do not end the game.

---

## 🛠️ Code Architecture

### **1. AllowedTiles** 🟩

**Purpose**: Manages the tiles the player is allowed to walk on.

```csharp
void Awake()
{
    InitializeAllowedTiles();
}

private void InitializeAllowedTiles()
{
    allowedTileSet = new HashSet<TileBase>(defaultAllowedTiles);
    Debug.Log("Initial allowed tiles: " + string.Join(", ", allowedTileSet));
}

public void AddTile(TileBase tile)
{
    if (tile != null && !allowedTileSet.Contains(tile))
    {
        allowedTileSet.Add(tile);
        Debug.Log($"Tile '{tile.name}' added to allowed tiles.");
    }
}
```

---

### **2. ItemTileDetector** 🎯

**Purpose**: Detects when the player interacts with item tiles and updates allowed movement.

```csharp
switch (tileName)
{
    case "BoatTile":
        allowedTiles.AddTile(TileManager.Instance.GetDeepSeaTile());
        Debug.Log("Boat collected! You can now sail on water.");
        break;

    case "GoatTile":
        allowedTiles.AddTile(TileManager.Instance.GetMountainTile());
        Debug.Log("Goat collected! Mountains are now passable.");
        break;

    case "PickaxeTile":
        PlayerInteraction.Instance.EnableTileModification(
            TileManager.Instance.GetMountainTile(),
            TileManager.Instance.GetGrassTile()
        );
        Debug.Log("Pickaxe collected! You can now transform mountains into grass.");
        break;
}
```

---

### **3. TilemapGraph** 🔗

**Purpose**: Represents the tilemap as a graph for pathfinding. Provides neighbors and edge weights for Dijkstra's Algorithm.

```csharp
public float GetEdgeWeight(Vector3Int from, Vector3Int to)
{
    TileBase tile = tilemap.GetTile(to);
    if (tile != null && tileWeights.ContainsKey(tile))
    {
        return tileWeights[tile];
    }
    return float.MaxValue; // Treat unknown tiles as impassable
}

public IEnumerable<Vector3Int> Neighbors(Vector3Int node)
{
    foreach (var direction in directions)
    {
        Vector3Int neighborPos = node + direction;
        TileBase neighborTile = tilemap.GetTile(neighborPos);

        if (neighborTile != null)
            yield return neighborPos;
    }
}
```

---

### **4. Dijkstra Algorithm Integration** 📈

**Purpose**: Calculates the least-cost path for click-based navigation.

```csharp
public static List<NodeType> GetPath<NodeType>(
    IGraph<NodeType> graph,
    NodeType startNode,
    NodeType endNode,
    Func<NodeType, NodeType, float> getEdgeWeight,
    int maxIterations = 1000
)
{
    var distances = new Dictionary<NodeType, float>();
    var previousNodes = new Dictionary<NodeType, NodeType>();
    var visited = new HashSet<NodeType>();
    // Implementation continues
}
```

---

### **5. TargetMover** 🎯

**Purpose**: Handles click-based movement on the tilemap. Ensures that clicks are valid and calculates the least-cost path to the target using Dijkstra's Algorithm.

```csharp
public void SetTarget(Vector3 newTarget)
{
    Vector3Int gridPosition = tilemap.WorldToCell(newTarget);

    if (!tilemap.HasTile(gridPosition))
    {
        Debug.LogWarning("Invalid Target: Clicked outside the tilemap.");
        return;
    }

    if (gridPosition == tilemap.WorldToCell(transform.position))
    {
        Debug.LogWarning("Invalid Target: You are already on this tile.");
        return;
    }

    targetInWorld = tilemap.GetCellCenterWorld(gridPosition);
    targetInGrid = gridPosition;
    atTarget = false;

    StartCoroutine(MoveTowardsTheTarget());
}
```

---

## 🎮 How to Play

### **Part 1: Tilemap Movement**
1. Use the **arrow keys** to move the player.
2. Collect items to unlock movement across specific tiles:
   - 🛶 **Boat**: Sail on water.
   - 🐐 **Goat**: Climb mountains.
   - ⛏️ **Pickaxe**: Transform mountains into grass.
3. Step on an invalid tile to transition to the next part.

### **Part 2: Dijkstra Navigation**
1. Click on a valid target tile.
2. The player will move to the target using the fastest path based on tile weights.
3. Invalid clicks display a warning but do not end the game.

---
