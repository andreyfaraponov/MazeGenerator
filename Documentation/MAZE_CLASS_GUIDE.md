# Maze Class - Usage Guide

## Overview

The `Maze` class has been enhanced to support creating various types of mazes with different algorithms, while maintaining full backward compatibility with existing code.

## Features

✅ **Backward Compatible** - Existing code continues to work
✅ **Multiple Constructors** - Choose complexity level based on needs
✅ **Factory Methods** - Semantic methods for specific maze types
✅ **Algorithm Selection** - Support for all 5 algorithms
✅ **Reproducible Mazes** - Optional seed parameter
✅ **Smart Optimization** - Automatic algorithm selection

---

## Basic Usage (Backward Compatible)

### Default Maze
```csharp
// Original constructor still works - uses Eller's algorithm
var maze = new Maze(20, 20);
```

---

## Constructors

### 1. Simple Constructor (Backward Compatible)
```csharp
var maze = new Maze(width: 20, height: 20);
// Uses Eller's algorithm by default
```

### 2. With Algorithm Selection
```csharp
using MazeGenerator.Enums;

var maze = new Maze(
    columnsCount: 20, 
    rowsCount: 20, 
    algorithm: MazeAlgorithmType.RecursiveBacktracker
);
```

### 3. With Algorithm and Seed
```csharp
var maze = new Maze(
    columnsCount: 20, 
    rowsCount: 20, 
    algorithm: MazeAlgorithmType.Prim,
    seed: 42  // Reproducible maze
);
```

### 4. With Configuration
```csharp
var config = new MazeConfiguration
{
    Width = 20,
    Height = 20,
    Algorithm = MazeAlgorithmType.RecursiveDivision,
    Type = MazeType.Perfect,
    Seed = 12345
};

var maze = new Maze(config);
```

---

## Factory Methods

The `Maze` class provides semantic factory methods for creating specific types of mazes:

### CreateChallenging() - For Puzzle Games
```csharp
// Creates long, winding corridors (Recursive Backtracker)
var maze = Maze.CreateChallenging(width: 30, height: 30);

// With seed for reproducibility
var maze2 = Maze.CreateChallenging(30, 30, seed: 42);
```
**Best for:** Puzzle games, challenging mazes, games requiring long solution paths

**Characteristics:**
- Long winding passages
- Fewer branches
- Harder to solve
- More strategic thinking required

### CreateExploratory() - For Adventure Games
```csharp
// Creates many short branches (Prim's Algorithm)
var maze = Maze.CreateExploratory(width: 30, height: 30);

// With seed
var maze2 = Maze.CreateExploratory(30, 30, seed: 100);
```
**Best for:** Exploration games, RPGs, adventures with many paths

**Characteristics:**
- Many short dead ends
- High branching factor
- More exploration opportunities
- Multiple apparent paths

### CreateDungeon() - For Dungeon Layouts
```csharp
// Creates room-like chambers (Recursive Division)
var maze = Maze.CreateDungeon(width: 40, height: 40);

// With seed
var maze2 = Maze.CreateDungeon(40, 40, seed: 999);
```
**Best for:** Dungeon generators, room-based games, architectural layouts

**Characteristics:**
- Distinct rooms/chambers
- Corridors between rooms
- Structured appearance
- Cross-shaped intersections

### CreateBalanced() - General Purpose
```csharp
// Creates balanced maze (Eller's Algorithm)
var maze = Maze.CreateBalanced(width: 25, height: 25);

// With seed
var maze2 = Maze.CreateBalanced(25, 25, seed: 7);
```
**Best for:** General purpose, default choice, memory-efficient

**Characteristics:**
- Good balance of all traits
- Memory efficient
- Fast generation
- Reliable performance

### CreateUnbiased() - For Research
```csharp
// Creates truly unbiased maze (Aldous-Broder)
var maze = Maze.CreateUnbiased(width: 15, height: 15);

// Note: Slower for large mazes
var maze2 = Maze.CreateUnbiased(15, 15, seed: 555);
```
**Best for:** Research, statistical analysis, when uniformity matters

**Characteristics:**
- Uniform distribution
- No algorithm bias
- Statistical properties guaranteed
- Slower generation (especially large mazes)

⚠️ **Warning:** Not recommended for mazes larger than 50x50 due to slow generation time.

