using System;
using System.Collections.Generic;

namespace MazeGenerator.Solvers
{
	/// <summary>
	/// Solves mazes using Depth-First Search (DFS) algorithm.
	/// Finds a path but not necessarily the shortest one.
	/// </summary>
	public class DepthFirstSolver : IMazeSolver
	{
		public string Name => "Depth-First Search";
		
		public string Description => "Explores as far as possible along each branch before backtracking. Fast but doesn't guarantee the shortest path. Good for finding any solution quickly.";
		
		private HashSet<(int row, int col)> _visited;
		private Maze _maze;
		
		public List<(int row, int col)> Solve(Maze maze, (int row, int col) start, (int row, int col) end)
		{
			if (!IsValidPosition(maze, start) || !IsValidPosition(maze, end))
				return new List<(int row, int col)>();
			
			_maze = maze;
			_visited = new HashSet<(int row, int col)>();
			var path = new List<(int row, int col)>();
			
			if (DFS(start, end, path))
			{
				return path;
			}
			
			return new List<(int row, int col)>();
		}
		
		private bool DFS((int row, int col) current, (int row, int col) end, List<(int row, int col)> path)
		{
			path.Add(current);
			_visited.Add(current);
			
			if (current == end)
				return true;
			
			foreach (var neighbor in GetAccessibleNeighbors(current))
			{
				if (!_visited.Contains(neighbor))
				{
					if (DFS(neighbor, end, path))
						return true;
				}
			}
			
			// Backtrack
			path.RemoveAt(path.Count - 1);
			return false;
		}
		
		private bool IsValidPosition(Maze maze, (int row, int col) pos)
		{
			return pos.row >= 0 && pos.row < maze.Height &&
			       pos.col >= 0 && pos.col < maze.Width;
		}
		
		private List<(int row, int col)> GetAccessibleNeighbors((int row, int col) pos)
		{
			var neighbors = new List<(int row, int col)>();
			var cell = _maze.GetCell(pos.row, pos.col);
			
			// North
			if (pos.row > 0 && !cell.Top)
				neighbors.Add((pos.row - 1, pos.col));
			
			// South
			if (pos.row < _maze.Height - 1 && !cell.Bottom)
				neighbors.Add((pos.row + 1, pos.col));
			
			// East
			if (pos.col < _maze.Width - 1 && !cell.Right)
				neighbors.Add((pos.row, pos.col + 1));
			
			// West
			if (pos.col > 0 && !cell.Left)
				neighbors.Add((pos.row, pos.col - 1));
			
			return neighbors;
		}
	}
}
