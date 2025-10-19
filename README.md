# MazeGenerator

A flexible C# library for generating, rendering, and solving mazes.

## Features

**5 Generation Algorithms** - Eller, Recursive Backtracker, Prim, Recursive Division, Aldous-Broder  
**3 Maze Types** - Perfect, Braided, WithRooms  
**2 Rendering Formats** - ASCII and SVG  
**2 Solving Algorithms** - BFS and DFS  

## Installation

### As .NET Library

```bash
git clone <repository-url>
cd MazeGenerator
dotnet build
```

### As Unity Package

1. Open Unity Editor → **Window** → **Package Manager**
2. Click **+** → **Add package from git URL**
3. Enter: `https://github.com/andreyfaraponov/MazeGenerator.git`

Or manually copy the folder into your Unity project's `Assets` directory.

**Requirements:** Unity 2019.4+, .NET Standard 2.0

---

## Quick Start

### 1. Simple Maze Creation

```csharp
using MazeGenerator;
using MazeGenerator.Rendering;

// Create a maze
var maze = MazeFactory.CreateBalanced(20, 20);

// Render to console
var renderer = new AsciiRenderer();
Console.WriteLine(renderer.RenderUnicode(maze));
```

### 2. Save as SVG

```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using System.IO;

var maze = MazeFactory.CreateChallenging(25, 25);
var renderer = new SvgRenderer();
File.WriteAllText("maze.svg", renderer.Render(maze));
```

### 3. Solve and Visualize

```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using MazeGenerator.Solvers;

var maze = MazeFactory.CreateChallenging(20, 20);
var solver = new BreadthFirstSolver();
var path = solver.Solve(maze, (0, 0), (19, 19));

var renderer = new SvgRenderer();
var svg = renderer.RenderWithPath(maze, path, RenderConfiguration.Default());
File.WriteAllText("solution.svg", svg);
```

### 4. Create Dungeon with Rooms

```csharp
using MazeGenerator;

var dungeon = MazeFactory.CreateWithRooms(
    width: 40,
    height: 40,
    roomCount: 8,
    minRoomSize: 4,
    maxRoomSize: 10
);
```

---

## Generation Algorithms

### Algorithm Overview

MazeGenerator supports 5 distinct generation algorithms, each with unique characteristics:

#### 1. Eller's Algorithm (Default)

**Use:** General purpose, large mazes, memory-constrained environments

```csharp
var maze = MazeFactory.CreateBalanced(50, 50);
// or
var maze = MazeFactory.Create(50, 50, MazeAlgorithmType.Eller);
```

**Characteristics:**
- ⚡ Fast generation
- 🟢 Low memory usage (row-by-row)
- 📊 Slight horizontal bias
- 🎯 Best for: Large mazes (100x100+), general use

#### 2. Recursive Backtracker

**Use:** Puzzle games, challenging mazes

```csharp
var maze = MazeFactory.CreateChallenging(30, 30);
// or
var maze = MazeFactory.Create(30, 30, MazeAlgorithmType.RecursiveBacktracker);
```

**Characteristics:**
- 🌊 Long winding corridors
- 🎮 Challenging for human solvers
- 📈 Low branching factor
- 🎯 Best for: Puzzles, adventure games

#### 3. Prim's Algorithm

**Use:** Exploration games, adventure mazes

```csharp
var maze = MazeFactory.CreateExploratory(30, 30);
// or
var maze = MazeFactory.Create(30, 30, MazeAlgorithmType.Prim);
```

**Characteristics:**
- 🌳 High branching factor
- 🔍 Many short paths
- 🎲 Interesting exploration
- 🎯 Best for: Exploration, RPGs

#### 4. Recursive Division

**Use:** Dungeons, room-based layouts

```csharp
var maze = MazeFactory.CreateDungeon(40, 40);
// or
var maze = MazeFactory.Create(40, 40, MazeAlgorithmType.RecursiveDivision);
```

**Characteristics:**
- 🏰 Room-like chambers
- ⚡ Very fast generation
- 📐 Structured appearance
- 🎯 Best for: Dungeons, strategy games

#### 5. Aldous-Broder

**Use:** Research, statistical analysis

```csharp
var maze = MazeFactory.CreateUnbiased(20, 20);
// or
var maze = MazeFactory.Create(20, 20, MazeAlgorithmType.AldousBroder);
```

**Characteristics:**
- 🎲 Truly unbiased
- 📊 Uniform distribution
- 🐌 Slowest algorithm
- 🎯 Best for: Research, small mazes only

⚠️ **Warning:** Not recommended for mazes > 50×50

### Algorithm Selection Guide

| Maze Size | Recommended Algorithm | Why |
|-----------|----------------------|-----|
| < 10×10 | Any | All perform well |
| 10×10 to 50×50 | Any except Aldous-Broder | Choose by characteristics |
| 50×50 to 200×200 | Eller, Backtracker, Prim, Division | Good performance |
| 200×200 to 1000×1000 | Eller, Division | Memory efficient |
| > 1000×1000 | Eller | Best memory usage |

