using System.Text;

namespace MazeGenerator.Rendering
{
	/// <summary>
	/// Renders mazes as SVG (Scalable Vector Graphics) format.
	/// </summary>
	public class SvgRenderer : IMazeRenderer
	{
		public string Name => "SVG Renderer";
		
		public string Description => "Renders mazes as Scalable Vector Graphics (SVG). Perfect for web display, printing, and high-quality output.";
		
		public string FileExtension => "svg";
		
		/// <summary>
		/// Renders the maze using default configuration.
		/// </summary>
		public string Render(Maze maze)
		{
			return Render(maze, RenderConfiguration.Default());
		}
		
		/// <summary>
		/// Renders the maze with custom configuration.
		/// </summary>
		public string Render(Maze maze, RenderConfiguration config)
		{
			int cellSize = config.CellSize;
			int wallThickness = config.WallThickness;
			int width = maze.Width;
			int height = maze.Height;
			
			int svgWidth = width * cellSize + wallThickness;
			int svgHeight = height * cellSize + wallThickness;
			
			var sb = new StringBuilder();
			
			// SVG header
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" " +
			             $"width=\"{svgWidth}\" height=\"{svgHeight}\" " +
			             $"viewBox=\"0 0 {svgWidth} {svgHeight}\">");
			
			// Background
			sb.AppendLine($"  <rect width=\"{svgWidth}\" height=\"{svgHeight}\" " +
			             $"fill=\"{config.BackgroundColor}\" />");
			
			// Draw walls
			sb.AppendLine($"  <g stroke=\"{config.WallColor}\" " +
			             $"stroke-width=\"{wallThickness}\" " +
			             $"stroke-linecap=\"square\">");
			
			// Draw horizontal walls
			for (int row = 0; row < height; row++)
			{
				for (int col = 0; col < width; col++)
				{
					var cell = maze.GetCell(row, col);
					int x = col * cellSize + wallThickness / 2;
					int y = row * cellSize + wallThickness / 2;
					
					// Top wall
					if (cell.Top)
					{
						int x1 = x;
						int y1 = y;
						int x2 = x + cellSize;
						int y2 = y;
						sb.AppendLine($"    <line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" />");
					}
					
					// Bottom wall
					if (cell.Bottom)
					{
						int x1 = x;
						int y1 = y + cellSize;
						int x2 = x + cellSize;
						int y2 = y + cellSize;
						sb.AppendLine($"    <line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" />");
					}
					
					// Left wall
					if (cell.Left)
					{
						int x1 = x;
						int y1 = y;
						int x2 = x;
						int y2 = y + cellSize;
						sb.AppendLine($"    <line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" />");
					}
					
					// Right wall
					if (cell.Right)
					{
						int x1 = x + cellSize;
						int y1 = y;
						int x2 = x + cellSize;
						int y2 = y + cellSize;
						sb.AppendLine($"    <line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" />");
					}
				}
			}
			
			sb.AppendLine("  </g>");
			
			// Draw markers if enabled
			if (config.ShowMarkers)
			{
				var startPos = config.StartPosition ?? (0, 0);
				var endPos = config.EndPosition ?? (height - 1, width - 1);
				
				// Start marker (green circle)
				int startX = startPos.col * cellSize + cellSize / 2 + wallThickness / 2;
				int startY = startPos.row * cellSize + cellSize / 2 + wallThickness / 2;
				int markerRadius = cellSize / 4;
				sb.AppendLine($"  <circle cx=\"{startX}\" cy=\"{startY}\" r=\"{markerRadius}\" " +
				             $"fill=\"#00FF00\" stroke=\"#000000\" stroke-width=\"1\" />");
				
				// End marker (red circle)
				int endX = endPos.col * cellSize + cellSize / 2 + wallThickness / 2;
				int endY = endPos.row * cellSize + cellSize / 2 + wallThickness / 2;
				sb.AppendLine($"  <circle cx=\"{endX}\" cy=\"{endY}\" r=\"{markerRadius}\" " +
				             $"fill=\"#FF0000\" stroke=\"#000000\" stroke-width=\"1\" />");
			}
			
			// SVG footer
			sb.AppendLine("</svg>");
			
			return sb.ToString();
		}
		
		/// <summary>
		/// Renders the maze with a solution path highlighted.
		/// </summary>
		/// <param name="maze">The maze to render.</param>
		/// <param name="path">List of (row, col) coordinates representing the solution path.</param>
		/// <param name="config">Rendering configuration.</param>
		public string RenderWithPath(Maze maze, System.Collections.Generic.List<(int row, int col)> path, RenderConfiguration config)
		{
			// First render the basic maze
			string basicSvg = Render(maze, config);
			
			// Remove the closing </svg> tag
			int closingTag = basicSvg.LastIndexOf("</svg>");
			var sb = new StringBuilder(basicSvg.Substring(0, closingTag));
			
			// Add path
			if (path != null && path.Count > 1)
			{
				int cellSize = config.CellSize;
				int wallThickness = config.WallThickness;
				
				sb.AppendLine($"  <g stroke=\"{config.PathColor}\" " +
				             $"stroke-width=\"{cellSize / 4}\" " +
				             $"stroke-linecap=\"round\" " +
				             $"fill=\"none\" opacity=\"0.6\">");
				
				sb.Append("    <path d=\"");
				for (int i = 0; i < path.Count; i++)
				{
					int x = path[i].col * cellSize + cellSize / 2 + wallThickness / 2;
					int y = path[i].row * cellSize + cellSize / 2 + wallThickness / 2;
					
					if (i == 0)
						sb.Append($"M {x} {y}");
					else
						sb.Append($" L {x} {y}");
				}
				sb.AppendLine("\" />");
				sb.AppendLine("  </g>");
			}
			
			// Add closing tag back
			sb.AppendLine("</svg>");
			
			return sb.ToString();
		}
	}
}
