# Algorithm Comparison Guide

## Quick Reference

### Algorithm Selection Matrix

| Use Case | Best Algorithm | Why |
|----------|---------------|-----|
| Challenging puzzle | Recursive Backtracker | Long solution paths, fewer branches |
| Exploration game | Prim | Many paths, high branching |
| Dungeon layout | Recursive Division | Room-like structure |
| Large maze (memory limited) | Eller | Low memory usage |
| Unbiased/research | Aldous-Broder | Uniform distribution |
| General purpose | Eller or Prim | Good balance |

### Performance Comparison

#### Generation Speed (10,000 cells)
1. **Recursive Division** - Fastest (~0.5x time)
2. **Eller** - Very Fast (~1x time, baseline)
3. **Recursive Backtracker** - Fast (~1.2x time)
4. **Prim** - Medium (~1.8x time)
5. **Aldous-Broder** - Slow (~5-20x time, varies)

#### Memory Usage
1. **Eller** - Lowest (row-by-row)
2. **Recursive Division** - Low (recursion stack only)
3. **Recursive Backtracker** - Medium (visited array + stack)
4. **Prim** - Medium (frontier list)
5. **Aldous-Broder** - Medium (visited array)

### Visual Characteristics

#### Corridor Length
- **Longest**: Recursive Backtracker
- **Long**: Eller
- **Medium**: Aldous-Broder, Recursive Division
- **Shortest**: Prim

#### Branching Factor
- **Highest**: Prim (many choices at intersections)
- **High**: Aldous-Broder
- **Medium**: Eller, Recursive Division
- **Lowest**: Recursive Backtracker

#### Pattern/Structure
- **Most Structured**: Recursive Division (obvious rooms)
- **Some Structure**: Eller (horizontal bias)
- **Least Structure**: Aldous-Broder (truly random)
- **Natural Flow**: Recursive Backtracker, Prim

### Code Examples

#### Compare All Algorithms
```csharp
using MazeGenerator;
using MazeGenerator.Enums;

// Use same seed for fair comparison
int seed = 42;
int size = 20;

var eller = MazeFactory.Create(size, size, MazeAlgorithmType.Eller, seed);
var backtracker = MazeFactory.Create(size, size, MazeAlgorithmType.RecursiveBacktracker, seed);
var prim = MazeFactory.Create(size, size, MazeAlgorithmType.Prim, seed);
var division = MazeFactory.Create(size, size, MazeAlgorithmType.RecursiveDivision, seed);
var aldous = MazeFactory.Create(size, size, MazeAlgorithmType.AldousBroder, seed);
```

#### Choose Algorithm Dynamically
```csharp
using MazeGenerator;
using MazeGenerator.Enums;

public MazeGrid CreateMazeForDifficulty(int size, string difficulty)
{
    MazeAlgorithmType algorithm = difficulty switch
    {
        "easy" => MazeAlgorithmType.Prim,              // Many short paths
        "medium" => MazeAlgorithmType.Eller,            // Balanced
        "hard" => MazeAlgorithmType.RecursiveBacktracker, // Long winding paths
        "dungeon" => MazeAlgorithmType.RecursiveDivision, // Room-based
        _ => MazeAlgorithmType.Eller
    };
    
    return MazeFactory.Create(size, size, algorithm);
}
```

#### Performance Testing
```csharp
using System.Diagnostics;
using MazeGenerator;
using MazeGenerator.Enums;

public void BenchmarkAlgorithms()
{
    int size = 100;
    int iterations = 10;
    
    foreach (MazeAlgorithmType algorithm in Enum.GetValues(typeof(MazeAlgorithmType)))
    {
        var sw = Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            var maze = MazeFactory.Create(size, size, algorithm, i);
        }
        
        sw.Stop();
        Console.WriteLine($"{algorithm}: {sw.ElapsedMilliseconds / iterations}ms average");
    }
}
```

### Detailed Algorithm Analysis