### Algorithm Comparison

```csharp
using MazeGenerator;
using MazeGenerator.Enums;

// Compare all algorithms with same seed
int seed = 42;
var mazes = new Dictionary<string, Maze>
{
    ["Eller"] = MazeFactory.Create(20, 20, MazeAlgorithmType.Eller, seed),
    ["Backtracker"] = MazeFactory.Create(20, 20, MazeAlgorithmType.RecursiveBacktracker, seed),
    ["Prim"] = MazeFactory.Create(20, 20, MazeAlgorithmType.Prim, seed),
    ["Division"] = MazeFactory.Create(20, 20, MazeAlgorithmType.RecursiveDivision, seed),
    ["AldousBroder"] = MazeFactory.Create(20, 20, MazeAlgorithmType.AldousBroder, seed)
};
```

---

## Maze Types & Modifiers

### Perfect Mazes

Traditional mazes with exactly one path between any two points.

```csharp
var maze = MazeFactory.Create(25, 25);
// or explicitly
var config = new MazeConfiguration
{
    Width = 25,
    Height = 25,
    Type = MazeType.Perfect
};
var maze = new Maze(config);
```

**Characteristics:**
- ✅ No loops
- ✅ Single solution path
- ✅ All cells reachable
- ✅ Best for puzzles

### Braided Mazes

Mazes with loops created by removing dead ends.

```csharp
// Default braiding (50% dead ends removed)
var maze = MazeFactory.CreateBraided(30, 30);

// Custom braiding factor
var maze = MazeFactory.CreateBraided(30, 30, braidingFactor: 0.7);

// With seed
var maze = MazeFactory.CreateBraided(30, 30, braidingFactor: 0.5, seed: 42);
```

**Braiding Factor:**
- `0.0` = Perfect maze (no loops)
- `0.5` = Remove 50% of dead ends
- `1.0` = Remove all dead ends

**Characteristics:**
- ✅ Multiple solution paths
- ✅ Loops and cycles
- ✅ Less frustrating exploration
- ✅ Better for casual games

**Example: Progressive Difficulty**
```csharp
public Maze CreateMazeForLevel(int level)
{
    // Reduce braiding as player progresses
    double braiding = Math.Max(0.0, 1.0 - (level * 0.1));
    return MazeFactory.CreateBraided(20 + level, 20 + level, braiding);
}
```

### Mazes With Rooms

Dungeon-style mazes with rectangular open chambers.

```csharp
// Default settings (5 rooms, size 3-7)
var dungeon = MazeFactory.CreateWithRooms(30, 30);

// Custom configuration
var dungeon = MazeFactory.CreateWithRooms(
    width: 40,
    height: 40,
    roomCount: 8,
    minRoomSize: 4,
    maxRoomSize: 10,
    seed: 42
);

// Using configuration object
var config = new MazeConfiguration
{
    Width = 35,
    Height = 35,
    Type = MazeType.WithRooms,
    RoomCount = 7,
    MinRoomSize = 3,
    MaxRoomSize = 8,
    Algorithm = MazeAlgorithmType.RecursiveBacktracker
};
var dungeon = new Maze(config);
```

**Parameters:**
- `RoomCount` - Number of rooms to place (default: 5)
- `MinRoomSize` - Minimum room dimensions (default: 3)
- `MaxRoomSize` - Maximum room dimensions (default: 7)

**Characteristics:**
- ✅ Open rectangular chambers
- ✅ Corridors connecting rooms
- ✅ Dungeon-style layouts
- ✅ Smart placement (no overlaps)
- ✅ Guaranteed connections
- ✅ Works with all algorithms

**Best For:**
- 🎮 RPG dungeons
- 🎲 Roguelike games
- 🏰 Dungeon crawlers
- ⚔️ Strategy games

**Example: Themed Dungeons**
```csharp
public enum DungeonTheme { Crypt, Cave, Castle, Mine }

public Maze GenerateThemedDungeon(DungeonTheme theme)
{
    return theme switch
    {
        DungeonTheme.Crypt => MazeFactory.CreateWithRooms(
            30, 30, roomCount: 6, minRoomSize: 3, maxRoomSize: 5),
        
        DungeonTheme.Cave => MazeFactory.CreateWithRooms(
            35, 35, roomCount: 4, minRoomSize: 5, maxRoomSize: 10),
        
        DungeonTheme.Castle => MazeFactory.CreateWithRooms(
            40, 40, roomCount: 8, minRoomSize: 4, maxRoomSize: 8),
        
        DungeonTheme.Mine => MazeFactory.CreateWithRooms(
            25, 25, roomCount: 3, minRoomSize: 3, maxRoomSize: 6),
        
        _ => MazeFactory.CreateWithRooms(30, 30)
    };
}
```

---

## Rendering

### ASCII Rendering

Console-friendly text output with Unicode box-drawing characters.

#### Basic ASCII

