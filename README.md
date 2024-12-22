# 🗺️ Tilemap Pathfinding Game

This project is based on **Unity Week 5: Two-dimensional Scene-building and Path-finding**. It demonstrates how to construct a 2D scene using tilemaps and implements path-finding using both **BFS** and **Dijkstra's algorithm**. 

📖 **Text explanations** for the foundational concepts are available [here](https://github.com/gamedev-at-ariel/gamedev-5782) in folder 07.

---

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
    // Initialize with only grass and swamp as allowed tiles
    allowedTileList = new List<TileBase> { grassTile, swampTile };
    Debug.Log("Initial allowed tiles: " + string.Join(", ", allowedTileList));
}

public void AddTile(TileBase tile)
{
    if (!allowedTileList.Contains(tile))
    {
        allowedTileList.Add(tile);
        Debug.Log($"Tile '{tile.name}' added to allowed tiles.");
    }
}
```

- **Highlights**: 
  - Starts with **grass** and **swamp** as valid movement tiles.
  - Updates dynamically when power-ups are collected.

---

### **2. ItemTileDetector** 🎯

**Purpose**: Detects when the player interacts with item tiles and updates allowed movement.

```csharp
switch (tileName)
{
    case "BoatTile":
        allowedTiles.AddTile(TileManager.Instance.GetDeepSeaTile());
        allowedTiles.AddTile(TileManager.Instance.GetMediumSeaTile());
        Debug.Log("Boat collected! You can now sail on water.");
        break;
    case "GoatTile":
        allowedTiles.AddTile(TileManager.Instance.GetMountainTile());
        Debug.Log("Goat collected! Mountains are now passable.");
        break;
    case "PickaxeTile":
        PlayerInteraction.Instance.EnableTileModification(
            TileManager.Instance.GetMountainTile(), TileManager.Instance.GetGrassTile()
        );
        Debug.Log("Pickaxe collected! You can now transform mountains into grass.");
        break;
}
```

---

### **3. StartGameManager** 🖼️

**Purpose**: Manages the start and game-over states, including transitions between scenes.

```csharp
public void ShowGameOver(string message)
{
    if (gameOverPanel != null)
    {
        gameOverPanel.SetActive(true);

        if (gameOverText != null)
        {
            gameOverText.text = message; // Display the specific Game Over message
        }
    }

    Time.timeScale = 0; // Pause the game
    StartCoroutine(WaitAndLoadNextScene());
}
```

- **Highlights**:
  - Displays a specific **Game Over** message when the player steps on an invalid tile.
  - Automatically transitions to the next scene after a delay.

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
    // ... Implementation continues
}
```

- **Highlights**:
  - Implements **Dijkstra’s algorithm** to find the least-cost path in a weighted graph.
  - Uses the `TilemapGraph` to represent the tilemap.

---

## 🎮 How to Play

### **Part 1: Tilemap Movement**
1. Use the **arrow keys** to move the player.
2. Collect items to unlock movement across specific tiles:
   - **Boat**: Sail on water.
   - **Goat**: Climb mountains.
   - **Pickaxe**: Transform mountains into grass.
3. Step on an invalid tile to transition to the next part.

### **Part 2: Dijkstra Navigation**
1. Click on a valid target tile.
2. The player will move to the target using the fastest path based on tile weights.
3. Invalid clicks display a warning but do not end the game.

---
