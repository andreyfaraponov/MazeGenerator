using System;
using System.Collections.Generic;

namespace MazeGenerator.Algorithms
{
	/// <summary>
	/// Recursive Division algorithm for maze generation.
	/// Creates chambers and corridors by recursively dividing space.
	/// </summary>
	public class RecursiveDivisionAlgorithm : IMazeAlgorithm
	{
		public string Name => "Recursive Division";
		
		public string Description => "Creates chambers and corridors by recursively dividing space. Starts with an empty area and adds walls, creating a room-like structure. Good for dungeon-style layouts.";
		
		private Random _random;
		private int _width;
		private int _height;
		
		public void Generate(List<List<Cell>> cells, MazeConfiguration config)
		{
			_width = config.Width;
			_height = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();
			
			// Start with no internal walls (empty maze)
			InitializeCells(cells);
			
			// Recursively divide the space
			Divide(cells, 0, 0, _width, _height);
		}
		
		private void InitializeCells(List<List<Cell>> cells)
		{
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					var cell = cells[y][x];
					cell.Left = false;
					cell.Right = false;
					cell.Top = false;
					cell.Bottom = false;
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
		
		private void Divide(List<List<Cell>> cells, int x, int y, int width, int height)
		{
			// Base case: if chamber is too small, don't divide
			if (width < 2 || height < 2)
				return;
			
			// Decide whether to divide horizontally or vertically
			bool horizontal;
			if (width > height)
				horizontal = false;
			else if (height > width)
				horizontal = true;
			else
				horizontal = _random.Next(2) == 0;
			
			if (horizontal)
			{
				DivideHorizontally(cells, x, y, width, height);
			}
			else
			{
				DivideVertically(cells, x, y, width, height);
			}
		}
		
		private void DivideHorizontally(List<List<Cell>> cells, int x, int y, int width, int height)
		{
			// Choose a random row to divide on (not on the edges)
			int divideY = y + _random.Next(height - 1);
			
			// Choose a random column for the passage
			int passageX = x + _random.Next(width);
			
			// Add horizontal wall with one passage
			for (int i = 0; i < width; i++)
			{
				if (x + i != passageX)
				{
					// Add wall to bottom of cell at divideY
					if (divideY < _height)
					{
						cells[divideY][x + i].Bottom = true;
						if (divideY + 1 < _height)
							cells[divideY + 1][x + i].Top = true;
					}
				}
			}
			
			// Recursively divide the two new chambers
			Divide(cells, x, y, width, divideY - y + 1);
			Divide(cells, x, divideY + 1, width, height - (divideY - y + 1));
		}
		
		private void DivideVertically(List<List<Cell>> cells, int x, int y, int width, int height)
		{
			// Choose a random column to divide on (not on the edges)
			int divideX = x + _random.Next(width - 1);
			
			// Choose a random row for the passage
			int passageY = y + _random.Next(height);
			
			// Add vertical wall with one passage
			for (int i = 0; i < height; i++)
			{
				if (y + i != passageY)
				{
					// Add wall to right of cell at divideX
					if (divideX < _width)
					{
						cells[y + i][divideX].Right = true;
						if (divideX + 1 < _width)
							cells[y + i][divideX + 1].Left = true;
					}
				}
			}
			
			// Recursively divide the two new chambers
			Divide(cells, x, y, divideX - x + 1, height);
			Divide(cells, divideX + 1, y, width - (divideX - x + 1), height);
		}
	}
}