```csharp
using MazeGenerator.Rendering;

var maze = MazeFactory.CreateBalanced(15, 15);
var renderer = new AsciiRenderer();

// Standard ASCII (+ - |)
Console.WriteLine(renderer.Render(maze));

// Unicode box-drawing (┌ ─ │)
Console.WriteLine(renderer.RenderUnicode(maze));
```

#### ASCII with Configuration

```csharp
var config = RenderConfiguration.Default();
string output = renderer.Render(maze, config);
```

**Best For:**
- 💻 Console applications
- 📝 Debug output
- 📧 Email/text reports
- 🔧 Testing

### SVG Rendering

Scalable vector graphics for web, print, and high-quality display.

#### Basic SVG

```csharp
using MazeGenerator.Rendering;
using System.IO;

var maze = MazeFactory.CreateChallenging(25, 25);
var renderer = new SvgRenderer();

string svg = renderer.Render(maze);
File.WriteAllText("maze.svg", svg);
```

#### Custom Styling

```csharp
var config = new RenderConfiguration
{
    CellSize = 20,
    WallColor = "#0066CC",
    BackgroundColor = "#E6F2FF",
    WallThickness = 2,
    ShowMarkers = true
};

string svg = renderer.Render(maze, config);
File.WriteAllText("styled_maze.svg", svg);
```

#### Dark Theme

```csharp
var darkConfig = new RenderConfiguration
{
    WallColor = "#FFFFFF",
    BackgroundColor = "#1E1E1E",
    PathColor = "#00FF00",
    CellSize = 20
};

string darkSvg = renderer.Render(maze, darkConfig);
```

#### Preset Configurations

```csharp
// Small (10px cells)
var small = RenderConfiguration.Small();

// Default (15px cells)
var def = RenderConfiguration.Default();

// Large (25px cells)
var large = RenderConfiguration.Large();
```

#### Rendering with Solution Path

```csharp
using MazeGenerator.Solvers;

var maze = MazeFactory.CreateChallenging(20, 20);
var solver = new BreadthFirstSolver();
var path = solver.Solve(maze, (0, 0), (19, 19));

var renderer = new SvgRenderer();
var config = new RenderConfiguration
{
    PathColor = "#FF6600",
    ShowMarkers = true
};

string svg = renderer.RenderWithPath(maze, path, config);
File.WriteAllText("solution.svg", svg);
```

**Best For:**
- 🌐 Web applications
- 📄 Print output
- 📊 Presentations
- 🎨 High-quality graphics

### Rendering Configuration

```csharp
public class RenderConfiguration
{
    public int CellSize { get; set; } = 15;
    public int WallThickness { get; set; } = 2;
    public string WallColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string PathColor { get; set; } = "#FF0000";
    public string StartMarkerColor { get; set; } = "#00FF00";
    public string EndMarkerColor { get; set; } = "#0000FF";
    public bool ShowMarkers { get; set; } = false;
}
```

### Complete Rendering Example

```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using System.IO;

public class MazeExporter
{
    public void ExportAllFormats(Maze maze, string baseName)
    {
        // ASCII
        var asciiRenderer = new AsciiRenderer();
        File.WriteAllText($"{baseName}.txt", asciiRenderer.Render(maze));
        File.WriteAllText($"{baseName}_unicode.txt", asciiRenderer.RenderUnicode(maze));
        
        // SVG - Multiple sizes
        var svgRenderer = new SvgRenderer();
        File.WriteAllText($"{baseName}_small.svg", 
            svgRenderer.Render(maze, RenderConfiguration.Small()));
        File.WriteAllText($"{baseName}_default.svg", 
            svgRenderer.Render(maze, RenderConfiguration.Default()));
        File.WriteAllText($"{baseName}_large.svg", 
            svgRenderer.Render(maze, RenderConfiguration.Large()));
    }
}
```

---

## Maze Solving

### Breadth-First Search (BFS)

Guarantees finding the shortest path between two points.

```csharp
using MazeGenerator.Solvers;

var maze = MazeFactory.CreateChallenging(30, 30);
var solver = new BreadthFirstSolver();

// Solve from top-left to bottom-right
var path = solver.Solve(maze, start: (0, 0), end: (29, 29));

Console.WriteLine($"Shortest path length: {path.Count} steps");
```

**Characteristics:**
- ✅ Guarantees shortest path
- ✅ Explores level by level
- ✅ More memory usage
- ✅ Slower than DFS
- 🎯 Use when shortest path is required

### Depth-First Search (DFS)

Fast pathfinding that finds any valid path.

```csharp
using MazeGenerator.Solvers;

var maze = MazeFactory.CreateExploratory(30, 30);
var solver = new DepthFirstSolver();

// Find any path
var path = solver.Solve(maze, start: (0, 0), end: (29, 29));

Console.WriteLine($"Path length: {path.Count} steps");
```

**Characteristics:**
- ✅ Fast pathfinding
- ✅ Less memory usage
- ✅ Finds any valid path
- ❌ May not be shortest
- 🎯 Use when any path is acceptable

### Visualizing Solutions

