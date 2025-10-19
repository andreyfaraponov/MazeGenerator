using System.Text;

namespace MazeGenerator.Rendering
{
	/// <summary>
	/// Renders mazes as ASCII art using box-drawing characters.
	/// </summary>
	public class AsciiRenderer : IMazeRenderer
	{
		public string Name => "ASCII Renderer";
		
		public string Description => "Renders mazes as ASCII art using box-drawing characters. Suitable for console output and text files.";
		
		public string FileExtension => "txt";
		
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
			var sb = new StringBuilder();
			int width = maze.Width;
			int height = maze.Height;
			
			// Render top border
			sb.Append("+");
			for (int col = 0; col < width; col++)
			{
				sb.Append("---+");
			}
			sb.AppendLine();
			
			// Render each row
			for (int row = 0; row < height; row++)
			{
				// Render cell row
				sb.Append("|");
				for (int col = 0; col < width; col++)
				{
					var cell = maze.GetCell(row, col);
					
					// Cell content (marker or space)
					if (config.ShowMarkers)
					{
						var startPos = config.StartPosition ?? (0, 0);
						var endPos = config.EndPosition ?? (height - 1, width - 1);
						
						if (row == startPos.row && col == startPos.col)
							sb.Append(" S ");
						else if (row == endPos.row && col == endPos.col)
							sb.Append(" E ");
						else
							sb.Append("   ");
					}
					else
					{
						sb.Append("   ");
					}
					
					// Right wall
					sb.Append(cell.Right ? "|" : " ");
				}
				sb.AppendLine();
				
				// Render bottom wall row
				sb.Append("+");
				for (int col = 0; col < width; col++)
				{
					var cell = maze.GetCell(row, col);
					sb.Append(cell.Bottom ? "---" : "   ");
					sb.Append("+");
				}
				sb.AppendLine();
			}
			
			return sb.ToString();
		}
		
		/// <summary>
		/// Renders the maze using Unicode box-drawing characters for better appearance.
		/// </summary>
		public string RenderUnicode(Maze maze)
		{
			var sb = new StringBuilder();
			int width = maze.Width;
			int height = maze.Height;
			
			// Render top border
			sb.Append("┌");
			for (int col = 0; col < width; col++)
			{
				sb.Append("───");
				sb.Append(col < width - 1 ? "┬" : "┐");
			}
			sb.AppendLine();
			
			// Render each row
			for (int row = 0; row < height; row++)
			{
				// Cell row
				sb.Append("│");
				for (int col = 0; col < width; col++)
				{
					var cell = maze.GetCell(row, col);
					sb.Append("   ");
					sb.Append(cell.Right ? "│" : " ");
				}
				sb.AppendLine();
				
				// Bottom wall row
				if (row < height - 1)
				{
					sb.Append("├");
					for (int col = 0; col < width; col++)
					{
						var cell = maze.GetCell(row, col);
						sb.Append(cell.Bottom ? "───" : "   ");
						
						// Junction character
						if (col < width - 1)
						{
							var rightCell = maze.GetCell(row, col + 1);
							bool hasBottom = cell.Bottom;
							bool hasBottomRight = rightCell.Bottom;
							bool hasRight = cell.Right;
							var cellBelow = maze.GetCell(row + 1, col);
							bool hasBelowRight = cellBelow.Right;
							
							// Determine junction type
							if (hasBottom && hasBottomRight && hasRight && hasBelowRight)
								sb.Append("┼");
							else if (hasBottom && hasBottomRight)
								sb.Append("┬");
							else if (hasRight && hasBelowRight)
								sb.Append("│");
							else if (hasBottom || hasBottomRight)
								sb.Append("─");
							else
								sb.Append(" ");
						}
						else
						{
							sb.Append(cell.Bottom ? "┤" : "│");
						}
					}
					sb.AppendLine();
				}
			}
			
			// Bottom border
			sb.Append("└");
			for (int col = 0; col < width; col++)
			{
				sb.Append("───");
				sb.Append(col < width - 1 ? "┴" : "┘");
			}
			sb.AppendLine();
			
			return sb.ToString();
		}
	}
}
