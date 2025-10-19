using System;
using System.Collections.Generic;

namespace MazeGenerator.Algorithms
{
	/// <summary>
	/// Prim's algorithm for maze generation.
	/// Creates mazes with many short dead ends using a minimum spanning tree approach.
	/// </summary>
	public class PrimAlgorithm : IMazeAlgorithm
	{
		public string Name => "Prim's Algorithm";
		
		public string Description => "Creates mazes with many short dead ends using a minimum spanning tree approach. Results in more branching and a more 'random' appearance compared to Recursive Backtracker.";
		
		private Random _random;
		private int _width;
		private int _height;
		private bool[,] _inMaze;
		
		public void Generate(List<List<Cell>> cells, MazeConfiguration config)
		{
			_width = config.Width;
			_height = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();
			_inMaze = new bool[_height, _width];
			
			// Initialize all cells with walls
			InitializeCells(cells);
			
			// Start from a random cell
			int startX = _random.Next(_width);
			int startY = _random.Next(_height);
			_inMaze[startY, startX] = true;
			
			// Add walls of starting cell to frontier
			var frontier = new List<Wall>();
			AddWallsToFrontier(frontier, startX, startY);
			
			// Main algorithm loop
			while (frontier.Count > 0)
			{
				// Pick a random wall from frontier
				int wallIndex = _random.Next(frontier.Count);
				var wall = frontier[wallIndex];
				frontier.RemoveAt(wallIndex);
				
				// Check if wall divides cell in maze from cell not in maze
				int inX = wall.x1;
				int inY = wall.y1;
				int outX = wall.x2;
				int outY = wall.y2;
				
				// Ensure inMaze cell is the one already in the maze
				if (!_inMaze[inY, inX] && _inMaze[outY, outX])
				{
					var temp = inX;
					inX = outX;
					outX = temp;
					temp = inY;
					inY = outY;
					outY = temp;
				}
				
				// If exactly one cell is in the maze, remove the wall
				if (_inMaze[inY, inX] && !_inMaze[outY, outX])
				{
					RemoveWall(cells, wall);
					_inMaze[outY, outX] = true;
					AddWallsToFrontier(frontier, outX, outY);
				}
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
		
		private void AddWallsToFrontier(List<Wall> frontier, int x, int y)
		{
			// Add north wall
			if (y > 0 && !_inMaze[y - 1, x])
			{
				frontier.Add(new Wall(x, y, x, y - 1, "North"));
			}
			
			// Add south wall
			if (y < _height - 1 && !_inMaze[y + 1, x])
			{
				frontier.Add(new Wall(x, y, x, y + 1, "South"));
			}
			
			// Add east wall
			if (x < _width - 1 && !_inMaze[y, x + 1])
			{
				frontier.Add(new Wall(x, y, x + 1, y, "East"));
			}
			
			// Add west wall
			if (x > 0 && !_inMaze[y, x - 1])
			{
				frontier.Add(new Wall(x, y, x - 1, y, "West"));
			}
		}
		
		private void RemoveWall(List<List<Cell>> cells, Wall wall)
		{
			var cell1 = cells[wall.y1][wall.x1];
			var cell2 = cells[wall.y2][wall.x2];
			
			switch (wall.direction)
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
		
		private struct Wall
		{
			public int x1, y1, x2, y2;
			public string direction;
			
			public Wall(int x1, int y1, int x2, int y2, string direction)
			{
				this.x1 = x1;
				this.y1 = y1;
				this.x2 = x2;
				this.y2 = y2;
				this.direction = direction;
			}
		}
	}
}