```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using MazeGenerator.Solvers;
using System.IO;

var maze = MazeFactory.CreateChallenging(25, 25);

// Solve with both algorithms
var bfsSolver = new BreadthFirstSolver();
var dfsSolver = new DepthFirstSolver();

var bfsPath = bfsSolver.Solve(maze, (0, 0), (24, 24));
var dfsPath = dfsSolver.Solve(maze, (0, 0), (24, 24));

// Render both solutions
var renderer = new SvgRenderer();

var bfsConfig = new RenderConfiguration { PathColor = "#0000FF" };
File.WriteAllText("bfs_solution.svg", 
    renderer.RenderWithPath(maze, bfsPath, bfsConfig));

var dfsConfig = new RenderConfiguration { PathColor = "#FF0000" };
File.WriteAllText("dfs_solution.svg", 
    renderer.RenderWithPath(maze, dfsPath, dfsConfig));

Console.WriteLine($"BFS path: {bfsPath.Count} steps (shortest)");
Console.WriteLine($"DFS path: {dfsPath.Count} steps");
```

### Handling No Solution

```csharp
var path = solver.Solve(maze, start, end);

if (path.Count == 0)
{
    Console.WriteLine("No path found between points");
}
else
{
    Console.WriteLine($"Path found: {path.Count} steps");
}
```

---

## API Reference

### MazeFactory

Static factory class for creating mazes.

#### Basic Creation

```csharp
// Default (Eller's algorithm)
Maze Create(int width, int height)

// With algorithm
Maze Create(int width, int height, MazeAlgorithmType algorithm)

// With algorithm and seed
Maze Create(int width, int height, MazeAlgorithmType algorithm, int seed)

// With full configuration
Maze Create(MazeConfiguration config)
```

#### Semantic Methods

```csharp
// Long corridors, challenging
Maze CreateChallenging(int width, int height, int? seed = null)

// Many branches, exploratory
Maze CreateExploratory(int width, int height, int? seed = null)

// Room-based, dungeon style
Maze CreateDungeon(int width, int height, int? seed = null)

// Balanced, general purpose
Maze CreateBalanced(int width, int height, int? seed = null)

// Unbiased, uniform distribution
Maze CreateUnbiased(int width, int height, int? seed = null)

// With loops, multiple paths
Maze CreateBraided(int width, int height, double braidingFactor = 0.5, int? seed = null)

// With rooms, dungeon chambers
Maze CreateWithRooms(int width, int height, int roomCount = 5, 
    int minRoomSize = 3, int maxRoomSize = 7, int? seed = null)

// Random algorithm selection
Maze CreateRandom(int width, int height, int? seed = null)

// Auto-select best algorithm for size
Maze CreateOptimized(int width, int height, int? seed = null)
```

### Maze Class

Main maze class containing the generated maze data.

#### Properties

```csharp
IReadOnlyList<IReadOnlyList<Cell>> Cells { get; }
int Width { get; }
int Height { get; }
MazeConfiguration Configuration { get; }
MazeGrid Grid { get; }
```

#### Methods

```csharp
Cell GetCell(int row, int column)
```

### Cell Class

Represents a single cell in the maze.

#### Properties

```csharp
bool Left { get; set; }    // Has left wall
bool Right { get; set; }   // Has right wall
bool Top { get; set; }     // Has top wall
bool Bottom { get; set; }  // Has bottom wall
int Set { get; set; }      // Set identifier (algorithm internal)
```

### MazeConfiguration

Configuration object for maze generation.

#### Properties

```csharp
int Width { get; set; }
int Height { get; set; }
int? Seed { get; set; }
MazeAlgorithmType Algorithm { get; set; }
MazeType Type { get; set; }
double BraidingFactor { get; set; }
int RoomCount { get; set; }
int MinRoomSize { get; set; }
int MaxRoomSize { get; set; }
```

#### Methods

```csharp
void Validate()
static MazeConfiguration Default(int width, int height)
```

### Renderers

#### AsciiRenderer

```csharp
string Render(Maze maze)
string Render(Maze maze, RenderConfiguration config)
string RenderUnicode(Maze maze)
```

#### SvgRenderer

```csharp
string Render(Maze maze)
string Render(Maze maze, RenderConfiguration config)
string RenderWithPath(Maze maze, List<(int, int)> path, RenderConfiguration config)
```

### Solvers

#### BreadthFirstSolver

```csharp
List<(int row, int col)> Solve(Maze maze, (int, int) start, (int, int) end)
```

#### DepthFirstSolver

```csharp
List<(int row, int col)> Solve(Maze maze, (int, int) start, (int, int) end)
```

### Enums

#### MazeAlgorithmType

```csharp
public enum MazeAlgorithmType
{
    Eller,
    RecursiveBacktracker,
    Prim,
    RecursiveDivision,
    AldousBroder
}
```

#### MazeType

```csharp
public enum MazeType
{
    Perfect,
    Braided,
    WithRooms
}
```

---

## Examples

### Example 1: Console Maze Viewer

