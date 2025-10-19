using System;
using System.Collections.Generic;

namespace MazeGenerator.Algorithms
{
	/// <summary>
	/// Aldous-Broder algorithm for maze generation.
	/// Creates uniform spanning trees using a random walk approach.
	/// </summary>
	public class AldousBroderAlgorithm : IMazeAlgorithm
	{
		public string Name => "Aldous-Broder Algorithm";
		
		public string Description => "Creates uniform spanning trees using random walk. Slower than other algorithms but produces truly unbiased mazes. Each possible maze configuration has equal probability.";
		
		private Random _random;
		private int _width;
		private int _height;
		private bool[,] _visited;
		private int _visitedCount;
		
		public void Generate(List<List<Cell>> cells, MazeConfiguration config)
		{
			_width = config.Width;
			_height = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();
			_visited = new bool[_height, _width];
			_visitedCount = 0;
			
			// Initialize all cells with walls
			InitializeCells(cells);
			
			// Start at a random cell
			int currentX = _random.Next(_width);
			int currentY = _random.Next(_height);
			_visited[currentY, currentX] = true;
			_visitedCount = 1;
			
			int totalCells = _width * _height;
			
			// Random walk until all cells are visited
			while (_visitedCount < totalCells)
			{
				// Choose a random neighbor
				var neighbors = GetNeighbors(currentX, currentY);
				if (neighbors.Count == 0)
					break; // Should never happen in a valid grid
				
				int neighborIndex = _random.Next(neighbors.Count);
				var neighbor = neighbors[neighborIndex];
				
				// If neighbor hasn't been visited, carve passage
				if (!_visited[neighbor.y, neighbor.x])
				{
					RemoveWall(cells, currentX, currentY, neighbor.x, neighbor.y, neighbor.direction);
					_visited[neighbor.y, neighbor.x] = true;
					_visitedCount++;
				}
				
				// Move to neighbor (whether visited or not)
				currentX = neighbor.x;
				currentY = neighbor.y;
			}
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
			
			// Set outer walls
			for (int y = 0; y < _height; y++)
			{
				cells[y][0].Left = true;
				cells[y][_width - 1].Right = true;
			}
			for (int x = 0; x < _width; x++)
			{
				cells[0][x].Top = true;
				cells[_height - 1][x].Bottom = true;
			}
		}
		
		private List<Neighbor> GetNeighbors(int x, int y)
		{
			var neighbors = new List<Neighbor>();
			
			// North
			if (y > 0)
				neighbors.Add(new Neighbor(x, y - 1, "North"));
			
			// South
			if (y < _height - 1)
				neighbors.Add(new Neighbor(x, y + 1, "South"));
			
			// East
			if (x < _width - 1)
				neighbors.Add(new Neighbor(x + 1, y, "East"));
			
			// West
			if (x > 0)
				neighbors.Add(new Neighbor(x - 1, y, "West"));
			
			return neighbors;
		}
		
		private void RemoveWall(List<List<Cell>> cells, int x1, int y1, int x2, int y2, string direction)
		{
			var cell1 = cells[y1][x1];
			var cell2 = cells[y2][x2];
			
			switch (direction)
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
		
		private struct Neighbor
		{
			public int x;
			public int y;
			public string direction;
			
			public Neighbor(int x, int y, string direction)
			{
				this.x = x;
				this.y = y;
				this.direction = direction;
			}
		}
	}
}