### CreateBraided() - Multiple Solution Paths
```csharp
// Creates maze with loops (removes 50% of dead ends)
var maze = Maze.CreateBraided(width: 20, height: 20);

// Custom braiding factor (0.0 to 1.0)
var maze2 = Maze.CreateBraided(20, 20, braidingFactor: 0.7);

// With seed
var maze3 = Maze.CreateBraided(20, 20, braidingFactor: 0.5, seed: 123);
```
**Best for:** Games where multiple paths are desired, less frustrating exploration

**Characteristics:**
- Contains loops
- Multiple solution paths
- Fewer dead ends
- Less frustrating for players

**Braiding Factor:**
- `0.0` = Perfect maze (no loops)
- `0.5` = Remove half the dead ends
- `1.0` = Remove all dead ends (maximum loops)

### CreateRandom() - For Variety
```csharp
// Randomly selects an algorithm
var maze = Maze.CreateRandom(width: 20, height: 20);

// With seed (selects algorithm based on seed)
var maze2 = Maze.CreateRandom(20, 20, seed: 42);
```
**Best for:** When you want variety, procedural generation with surprises

**Characteristics:**
- Different every time (or consistent with seed)
- Unpredictable characteristics
- Good for replayability

### CreateOptimized() - Smart Selection
```csharp
// Automatically selects best algorithm for size
var smallMaze = Maze.CreateOptimized(10, 10);      // Uses Prim
var mediumMaze = Maze.CreateOptimized(100, 100);    // Uses Recursive Backtracker
var largeMaze = Maze.CreateOptimized(1000, 1000);   // Uses Eller

// With seed
var maze = Maze.CreateOptimized(100, 100, seed: 42);
```
**Best for:** When you're unsure which algorithm to use, performance matters

**Selection Logic:**
- **< 100 cells:** Prim (variety)
- **100-10,000 cells:** Recursive Backtracker (good characteristics)
- **10,000-100,000 cells:** Eller (efficiency)
- **> 100,000 cells:** Eller (memory efficiency)

---

## Accessing Maze Properties

### Basic Properties
```csharp
var maze = Maze.CreateChallenging(30, 30);

// Get dimensions
int width = maze.Width;
int height = maze.Height;

// Get cells
var cells = maze.Cells;  // IReadOnlyList<IReadOnlyList<Cell>>

// Get specific cell
Cell cell = maze.GetCell(row: 5, column: 10);

// Get configuration used
MazeConfiguration config = maze.Configuration;

// Get underlying grid (advanced)
MazeGrid grid = maze.Grid;
```

### Cell Properties
```csharp
var cell = maze.GetCell(0, 0);

bool hasLeftWall = cell.Left;
bool hasRightWall = cell.Right;
bool hasTopWall = cell.Top;
bool hasBottomWall = cell.Bottom;
int setNumber = cell.Set;  // Internal algorithm data
```

---

## Complete Examples

### Example 1: Simple Game Integration
```csharp
using MazeGenerator;

public class GameManager
{
    public void StartNewLevel(int levelNumber)
    {
        // Create progressively harder mazes
        Maze maze;
        
        if (levelNumber <= 3)
            maze = Maze.CreateExploratory(15, 15);  // Easy
        else if (levelNumber <= 7)
            maze = Maze.CreateBalanced(20, 20);     // Medium
        else
            maze = Maze.CreateChallenging(25, 25);  // Hard
        
        RenderMaze(maze);
    }
    
    private void RenderMaze(Maze maze)
    {
        for (int y = 0; y < maze.Height; y++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                var cell = maze.GetCell(y, x);
                // Render cell walls...
            }
        }
    }
}
```

### Example 2: Procedural Generation with Seed
```csharp
using MazeGenerator;

public class ProceduralWorld
{
    public Maze GenerateMazeForRegion(int regionX, int regionY)
    {
        // Use region coordinates as seed for consistent generation
        int seed = regionX * 1000 + regionY;
        
        // Same region always generates same maze
        return Maze.CreateOptimized(50, 50, seed: seed);
    }
}
```

### Example 3: Dungeon Generator
```csharp
using MazeGenerator;

public class DungeonGenerator
{
    public Maze GenerateDungeon(int level, int size)
    {
        // Use level as seed for consistency
        var maze = Maze.CreateDungeon(size, size, seed: level);
        
        Console.WriteLine($"Generated dungeon for level {level}");
        Console.WriteLine($"Size: {maze.Width}x{maze.Height}");
        Console.WriteLine($"Algorithm: {maze.Configuration.Algorithm}");
        
        return maze;
    }
}
```