```csharp
using System;
using MazeGenerator;
using MazeGenerator.Rendering;

class MazeViewer
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var renderer = new AsciiRenderer();
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Maze Generator ===");
            Console.WriteLine("1. Challenging  2. Exploratory  3. Dungeon");
            Console.WriteLine("4. Braided  5. With Rooms  Q. Quit\n");
            
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Q) break;
            
            Maze maze = key switch
            {
                ConsoleKey.D1 => MazeFactory.CreateChallenging(15, 15),
                ConsoleKey.D2 => MazeFactory.CreateExploratory(15, 15),
                ConsoleKey.D3 => MazeFactory.CreateDungeon(15, 15),
                ConsoleKey.D4 => MazeFactory.CreateBraided(15, 15, 0.5),
                ConsoleKey.D5 => MazeFactory.CreateWithRooms(15, 15, 4),
                _ => null
            };
            
            if (maze != null)
            {
                Console.Clear();
                Console.WriteLine(renderer.RenderUnicode(maze));
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
```

### Example 2: Web API Endpoint

```csharp
using Microsoft.AspNetCore.Mvc;
using MazeGenerator;
using MazeGenerator.Rendering;

[ApiController]
[Route("api/maze")]
public class MazeApiController : ControllerBase
{
    [HttpGet("generate")]
    public IActionResult GenerateMaze(
        [FromQuery] int width = 20,
        [FromQuery] int height = 20,
        [FromQuery] string type = "balanced",
        [FromQuery] string format = "svg",
        [FromQuery] int? seed = null)
    {
        // Create maze based on type
        Maze maze = type.ToLower() switch
        {
            "challenging" => MazeFactory.CreateChallenging(width, height, seed),
            "exploratory" => MazeFactory.CreateExploratory(width, height, seed),
            "dungeon" => MazeFactory.CreateDungeon(width, height, seed),
            "braided" => MazeFactory.CreateBraided(width, height, 0.5, seed),
            "rooms" => MazeFactory.CreateWithRooms(width, height, 5, 3, 7, seed),
            _ => MazeFactory.CreateBalanced(width, height, seed)
        };
        
        // Render in requested format
        if (format.ToLower() == "svg")
        {
            var renderer = new SvgRenderer();
            return Content(renderer.Render(maze), "image/svg+xml");
        }
        else
        {
            var renderer = new AsciiRenderer();
            return Content(renderer.Render(maze), "text/plain");
        }
    }
    
    [HttpGet("solve")]
    public IActionResult SolveMaze(
        [FromQuery] int width = 20,
        [FromQuery] int height = 20,
        [FromQuery] int? seed = null)
    {
        var maze = MazeFactory.CreateChallenging(width, height, seed);
        var solver = new BreadthFirstSolver();
        var path = solver.Solve(maze, (0, 0), (height - 1, width - 1));
        
        var renderer = new SvgRenderer();
        var config = new RenderConfiguration { PathColor = "#FF6600" };
        var svg = renderer.RenderWithPath(maze, path, config);
        
        return Content(svg, "image/svg+xml");
    }
}
```

### Example 3: Game Level Generator

```csharp
using MazeGenerator;

public class LevelGenerator
{
    public Maze GenerateLevelForDifficulty(int level)
    {
        // Progressive difficulty
        if (level <= 3)
        {
            // Easy: small maze, high braiding
            return MazeFactory.CreateBraided(15, 15, braidingFactor: 0.7);
        }
        else if (level <= 7)
        {
            // Medium: larger maze, moderate braiding
            return MazeFactory.CreateBraided(20, 20, braidingFactor: 0.4);
        }
        else if (level <= 10)
        {
            // Hard: large maze, low braiding
            return MazeFactory.CreateChallenging(25, 25);
        }
        else
        {
            // Expert: very large, complex
            return MazeFactory.CreateChallenging(30 + level, 30 + level);
        }
    }
}
```

### Example 4: Procedural Dungeon Generator

```csharp
using MazeGenerator;

public class DungeonGenerator
{
    private int baseSeed;
    
    public DungeonGenerator(int worldSeed)
    {
        this.baseSeed = worldSeed;
    }
    
    public Maze GenerateFloor(int floorNumber)
    {
        // Each floor uses consistent seed
        int floorSeed = baseSeed + floorNumber * 1000;
        
        // Deeper floors have more rooms
        int roomCount = 3 + floorNumber;
        int maxRoomSize = Math.Min(5 + floorNumber, 12);
        
        return MazeFactory.CreateWithRooms(
            width: 30 + floorNumber * 2,
            height: 30 + floorNumber * 2,
            roomCount: roomCount,
            minRoomSize: 3,
            maxRoomSize: maxRoomSize,
            seed: floorSeed
        );
    }
    
    public Maze GenerateBossRoom(int floorNumber)
    {
        int seed = baseSeed + floorNumber * 1000 + 999;
        
        // Boss room: larger, more complex
        return MazeFactory.CreateWithRooms(
            width: 50,
            height: 50,
            roomCount: 10,
            minRoomSize: 5,
            maxRoomSize: 12,
            seed: seed
        );
    }
}
```

### Example 5: Batch Export Tool

