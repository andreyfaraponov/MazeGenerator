# Rendering Examples - Complete Guide

## Basic Examples

### Example 1: Simple Console Output
```csharp
using MazeGenerator;
using MazeGenerator.Rendering;

var maze = MazeFactory.CreateBalanced(10, 10);
var renderer = new AsciiRenderer();
Console.WriteLine(renderer.Render(maze));
```

### Example 2: Save to File
```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using System.IO;

var maze = MazeFactory.CreateChallenging(20, 20);
var renderer = new SvgRenderer();
File.WriteAllText("maze.svg", renderer.Render(maze));
```

### Example 3: Multiple Formats
```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using System.IO;

var maze = MazeFactory.CreateDungeon(15, 15, seed: 42);

// ASCII
var asciiRenderer = new AsciiRenderer();
File.WriteAllText("maze.txt", asciiRenderer.Render(maze));

// Unicode
File.WriteAllText("maze_unicode.txt", asciiRenderer.RenderUnicode(maze));

// SVG
var svgRenderer = new SvgRenderer();
File.WriteAllText("maze.svg", svgRenderer.Render(maze));
```

---

## Advanced Examples

### Example 4: Custom Colors
```csharp
var maze = MazeFactory.CreateBraided(20, 20, 0.5);
var renderer = new SvgRenderer();
var config = new RenderConfiguration
{
    CellSize = 25,
    WallColor = "#0066CC",
    BackgroundColor = "#E6F2FF",
    ShowMarkers = true
};

string svg = renderer.Render(maze, config);
File.WriteAllText("blue_maze.svg", svg);
```

### Example 5: Dark Theme
```csharp
var maze = MazeFactory.CreateExploratory(18, 18);
var renderer = new SvgRenderer();
var config = new RenderConfiguration
{
    WallColor = "#FFFFFF",
    BackgroundColor = "#1E1E1E",
    PathColor = "#00FF00"
};

File.WriteAllText("dark_maze.svg", renderer.Render(maze, config));
```

### Example 6: Size Configurations
```csharp
var maze = MazeFactory.CreateBalanced(30, 30);
var renderer = new SvgRenderer();

// Small (for overview)
var small = RenderConfiguration.Small();
File.WriteAllText("maze_small.svg", renderer.Render(maze, small));

// Default
var def = RenderConfiguration.Default();
File.WriteAllText("maze_default.svg", renderer.Render(maze, def));

// Large (for detail)
var large = RenderConfiguration.Large();
File.WriteAllText("maze_large.svg", renderer.Render(maze, large));
```

---

## Interactive Examples

### Example 7: Console Maze Viewer
```csharp
using System;
using MazeGenerator;
using MazeGenerator.Rendering;

class Program
{
    static void Main()
    {
        var renderer = new AsciiRenderer();
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Press 1-5 for different mazes, Q to quit\n");
            
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Q) break;
            
            Maze maze = key switch
            {
                ConsoleKey.D1 => MazeFactory.CreateChallenging(12, 12),
                ConsoleKey.D2 => MazeFactory.CreateExploratory(12, 12),
                ConsoleKey.D3 => MazeFactory.CreateDungeon(12, 12),
                ConsoleKey.D4 => MazeFactory.CreateBraided(12, 12, 0.5),
                ConsoleKey.D5 => MazeFactory.CreateRandom(12, 12),
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

### Example 8: Batch Generator
```csharp
using System;
using System.IO;
using MazeGenerator;
using MazeGenerator.Rendering;

class BatchGenerator
{
    static void Main()
    {
        Directory.CreateDirectory("output");
        
        var sizes = new[] { 10, 20, 30, 50 };
        var renderers = new IMazeRenderer[]
        {
            new AsciiRenderer(),
            new SvgRenderer()
        };
        
        foreach (var size in sizes)
        {
            var maze = MazeFactory.CreateOptimized(size, size, seed: 42);
            
            foreach (var renderer in renderers)
            {
                string filename = $"output/maze_{size}x{size}.{renderer.FileExtension}";
                string content = renderer.Render(maze);
                File.WriteAllText(filename, content);
                Console.WriteLine($"Generated: {filename}");
            }
        }
        
        Console.WriteLine("\nAll mazes generated!");
    }
}
```

---

## Web Integration Examples

### Example 9: ASP.NET Core API
```csharp
using Microsoft.AspNetCore.Mvc;
using MazeGenerator;
using MazeGenerator.Rendering;

