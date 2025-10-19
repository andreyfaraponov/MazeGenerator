namespace MazeGenerator.Enums
{
	/// <summary>
	/// Specifies the type of maze to generate.
	/// </summary>
	public enum MazeType
	{
		/// <summary>
		/// Perfect maze - exactly one path between any two cells.
		/// No loops, no isolated areas.
		/// </summary>
		Perfect,
		
		/// <summary>
		/// Braided maze - contains loops by removing some dead ends.
		/// Multiple paths between cells may exist.
		/// </summary>
		Braided,
		
		/// <summary>
		/// Maze with rooms - contains larger open rectangular spaces.
		/// Combines corridors with chambers.
		/// </summary>
		WithRooms
	}
}