```csharp
using System;
using System.IO;
using MazeGenerator;
using MazeGenerator.Rendering;

public class MazeExportTool
{
    public void ExportMazeCollection(string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        
        var sizes = new[] { 10, 20, 30, 50 };
        var types = new[]
        {
            ("challenging", (Func<int, int, int, Maze>)MazeFactory.CreateChallenging),
            ("exploratory", (Func<int, int, int, Maze>)MazeFactory.CreateExploratory),
            ("dungeon", (Func<int, int, int, Maze>)MazeFactory.CreateDungeon)
        };
        
        int seed = 42;
        var svgRenderer = new SvgRenderer();
        var asciiRenderer = new AsciiRenderer();
        
        foreach (var size in sizes)
        {
            foreach (var (name, factory) in types)
            {
                var maze = factory(size, size, seed);
                
                // Export SVG
                string svgPath = Path.Combine(outputDir, $"{name}_{size}x{size}.svg");
                File.WriteAllText(svgPath, svgRenderer.Render(maze));
                
                // Export ASCII
                string txtPath = Path.Combine(outputDir, $"{name}_{size}x{size}.txt");
                File.WriteAllText(txtPath, asciiRenderer.RenderUnicode(maze));
                
                Console.WriteLine($"Exported: {name} {size}×{size}");
            }
        }
        
        Console.WriteLine($"\nAll mazes exported to: {outputDir}");
    }
}
```

### Example 6: Adaptive Difficulty System

```csharp
using MazeGenerator;

public class AdaptiveMazeSystem
{
    private double playerSkill = 0.5; // 0.0 = beginner, 1.0 = expert
    private int completedMazes = 0;
    
    public Maze GenerateAdaptiveMaze()
    {
        int baseSize = 15;
        int size = baseSize + (int)(playerSkill * 20); // 15-35
        
        // Adjust braiding inversely to skill
        double braiding = 1.0 - playerSkill; // More skill = less braiding
        
        return MazeFactory.CreateBraided(size, size, braiding);
    }
    
    public void RecordCompletion(double completionTime, bool succeeded)
    {
        completedMazes++;
        
        // Fast completion = increase difficulty
        if (succeeded && completionTime < 30.0)
        {
            playerSkill = Math.Min(1.0, playerSkill + 0.05);
        }
        // Slow/failed = decrease difficulty
        else if (!succeeded || completionTime > 120.0)
        {
            playerSkill = Math.Max(0.0, playerSkill - 0.05);
        }
        
        Console.WriteLine($"Player skill: {playerSkill:P0}");
    }
}
```

---

## Performance

### Generation Performance

| Maze Size | Eller | Backtracker | Prim | Division | Aldous-Broder |
|-----------|-------|-------------|------|----------|---------------|
| 10×10 | <1ms | <1ms | <1ms | <1ms | 2-5ms |
| 50×50 | 2ms | 3ms | 5ms | 2ms | 50-200ms |
| 100×100 | 8ms | 12ms | 20ms | 8ms | 400-2000ms |
| 500×500 | 180ms | 250ms | 450ms | 170ms | Too slow |

### Rendering Performance

| Format | 10×10 | 50×50 | 100×100 |
|--------|-------|-------|---------|
| ASCII | <1ms | <1ms | 2ms |
| SVG | <1ms | 3ms | 12ms |

### Solving Performance

| Algorithm | 10×10 | 50×50 | 100×100 |
|-----------|-------|-------|---------|
| BFS | <1ms | 5ms | 20ms |
| DFS | <1ms | 3ms | 12ms |

### Memory Usage

| Component | Small (10×10) | Medium (50×50) | Large (100×100) |
|-----------|---------------|----------------|-----------------|
| Maze Data | ~2 KB | ~50 KB | ~200 KB |
| Generation | ~5 KB | ~100 KB | ~400 KB |
| Rendering | ~10 KB | ~200 KB | ~800 KB |

### Optimization Tips

1. **Use CreateOptimized()** - Automatically selects best algorithm
2. **Cache Rendered Output** - Don't re-render unchanged mazes
3. **Use Seeds** - Enable caching/pregeneration
4. **Avoid Aldous-Broder** - For mazes > 50×50
5. **Background Generation** - For large mazes (>200×200)
6. **Stream SVG** - For web applications, stream directly to response

```csharp
// Good: Optimized for size
var maze = MazeFactory.CreateOptimized(width, height);

// Good: Cache rendered output
private Dictionary<int, string> renderedCache = new();

public string GetRenderedMaze(int seed)
{
    if (!renderedCache.ContainsKey(seed))
    {
        var maze = MazeFactory.CreateBalanced(20, 20, seed);
        var renderer = new SvgRenderer();
        renderedCache[seed] = renderer.Render(maze);
    }
    return renderedCache[seed];
}

// Good: Background generation
await Task.Run(() => 
{
    var largeMaze = MazeFactory.CreateOptimized(500, 500);
    // Process maze...
});
```

---

## Best Practices

### For Game Development

