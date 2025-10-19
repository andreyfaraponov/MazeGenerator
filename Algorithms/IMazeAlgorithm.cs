using System.Collections.Generic;

namespace MazeGenerator.Algorithms
{
	/// <summary>
	/// Interface for maze generation algorithms.
	/// </summary>
	public interface IMazeAlgorithm
	{
		/// <summary>
		/// Gets the name of the algorithm.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Gets a description of the algorithm and its characteristics.
		/// </summary>
		string Description { get; }
		
		/// <summary>
		/// Generates a maze by modifying the provided cell grid.
		/// </summary>
		/// <param name="cells">The grid of cells to generate the maze in.</param>
		/// <param name="config">The maze configuration.</param>
		void Generate(List<List<Cell>> cells, MazeConfiguration config);
	}
}
