using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Modifiers
{
	/// <summary>
	/// Modifier that creates braided mazes by removing dead ends to add loops.
	/// Braided mazes have multiple solution paths instead of a single path.
	/// </summary>
	public class BraidingModifier : IMazeModifier
	{
		public string Name => "Braiding Modifier";
		
		public string Description => "Removes dead ends to create loops in the maze, providing multiple solution paths. Braiding factor controls how many dead ends to remove (0.0 = none, 1.0 = all).";
		
		private Random _random;
		private int _width;
		private int _height;
		
		public void Apply(List<List<Cell>> cells, MazeConfiguration config)
		{
			if (config.BraidingFactor <= 0.0)
				return; // No braiding needed
			
			_width = config.Width;
			_height = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value + 1000) : new Random();
			
			// Find all dead ends
			var deadEnds = FindDeadEnds(cells);
			
			// Calculate how many to remove
			int deadEndsToRemove = (int)Math.Ceiling(deadEnds.Count * config.BraidingFactor);
			
			// Shuffle dead ends for random selection
			ShuffleList(deadEnds);
			
			// Remove dead ends
			int removed = 0;
			foreach (var deadEnd in deadEnds)
			{
				if (removed >= deadEndsToRemove)
					break;
				
				if (RemoveDeadEnd(cells, deadEnd.row, deadEnd.col))
					removed++;
			}
		}
		
		private List<(int row, int col)> FindDeadEnds(List<List<Cell>> cells)
		{
			var deadEnds = new List<(int row, int col)>();
			
			for (int row = 0; row < _height; row++)
			{
				for (int col = 0; col < _width; col++)
				{
					if (IsDeadEnd(cells, row, col))
					{
						deadEnds.Add((row, col));
					}
				}
			}
			
			return deadEnds;
		}
		
		private bool IsDeadEnd(List<List<Cell>> cells, int row, int col)
		{
			var cell = cells[row][col];
			int wallCount = 0;
			
			// Count walls (excluding outer boundaries)
			if (cell.Top && row == 0) // Outer top wall doesn't count
				wallCount--;
			if (cell.Bottom && row == _height - 1) // Outer bottom wall doesn't count
				wallCount--;
			if (cell.Left && col == 0) // Outer left wall doesn't count
				wallCount--;
			if (cell.Right && col == _width - 1) // Outer right wall doesn't count
				wallCount--;
			
			if (cell.Top) wallCount++;
			if (cell.Bottom) wallCount++;
			if (cell.Left) wallCount++;
			if (cell.Right) wallCount++;
			
			// A dead end has exactly 3 walls (one opening)
			return wallCount == 3;
		}
		
		private bool RemoveDeadEnd(List<List<Cell>> cells, int row, int col)
		{
			var cell = cells[row][col];
			
			// Find walls that can be removed (not on outer boundary)
			var removableWalls = new List<Direction>();
			
			if (cell.Top && row > 0)
				removableWalls.Add(new Direction("Top", 0, -1));
			
			if (cell.Bottom && row < _height - 1)
				removableWalls.Add(new Direction("Bottom", 0, 1));
			
			if (cell.Left && col > 0)
				removableWalls.Add(new Direction("Left", -1, 0));
			
			if (cell.Right && col < _width - 1)
				removableWalls.Add(new Direction("Right", 1, 0));
			
			if (removableWalls.Count == 0)
				return false;
			
			// Choose a random wall to remove
			var wallToRemove = removableWalls[_random.Next(removableWalls.Count)];
			
			// Remove the wall between this cell and its neighbor
			int neighborCol = col + wallToRemove.dx;
			int neighborRow = row + wallToRemove.dy;
			var neighbor = cells[neighborRow][neighborCol];
			
			switch (wallToRemove.name)
			{
				case "Top":
					cell.Top = false;
					neighbor.Bottom = false;
					break;
				case "Bottom":
					cell.Bottom = false;
					neighbor.Top = false;
					break;
				case "Left":
					cell.Left = false;
					neighbor.Right = false;
					break;
				case "Right":
					cell.Right = false;
					neighbor.Left = false;
					break;
			}
			
			return true;
		}
		
		private void ShuffleList<T>(List<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int j = _random.Next(i + 1);
				T temp = list[i];
				list[i] = list[j];
				list[j] = temp;
			}
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