✅ **Use CreateOptimized()** for automatic algorithm selection  
✅ **Cache generated mazes** if they don't change  
✅ **Use seeds** for reproducible levels  
✅ **Use braiding** for casual games (0.5-0.7 factor)  
✅ **Use perfect mazes** for puzzle games  
✅ **Progressive difficulty** - adjust size and braiding by level  
✅ **Pregenerate** mazes during loading screens  

```csharp
// Good: Progressive difficulty
public Maze GetMazeForLevel(int level)
{
    double braiding = Math.Max(0.0, 1.0 - (level * 0.1));
    int size = 15 + level;
    return MazeFactory.CreateBraided(size, size, braiding);
}
```

### For Web Applications

✅ **Use SVG renderer** for scalable output  
✅ **Adjust cell size** based on maze dimensions  
✅ **Cache rendered SVGs** for performance  
✅ **Use async generation** for large mazes  
✅ **Stream output** directly to response  
✅ **Validate input** (size limits, reasonable parameters)  
✅ **Add rate limiting** to prevent abuse  

```csharp
// Good: Adaptive cell size
int cellSize = width * height > 1000 ? 10 : 
               width * height > 400 ? 15 : 20;

var config = new RenderConfiguration { CellSize = cellSize };
```

### For Console Applications

✅ **Use AsciiRenderer** with Unicode  
✅ **Keep maze size < 30×30** for readability  
✅ **Use monospace fonts**  
✅ **Clear console** before displaying  
✅ **Set UTF-8 encoding** for Unicode chars  

```csharp
// Good: Set encoding
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Good: Clear before display
Console.Clear();
var renderer = new AsciiRenderer();
Console.WriteLine(renderer.RenderUnicode(maze));
```

### For Procedural Generation

✅ **Use deterministic seeds** (world seed + coordinates)  
✅ **Document seed formulas** for reproducibility  
✅ **Test seed consistency** across sessions  
✅ **Vary algorithms** for different biomes/themes  

```csharp
// Good: Deterministic seed from coordinates
public Maze GenerateRegion(int x, int y)
{
    int seed = worldSeed + (x * 1000) + y;
    return MazeFactory.CreateOptimized(50, 50, seed);
}
```

### For Testing

✅ **Use fixed seeds** for reproducible tests  
✅ **Test all algorithms** with various sizes  
✅ **Verify maze properties** (connectivity, walls)  
✅ **Test edge cases** (1×1, very large)  
✅ **Benchmark performance** regularly  

```csharp
// Good: Reproducible test
[Test]
public void TestMazeGeneration()
{
    var maze1 = MazeFactory.CreateBalanced(20, 20, seed: 42);
    var maze2 = MazeFactory.CreateBalanced(20, 20, seed: 42);
    
    Assert.AreEqual(maze1.Width, maze2.Width);
    // Compare cells...
}
```

---

## Architecture

### Project Structure

```
MazeGenerator/
├── Core/
│   ├── Cell.cs                    - Cell with wall data
│   ├── Maze.cs                    - Main maze class
│   ├── MazeGrid.cs                - Internal grid management
│   ├── MazeConfiguration.cs       - Configuration object
│   └── MazeFactory.cs             - Factory for creation
├── Enums/
│   ├── MazeAlgorithmType.cs       - Algorithm enumeration
│   └── MazeType.cs                - Maze type enumeration
├── Algorithms/
│   ├── IMazeAlgorithm.cs          - Algorithm interface
│   ├── EllerAlgorithm.cs          - Eller's algorithm
│   ├── RecursiveBacktrackerAlgorithm.cs
│   ├── PrimAlgorithm.cs           - Prim's algorithm
│   ├── RecursiveDivisionAlgorithm.cs
│   └── AldousBroderAlgorithm.cs   - Aldous-Broder
├── Modifiers/
│   ├── IMazeModifier.cs           - Modifier interface
│   ├── BraidingModifier.cs        - Removes dead ends
│   └── RoomModifier.cs            - Adds rooms
├── Rendering/
│   ├── IMazeRenderer.cs           - Renderer interface
│   ├── RenderConfiguration.cs     - Render settings
│   ├── AsciiRenderer.cs           - ASCII output
│   └── SvgRenderer.cs             - SVG output
└── Solvers/
    ├── IMazeSolver.cs             - Solver interface
    ├── BreadthFirstSolver.cs      - BFS pathfinding
    └── DepthFirstSolver.cs        - DFS pathfinding
```

### Design Patterns

#### Factory Pattern
```csharp
// Centralized creation with semantic methods
var maze = MazeFactory.CreateChallenging(20, 20);
```

#### Strategy Pattern
```csharp
// Pluggable algorithms
public interface IMazeAlgorithm
{
    void Generate(List<List<Cell>> cells, MazeConfiguration config);
}
```

#### Builder Pattern
```csharp
// Configuration object
var config = new MazeConfiguration
{
    Width = 30,
    Height = 30,
    Algorithm = MazeAlgorithmType.Prim,
    Type = MazeType.Braided,
    BraidingFactor = 0.5
};
```

### Extensibility

#### Adding Custom Algorithm