#### Eller's Algorithm
**Strengths:**
- Memory efficient (processes row by row)
- Consistent performance
- Good for rectangular mazes
- Excellent for very large mazes

**Weaknesses:**
- Slight horizontal bias
- Less "natural" appearance than some others

**Best For:**
- Large mazes (1000x1000+)
- Memory-constrained environments
- When performance matters

#### Recursive Backtracker
**Strengths:**
- Creates very long corridors
- Challenging for human solvers
- Natural, organic appearance
- Fast generation

**Weaknesses:**
- Can create very difficult mazes
- Lower branching factor
- Stack depth can be issue for huge mazes

**Best For:**
- Puzzle games
- When challenge is desired
- Classic maze feel

#### Prim's Algorithm
**Strengths:**
- High branching factor
- Many valid paths
- Interesting exploration
- Fairly unbiased

**Weaknesses:**
- Can feel "busy" or cluttered
- Moderate generation speed
- More memory for frontier list

**Best For:**
- Exploration games
- When multiple paths desired
- Interactive maze games

#### Recursive Division
**Strengths:**
- Creates distinct rooms/chambers
- Very fast generation
- Low memory usage
- Unique appearance

**Weaknesses:**
- Obvious structure/pattern
- Less like traditional maze
- Can feel artificial

**Best For:**
- Dungeon generators
- Room-based layouts
- Architectural floor plans
- Strategy games

#### Aldous-Broder
**Strengths:**
- Truly unbiased (uniform spanning tree)
- Statistical properties guaranteed
- No visible patterns

**Weaknesses:**
- Slowest algorithm (especially large mazes)
- Unpredictable completion time
- Not suitable for real-time generation

**Best For:**
- Research/academic purposes
- When unbiased generation critical
- Small to medium mazes only
- Pre-generation scenarios

### Recommendations by Grid Size

#### Tiny (< 10x10)
- **Any algorithm works well**
- Choose based on desired appearance
- Performance not a concern

#### Small (10x10 to 50x50)
- **Recommended**: Any except Aldous-Broder
- All perform acceptably
- Choose based on visual preference

#### Medium (50x50 to 200x200)
- **Recommended**: Eller, Recursive Backtracker, Prim, Recursive Division
- **Avoid**: Aldous-Broder (too slow)
- Performance starts to matter

#### Large (200x200 to 1000x1000)
- **Recommended**: Eller, Recursive Division
- **Acceptable**: Recursive Backtracker, Prim
- **Avoid**: Aldous-Broder
- Memory and speed critical

#### Very Large (> 1000x1000)
- **Recommended**: Eller (best memory usage)
- **Possible**: Recursive Division
- **Difficult**: Others (may run out of memory)

### Integration Notes

All algorithms:
- ✅ Support seeded random generation
- ✅ Generate perfect mazes (no loops)
- ✅ Ensure all cells reachable
- ✅ Work with MazeConfiguration
- ✅ Compatible with future modifiers
- ✅ Support any rectangular grid size
- ✅ Initialize outer walls correctly
- ✅ Thread-safe (no shared state)

### Future Enhancements

With Phase 2 complete, these are now possible:

1. **Braiding** - Remove dead ends from any algorithm
2. **Weighting** - Add costs to cells
3. **Rooms** - Insert larger open areas
4. **Hybrid** - Combine multiple algorithms
5. **Rendering** - Visualize differences
6. **Analysis** - Measure maze properties

### Testing Guidelines

For each algorithm, verify:
1. ✅ Generates perfect maze (no loops)
2. ✅ All cells accessible
3. ✅ Outer walls intact
4. ✅ No isolated regions
5. ✅ Reproducible with same seed
6. ✅ Works with various sizes
7. ✅ No crashes or exceptions

### Conclusion

Phase 2 provides **5 production-ready maze generation algorithms**, each with distinct characteristics. Users can select the best algorithm for their specific needs based on:

- Desired visual appearance
- Performance requirements
- Memory constraints  
- Gameplay considerations
- Structural preferences

The clean interface design means adding more algorithms in the future is straightforward.