### Example 4: Algorithm Comparison
```csharp
using MazeGenerator;
using MazeGenerator.Enums;

public void CompareAlgorithms()
{
    int seed = 42;  // Same seed for fair comparison
    int size = 20;
    
    var mazes = new Dictionary<string, Maze>
    {
        ["Challenging"] = Maze.CreateChallenging(size, size, seed),
        ["Exploratory"] = Maze.CreateExploratory(size, size, seed),
        ["Dungeon"] = Maze.CreateDungeon(size, size, seed),
        ["Balanced"] = Maze.CreateBalanced(size, size, seed),
        ["Unbiased"] = Maze.CreateUnbiased(size, size, seed)
    };
    
    foreach (var kvp in mazes)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value.Configuration.Algorithm}");
    }
}
```

### Example 5: Custom Configuration
```csharp
using MazeGenerator;
using MazeGenerator.Enums;

public Maze CreateCustomMaze()
{
    var config = new MazeConfiguration
    {
        Width = 50,
        Height = 30,
        Algorithm = MazeAlgorithmType.RecursiveBacktracker,
        Type = MazeType.Perfect,
        Seed = 12345,
        BraidingFactor = 0.0  // Perfect maze (no loops)
    };
    
    return new Maze(config);
}
```

---

## Migration Guide

### Old Code (Still Works!)
```csharp
// This continues to work exactly as before
var maze = new Maze(20, 20);
var cells = maze.Cells;
```

### New Code (More Options)
```csharp
// Use factory methods for better semantics
var maze = Maze.CreateChallenging(20, 20);

// Or specify algorithm directly
var maze2 = new Maze(20, 20, MazeAlgorithmType.Prim);

// Or use full configuration
var config = new MazeConfiguration { Width = 20, Height = 20 };
var maze3 = new Maze(config);
```

---

## Quick Reference Table

| Method | Algorithm | Best For | Speed | Characteristics |
|--------|-----------|----------|-------|-----------------|
| `new Maze(w, h)` | Eller | Backward compat | Fast | Balanced |
| `CreateChallenging()` | Recursive Backtracker | Puzzles | Fast | Long paths |
| `CreateExploratory()` | Prim | Adventure | Medium | Many branches |
| `CreateDungeon()` | Recursive Division | Dungeons | Fast | Room-like |
| `CreateBalanced()` | Eller | General | Fast | Balanced |
| `CreateUnbiased()` | Aldous-Broder | Research | Slow | Uniform |
| `CreateBraided()` | Eller + Braiding | Multiple paths | Fast | Has loops |
| `CreateRandom()` | Random | Variety | Varies | Unpredictable |
| `CreateOptimized()` | Auto-select | Performance | Optimized | Size-dependent |

---

## Tips and Best Practices

### ✅ DO:
- Use factory methods for semantic clarity
- Provide seeds for reproducible mazes
- Use `CreateOptimized()` when unsure
- Test with different algorithms for variety
- Use `CreateChallenging()` for puzzle games

### ❌ DON'T:
- Use `CreateUnbiased()` for large mazes (> 50x50)
- Ignore seed parameter if reproducibility matters
- Use challenging mazes for casual games
- Forget to handle configuration validation errors

---

## Performance Guidelines

### Small Mazes (< 50x50)
- Any algorithm works well
- Choose based on desired characteristics
- Performance not a concern

### Medium Mazes (50x50 to 200x200)
- Use `CreateChallenging()`, `CreateExploratory()`, `CreateDungeon()`, or `CreateBalanced()`
- Avoid `CreateUnbiased()` (too slow)
- Consider using seeds for caching

### Large Mazes (> 200x200)
- Use `CreateBalanced()` or `CreateOptimized()`
- Eller's algorithm is most efficient
- Consider generating in background thread

---

## Summary

The enhanced `Maze` class provides:
- ✅ Full backward compatibility
- ✅ Multiple creation options
- ✅ Semantic factory methods
- ✅ Support for all 5 algorithms
- ✅ Reproducible generation
- ✅ Smart auto-optimization
- ✅ Clear documentation
- ✅ Easy to use API

Choose the method that best fits your use case, from simple `new Maze(w, h)` to advanced configuration with `CreateCustomMaze()`.
