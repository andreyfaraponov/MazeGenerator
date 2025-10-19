namespace MazeGenerator.Rendering
{
	/// <summary>
	/// Configuration for maze rendering.
	/// </summary>
	public class RenderConfiguration
	{
		/// <summary>
		/// Gets or sets the cell size in pixels (for graphical renderers).
		/// </summary>
		public int CellSize { get; set; } = 20;
		
		/// <summary>
		/// Gets or sets the wall thickness in pixels.
		/// </summary>
		public int WallThickness { get; set; } = 2;
		
		/// <summary>
		/// Gets or sets the wall color (hex format, e.g., "#000000").
		/// </summary>
		public string WallColor { get; set; } = "#000000";
		
		/// <summary>
		/// Gets or sets the background color (hex format, e.g., "#FFFFFF").
		/// </summary>
		public string BackgroundColor { get; set; } = "#FFFFFF";
		
		/// <summary>
		/// Gets or sets the path color for solution highlighting (hex format).
		/// </summary>
		public string PathColor { get; set; } = "#FF0000";
		
		/// <summary>
		/// Gets or sets whether to include start and end markers.
		/// </summary>
		public bool ShowMarkers { get; set; } = false;
		
		/// <summary>
		/// Gets or sets the start marker position (row, column). If null, uses top-left.
		/// </summary>
		public (int row, int col)? StartPosition { get; set; }
		
		/// <summary>
		/// Gets or sets the end marker position (row, column). If null, uses bottom-right.
		/// </summary>
		public (int row, int col)? EndPosition { get; set; }
		
		/// <summary>
		/// Gets or sets whether to draw grid lines inside cells.
		/// </summary>
		public bool ShowGrid { get; set; } = false;
		
		/// <summary>
		/// Creates a default rendering configuration.
		/// </summary>
		public static RenderConfiguration Default()
		{
			return new RenderConfiguration();
		}
		
		/// <summary>
		/// Creates a configuration for small mazes.
		/// </summary>
		public static RenderConfiguration Small()
		{
			return new RenderConfiguration
			{
				CellSize = 15,
				WallThickness = 1
			};
		}
		
		/// <summary>
		/// Creates a configuration for large mazes.
		/// </summary>
		public static RenderConfiguration Large()
		{
			return new RenderConfiguration
			{
				CellSize = 30,
				WallThickness = 3
			};
		}
	}
}