[ApiController]
[Route("api/maze")]
public class MazeApiController : ControllerBase
{
    [HttpGet("generate")]
    public IActionResult Generate(
        int width = 20,
        int height = 20,
        string format = "svg",
        int? seed = null)
    {
        var maze = seed.HasValue
            ? MazeFactory.CreateBalanced(width, height, seed.Value)
            : MazeFactory.CreateBalanced(width, height);
        
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
}
```

### Example 10: Dynamic Sizing
```csharp
[HttpGet("maze/adaptive")]
public IActionResult GetAdaptiveMaze(int width, int height)
{
    var maze = MazeFactory.CreateOptimized(width, height);
    var renderer = new SvgRenderer();
    
    // Adjust cell size based on maze dimensions
    int cellSize = width * height > 1000 ? 10 :
                   width * height > 400 ? 15 : 20;
    
    var config = new RenderConfiguration
    {
        CellSize = cellSize,
        WallThickness = Math.Max(1, cellSize / 10)
    };
    
    return Content(renderer.Render(maze, config), "image/svg+xml");
}
```

---

## Testing Examples

### Example 11: Verify All Algorithms
```csharp
using MazeGenerator;
using MazeGenerator.Rendering;
using MazeGenerator.Enums;
using System;
using System.IO;

class AlgorithmTest
{
    static void Main()
    {
        var algorithms = Enum.GetValues(typeof(MazeAlgorithmType));
        var renderer = new SvgRenderer();
        
        foreach (MazeAlgorithmType algorithm in algorithms)
        {
            try
            {
                var config = new MazeConfiguration
                {
                    Width = 20,
                    Height = 20,
                    Algorithm = algorithm,
                    Seed = 42
                };
                
                var maze = new Maze(config);
                string svg = renderer.Render(maze);
                File.WriteAllText($"test_{algorithm}.svg", svg);
                
                Console.WriteLine($"✓ {algorithm} - Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ {algorithm} - Failed: {ex.Message}");
            }
        }
    }
}
```

### Example 12: Performance Benchmark
```csharp
using System;
using System.Diagnostics;
using MazeGenerator;
using MazeGenerator.Rendering;

class PerformanceTest
{
    static void Main()
    {
        var sizes = new[] { 10, 20, 50, 100 };
        var renderers = new[] 
        { 
            (new AsciiRenderer(), "ASCII"),
            (new SvgRenderer(), "SVG")
        };
        
        foreach (var size in sizes)
        {
            var maze = MazeFactory.CreateBalanced(size, size);
            
            foreach (var (renderer, name) in renderers)
            {
                var sw = Stopwatch.StartNew();
                
                for (int i = 0; i < 100; i++)
                {
                    _ = renderer.Render(maze);
                }
                
                sw.Stop();
                double avgMs = sw.ElapsedMilliseconds / 100.0;
                
                Console.WriteLine($"{size}×{size} {name}: {avgMs:F2}ms average");
            }
        }
    }
}
```

---

## Expected Results

All examples should:
- ✅ Compile without errors
- ✅ Generate valid output
- ✅ ASCII output displays correctly in console
- ✅ SVG files open in browsers
- ✅ Files saved successfully
- ✅ Performance is acceptable
- ✅ No crashes or exceptions

---

## Tips

### Console Display
```csharp
// Set console to UTF-8 for Unicode rendering
Console.OutputEncoding = System.Text.Encoding.UTF8;
```

### File Paths
```csharp
// Create output directory if it doesn't exist
Directory.CreateDirectory("output");
string path = Path.Combine("output", "maze.svg");
```

### Error Handling
```csharp
try
{
    var svg = renderer.Render(maze);
    File.WriteAllText("maze.svg", svg);
}
catch (IOException ex)
{
    Console.WriteLine($"Failed to save file: {ex.Message}");
}
```

### Validation
```csharp
if (width < 3 || width > 100)
    throw new ArgumentException("Width must be between 3 and 100");
```
