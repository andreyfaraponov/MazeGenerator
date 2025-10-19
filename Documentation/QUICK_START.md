# Quick Start Guide - Updated API

## Creating Mazes

### Simple Creation (Backward Compatible)
```csharp
// Constructor - uses Eller's algorithm by default
var maze = new Maze(20, 20);

// With algorithm
var maze = new Maze(20, 20, MazeAlgorithmType.RecursiveBacktracker);

// With seed
var maze = new Maze(20, 20, MazeAlgorithmType.Prim, seed: 42);
```

### Factory Creation (Recommended)
```csharp
using MazeGenerator;
using MazeGenerator.Enums;

// Basic factory
var maze = MazeFactory.Create(20, 20);

// With algorithm
var maze = MazeFactory.Create(20, 20, MazeAlgorithmType.Prim);

// With seed
var maze = MazeFactory.Create(20, 20, MazeAlgorithmType.Eller, seed: 42);
```

### Semantic Factory Methods (Best for Intent)
```csharp
using MazeGenerator;

// For puzzle games - long winding corridors
var puzzle = MazeFactory.CreateChallenging(30, 30);

// For adventure games - many branching paths
var adventure = MazeFactory.CreateExploratory(40, 40);

// For dungeon generation - room-like layout
var dungeon = MazeFactory.CreateDungeon(50, 50);

// General purpose - balanced characteristics
var general = MazeFactory.CreateBalanced(25, 25);

// For research - unbiased generation
var research = MazeFactory.CreateUnbiased(20, 20);

// With loops - multiple solution paths
var braided = MazeFactory.CreateBraided(30, 30, braidingFactor: 0.5);

// Random algorithm - variety
var random = MazeFactory.CreateRandom(25, 25);

// Auto-optimized - best for size
var optimized = MazeFactory.CreateOptimized(100, 100);
```

### With Reproducibility
```csharp
// Add seed for reproducible generation
var maze1 = MazeFactory.CreateChallenging(30, 30, seed: 42);
var maze2 = MazeFactory.CreateChallenging(30, 30, seed: 42);
// maze1 and maze2 are identical
```

---

## Accessing Maze Data

```csharp
var maze = MazeFactory.CreateDungeon(20, 20);

// Get dimensions
int width = maze.Width;        // 20
int height = maze.Height;      // 20

// Get all cells
var cells = maze.Cells;        // IReadOnlyList<IReadOnlyList<Cell>>

// Get specific cell
Cell cell = maze.GetCell(5, 10);

// Check walls
bool hasLeftWall = cell.Left;
bool hasRightWall = cell.Right;
bool hasTopWall = cell.Top;
bool hasBottomWall = cell.Bottom;

// Get configuration
var config = maze.Configuration;
var algorithm = config.Algorithm;  // RecursiveDivision
```

---

## Complete Examples

### Example 1: Game Level Generator
```csharp
using MazeGenerator;

public class LevelGenerator
{
    public Maze GenerateLevel(int level)
    {
        // Progressive difficulty
        if (level <= 3)
            return MazeFactory.CreateExploratory(15, 15);  // Easy
        else if (level <= 7)
            return MazeFactory.CreateBalanced(20, 20);     // Medium
        else
            return MazeFactory.CreateChallenging(25, 25);  // Hard
    }
}
```

### Example 2: Procedural World
```csharp
using MazeGenerator;

public class WorldGenerator
{
    public Maze GenerateRegion(int regionX, int regionY)
    {
        // Use coordinates as seed for consistency
        int seed = regionX * 1000 + regionY;
        return MazeFactory.CreateOptimized(50, 50, seed: seed);
    }
}
```

### Example 3: Dungeon Creator
```csharp
using MazeGenerator;

public class DungeonCreator
{
    public Maze CreateDungeon(string difficulty)
    {
        return difficulty switch
        {
            "easy" => MazeFactory.CreateExploratory(20, 20),
            "medium" => MazeFactory.CreateBalanced(30, 30),
            "hard" => MazeFactory.CreateChallenging(40, 40),
            "dungeon" => MazeFactory.CreateDungeon(50, 50),
            _ => MazeFactory.CreateBalanced(25, 25)
        };
    }
}
```

### Example 4: Compare Algorithms
```csharp
using MazeGenerator;
using MazeGenerator.Enums;
using System.Collections.Generic;

public void CompareAllAlgorithms()
{
    int seed = 42;  // Same seed for fair comparison
    var mazes = new Dictionary<string, Maze>
    {
        ["Challenging"] = MazeFactory.CreateChallenging(15, 15, seed),
        ["Exploratory"] = MazeFactory.CreateExploratory(15, 15, seed),
        ["Dungeon"] = MazeFactory.CreateDungeon(15, 15, seed),
        ["Balanced"] = MazeFactory.CreateBalanced(15, 15, seed),
        ["Unbiased"] = MazeFactory.CreateUnbiased(15, 15, seed),
    };
    
    foreach (var kvp in mazes)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value.Width}x{kvp.Value.Height}");
    }
}
```

---

## API Quick Reference

### MazeFactory Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Create(config)` | Maze | Create with configuration |
| `Create(w, h)` | Maze | Create with defaults |
| `Create(w, h, alg)` | Maze | Create with algorithm |
| `Create(w, h, alg, seed)` | Maze | Create with seed |
| `CreateChallenging(w, h, seed?)` | Maze | Long corridors |
| `CreateExploratory(w, h, seed?)` | Maze | Many branches |
| `CreateDungeon(w, h, seed?)` | Maze | Room-like |
| `CreateBalanced(w, h, seed?)` | Maze | General purpose |
| `CreateUnbiased(w, h, seed?)` | Maze | Uniform distribution |
| `CreateBraided(w, h, factor, seed?)` | Maze | With loops |
| `CreateRandom(w, h, seed?)` | Maze | Random algorithm |
| `CreateOptimized(w, h, seed?)` | Maze | Auto-select |

### Maze Properties

| Property | Type | Description |
|----------|------|-------------|
| `Cells` | `IReadOnlyList<IReadOnlyList<Cell>>` | All cells |
| `Width` | `int` | Number of columns |
| `Height` | `int` | Number of rows |
| `Configuration` | `MazeConfiguration` | Config used |
| `Grid` | `MazeGrid` | Underlying grid |

### Maze Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `GetCell(row, col)` | `Cell` | Get specific cell |

---

## Tips

✅ **Use MazeFactory** for creating mazes (not `Maze.CreateXxx()`)
✅ **Use constructors** for backward compatibility or simple cases
✅ **Use semantic methods** for clear intent (CreateChallenging, etc.)
✅ **Add seeds** for reproducible generation
✅ **Use CreateOptimized()** when unsure which algorithm to use
✅ **Use CreateRandom()** for variety in procedural generation

❌ **Don't use** `Maze.CreateXxx()` - these have been moved to MazeFactory
❌ **Don't forget** to add `using MazeGenerator;`
❌ **Don't use** CreateUnbiased() for large mazes (>50x50) - it's slow
