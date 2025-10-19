using System.Collections.Generic;

namespace MazeGenerator.Modifiers
{
	/// <summary>
	/// Interface for maze modifiers that alter generated mazes.
	/// </summary>
	public interface IMazeModifier
	{
		/// <summary>
		/// Gets the name of the modifier.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Gets a description of what the modifier does.
		/// </summary>
		string Description { get; }
		
		/// <summary>
		/// Applies the modification to the maze cells.
		/// </summary>
		/// <param name="cells">The grid of cells to modify.</param>
		/// <param name="config">The maze configuration.</param>
		void Apply(List<List<Cell>> cells, MazeConfiguration config);
	}
}
