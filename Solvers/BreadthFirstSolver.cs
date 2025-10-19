using System;
using System.Collections.Generic;

namespace MazeGenerator.Solvers
{
	/// <summary>
	/// Solves mazes using Breadth-First Search (BFS) algorithm.
	/// Guarantees the shortest path between start and end positions.
	/// </summary>
	public class BreadthFirstSolver : IMazeSolver
	{
		public string Name => "Breadth-First Search";
		
		public string Description => "Guarantees the shortest path between two points. Explores all neighbors at current depth before moving deeper. Best for finding optimal solutions.";
		
		public List<(int row, int col)> Solve(Maze maze, (int row, int col) start, (int row, int col) end)
		{
			if (!IsValidPosition(maze, start) || !IsValidPosition(maze, end))
				return new List<(int row, int col)>();
			
			var queue = new Queue<(int row, int col)>();
			var visited = new HashSet<(int row, int col)>();
			var parent = new Dictionary<(int row, int col), (int row, int col)>();
			
			queue.Enqueue(start);
			visited.Add(start);
			parent[start] = (-1, -1); // No parent for start
			
			while (queue.Count > 0)
			{
				var current = queue.Dequeue();
				
				if (current == end)
				{
					// Reconstruct path
					return ReconstructPath(parent, start, end);
				}
				
				// Explore neighbors
				foreach (var neighbor in GetAccessibleNeighbors(maze, current))
				{
					if (!visited.Contains(neighbor))
					{
						visited.Add(neighbor);
						parent[neighbor] = current;
						queue.Enqueue(neighbor);
					}
				}
			}
			
			// No path found
			return new List<(int row, int col)>();
		}
		
		private bool IsValidPosition(Maze maze, (int row, int col) pos)
		{
			return pos.row >= 0 && pos.row < maze.Height &&
			       pos.col >= 0 && pos.col < maze.Width;
		}
		
		private List<(int row, int col)> GetAccessibleNeighbors(Maze maze, (int row, int col) pos)
		{
			var neighbors = new List<(int row, int col)>();
			var cell = maze.GetCell(pos.row, pos.col);
			
			// North
			if (pos.row > 0 && !cell.Top)
				neighbors.Add((pos.row - 1, pos.col));
			
			// South
			if (pos.row < maze.Height - 1 && !cell.Bottom)
				neighbors.Add((pos.row + 1, pos.col));
			
			// East
			if (pos.col < maze.Width - 1 && !cell.Right)
				neighbors.Add((pos.row, pos.col + 1));
			
			// West
			if (pos.col > 0 && !cell.Left)
				neighbors.Add((pos.row, pos.col - 1));
			
			return neighbors;
		}
		
		private List<(int row, int col)> ReconstructPath(
			Dictionary<(int row, int col), (int row, int col)> parent,
			(int row, int col) start,
			(int row, int col) end)
		{
			var path = new List<(int row, int col)>();
			var current = end;
			
			while (current != start)
			{
				path.Add(current);
				current = parent[current];
			}
			
			path.Add(start);
			path.Reverse();
			
			return path;
		}
	}
}
