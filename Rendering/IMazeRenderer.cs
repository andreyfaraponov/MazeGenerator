namespace MazeGenerator.Rendering
{
	/// <summary>
	/// Interface for maze renderers that convert mazes to various output formats.
	/// </summary>
	public interface IMazeRenderer
	{
		/// <summary>
		/// Gets the name of the renderer.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Gets a description of the output format.
		/// </summary>
		string Description { get; }
		
		/// <summary>
		/// Gets the file extension for the output format (without dot).
		/// </summary>
		string FileExtension { get; }
		
		/// <summary>
		/// Renders the maze to a string representation.
		/// </summary>
		/// <param name="maze">The maze to render.</param>
		/// <returns>String representation of the maze.</returns>
		string Render(Maze maze);
		
		/// <summary>
		/// Renders the maze with custom configuration.
		/// </summary>
		/// <param name="maze">The maze to render.</param>
		/// <param name="config">The rendering configuration.</param>
		/// <returns>String representation of the maze.</returns>
		string Render(Maze maze, RenderConfiguration config);
	}
}
