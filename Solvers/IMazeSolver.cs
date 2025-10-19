using System.Collections.Generic;

namespace MazeGenerator.Solvers
{
	/// <summary>
	/// Interface for maze solving algorithms.
	/// </summary>
	public interface IMazeSolver
	{
		/// <summary>
		/// Gets the name of the solver algorithm.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Gets a description of the algorithm and its characteristics.
		/// </summary>
		string Description { get; }
		
		/// <summary>
		/// Finds a path from start to end position in the maze.
		/// </summary>
		/// <param name="maze">The maze to solve.</param>
		/// <param name="start">The starting position (row, column).</param>
		/// <param name="end">The ending position (row, column).</param>
		/// <returns>List of positions representing the path, or empty list if no path exists.</returns>
		List<(int row, int col)> Solve(Maze maze, (int row, int col) start, (int row, int col) end);
	}
}
