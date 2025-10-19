namespace MazeGenerator.Enums
{
	/// <summary>
	/// Specifies the algorithm to use for maze generation.
	/// </summary>
	public enum MazeAlgorithmType
	{
		/// <summary>
		/// Eller's algorithm - generates perfect mazes row by row.
		/// Memory efficient, good for large mazes.
		/// </summary>
		Eller,
		
		/// <summary>
		/// Recursive Backtracker - creates long, winding passages.
		/// Uses depth-first search with backtracking.
		/// </summary>
		RecursiveBacktracker,
		
		/// <summary>
		/// Prim's algorithm - creates many short dead ends.
		/// Uses minimum spanning tree approach.
		/// </summary>
		Prim,
		
		/// <summary>
		/// Recursive Division - creates chambers and corridors.
		/// Divides space recursively.
		/// </summary>
		RecursiveDivision,
		
		/// <summary>
		/// Aldous-Broder algorithm - random walk approach.
		/// Creates uniform spanning tree, slower but unbiased.
		/// </summary>
		AldousBroder
	}
}
