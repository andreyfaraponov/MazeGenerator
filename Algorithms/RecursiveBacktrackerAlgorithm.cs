using System;
using System.Collections.Generic;

namespace MazeGenerator.Algorithms
{
	/// <summary>
	/// Recursive Backtracker algorithm for maze generation.
	/// Creates long, winding passages using depth-first search with backtracking.
	/// </summary>
	public class RecursiveBacktrackerAlgorithm : IMazeAlgorithm
	{
		public string Name => "Recursive Backtracker";
		
		public string Description => "Creates long, winding passages using depth-first search. Tends to create mazes with long corridors and relatively few dead ends. Good for creating challenging puzzles.";
		
		private Random _random;
		private int _width;
		private int _height;
		private bool[,] _visited;
		
		public void Generate(List<List<Cell>> cells, MazeConfiguration config)
		{
			_width = config.Width;
			_height = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();
			_visited = new bool[_height, _width];
			
			// Initialize all cells with walls on all sides
			InitializeCells(cells);
			
			// Start from a random cell
			int startX = _random.Next(_width);
			int startY = _random.Next(_height);
			
			// Perform recursive backtracking
			CarvePassages(cells, startX, startY);
		}
		
		private void InitializeCells(List<List<Cell>> cells)
		{
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					var cell = cells[y][x];
					cell.Left = true;
					cell.Right = true;
					cell.Top = true;
					cell.Bottom = true;
					cell.Set = 0;
				}
			}
		}
		
		private void CarvePassages(List<List<Cell>> cells, int x, int y)
		{
			_visited[y, x] = true;
			
			// Get all unvisited neighbors in random order
			var directions = GetShuffledDirections();
			
			foreach (var dir in directions)
			{
				int nx = x + dir.dx;
				int ny = y + dir.dy;
				
				// Check if neighbor is valid and unvisited
				if (IsValid(nx, ny) && !_visited[ny, nx])
				{
					// Remove wall between current cell and neighbor
					RemoveWall(cells, x, y, nx, ny, dir);
					
					// Recursively visit neighbor
					CarvePassages(cells, nx, ny);
				}
			}
		}
		
		private bool IsValid(int x, int y)
		{
			return x >= 0 && x < _width && y >= 0 && y < _height;
		}
		
		private void RemoveWall(List<List<Cell>> cells, int x1, int y1, int x2, int y2, Direction dir)
		{
			var cell1 = cells[y1][x1];
			var cell2 = cells[y2][x2];
			
			switch (dir.name)
			{
				case "North":
					cell1.Top = false;
					cell2.Bottom = false;
					break;
				case "South":
					cell1.Bottom = false;
					cell2.Top = false;
					break;
				case "East":
					cell1.Right = false;
					cell2.Left = false;
					break;
				case "West":
					cell1.Left = false;
					cell2.Right = false;
					break;
			}
		}
		
		private List<Direction> GetShuffledDirections()
		{
			var directions = new List<Direction>
			{
				new Direction("North", 0, -1),
				new Direction("South", 0, 1),
				new Direction("East", 1, 0),
				new Direction("West", -1, 0)
			};
			
			// Fisher-Yates shuffle
			for (int i = directions.Count - 1; i > 0; i--)
			{
				int j = _random.Next(i + 1);
				(directions[i], directions[j]) = (directions[j], directions[i]);
			}
			
			return directions;
		}
		
		private struct Direction
		{
			public string name;
			public int dx;
			public int dy;
			
			public Direction(string name, int dx, int dy)
			{
				this.name = name;
				this.dx = dx;
				this.dy = dy;
			}
		}
	}
}