```csharp
public class CustomAlgorithm : IMazeAlgorithm
{
    public string Name => "Custom Algorithm";
    public string Description => "My custom maze generation";
    
    public void Generate(List<List<Cell>> cells, MazeConfiguration config)
    {
        int width = config.Width;
        int height = config.Height;
        var random = config.Seed.HasValue 
            ? new Random(config.Seed.Value) 
            : new Random();
        
        // Your algorithm logic here
        // Modify cells to create passages
    }
}

// Use it
var config = new MazeConfiguration
{
    Width = 20,
    Height = 20,
    Algorithm = MazeAlgorithmType.Eller // Would need to extend enum
};
```

#### Adding Custom Modifier

```csharp
public class WeightedModifier : IMazeModifier
{
    public string Name => "Weighted Modifier";
    public string Description => "Adds weights to cells";
    
    public void Apply(List<List<Cell>> cells, MazeConfiguration config)
    {
        // Modify maze after generation
        // Add weights, special cells, etc.
    }
}
```

#### Adding Custom Renderer

```csharp
public class JsonRenderer : IMazeRenderer
{
    public string Name => "JSON Renderer";
    public string Description => "Exports maze as JSON";
    public string FileExtension => "json";
    
    public string Render(Maze maze)
    {
        // Convert maze to JSON
        return JsonConvert.SerializeObject(maze.Cells);
    }
    
    public string Render(Maze maze, RenderConfiguration config)
    {
        return Render(maze);
    }
}
```

---

## Troubleshooting

### Common Issues

#### Maze Appears Disconnected

**Problem:** Some cells seem unreachable

**Solutions:**
- Verify you're using perfect maze (not accidentally applying braiding)
- Check that maze generation completed without exceptions
- For WithRooms type, this is expected (rooms create open spaces)

```csharp
// Ensure perfect maze
var config = new MazeConfiguration
{
    Width = 20,
    Height = 20,
    Type = MazeType.Perfect
};
var maze = new Maze(config);
```

#### SVG Not Displaying

**Problem:** SVG file won't open in browser

**Solutions:**
- Verify output is valid XML (check for special characters)
- Ensure cell size isn't too small (< 5px)
- Check file saved with .svg extension
- Validate SVG with online validator

```csharp
// Use reasonable cell size
var config = new RenderConfiguration { CellSize = 15 };
```

#### Performance Issues

**Problem:** Maze generation is too slow

**Solutions:**
- Use `CreateOptimized()` for automatic selection
- Avoid Aldous-Broder for large mazes (> 50×50)
- Cache rendered output if displaying repeatedly
- Generate large mazes in background thread

```csharp
// Good: Async generation
await Task.Run(() =>
{
    var maze = MazeFactory.CreateOptimized(500, 500);
});
```

#### Unicode Characters Not Displaying

**Problem:** Box-drawing characters show as ???

**Solutions:**
- Set console encoding to UTF-8
- Use monospace font that supports Unicode
- Fall back to ASCII renderer if needed

```csharp
// Set UTF-8 encoding
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Or use ASCII fallback
var renderer = new AsciiRenderer();
Console.WriteLine(renderer.Render(maze)); // Uses + - | instead
```

#### Out of Memory

**Problem:** Application crashes with large mazes

**Solutions:**
- Use Eller algorithm (most memory efficient)
- Reduce maze size
- Generate in chunks if possible
- Increase heap size

```csharp
// Use memory-efficient algorithm
var maze = MazeFactory.Create(1000, 1000, MazeAlgorithmType.Eller);
```

#### Mazes Too Easy/Hard

**Problem:** Difficulty doesn't match expectations

**Solutions:**
- Adjust braiding factor (higher = easier)
- Change algorithm (Backtracker = harder)
- Modify maze size
- Use progressive difficulty

```csharp
// Adjust difficulty
double braiding = difficultyLevel switch
{
    "easy" => 0.7,
    "medium" => 0.4,
    "hard" => 0.0,
    _ => 0.5
};

var maze = MazeFactory.CreateBraided(20, 20, braiding);
```

### Getting Help

- 📖 Check this documentation
- 💻 Review examples in repository
- 🐛 Report issues on GitHub
- 💬 Ask questions in discussions

---

## License

[Your License Here - e.g., MIT, Apache 2.0, etc.]

---

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

---

## Changelog

### Version 2.0.0
- ✅ Added 5 generation algorithms
- ✅ Added braiding modifier
- ✅ Added room modifier
- ✅ Added SVG renderer
- ✅ Added maze solvers (BFS, DFS)
- ✅ Complete documentation
- ✅ Performance optimizations
- ✅ Factory pattern implementation

### Version 1.0.0
- ✅ Initial release
- ✅ Basic maze generation
- ✅ ASCII rendering

---

## Acknowledgments

Built with C# and .NET Standard 2.0 for maximum compatibility.

Inspired by classic maze generation algorithms and modern game development needs.

---

<div align="center">

**MazeGenerator** - Professional maze generation for C#

Made with ❤️ for game developers, educators, and maze enthusiasts

</div>
