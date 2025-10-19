using System;
using System.Collections.Generic;
using MazeGenerator.Algorithms;
using MazeGenerator.Enums;
using MazeGenerator.Modifiers;

namespace MazeGenerator
{
	/// <summary>
	/// Represents a maze grid with cells and walls.
	/// </summary>
	public class MazeGrid
	{
		private readonly MazeConfiguration _configuration;
		private readonly List<List<Cell>> _cells;
		
		/// <summary>
		/// Gets the cells of the maze as a read-only collection.
		/// </summary>
		public IReadOnlyList<IReadOnlyList<Cell>> Cells => _cells;
		
		/// <summary>
		/// Gets the width of the maze (number of columns).
		/// </summary>
		public int Width => _configuration.Width;
		
		/// <summary>
		/// Gets the height of the maze (number of rows).
		/// </summary>
		public int Height => _configuration.Height;
		
		/// <summary>
		/// Gets the configuration used to generate this maze.
		/// </summary>
		public MazeConfiguration Configuration => _configuration;
		
		/// <summary>
		/// Creates a new maze grid with the specified configuration.
		/// </summary>
		/// <param name="configuration">The maze configuration.</param>
		public MazeGrid(MazeConfiguration configuration)
		{
			configuration.Validate();
			_configuration = configuration;
			
			// Initialize the grid with empty cells
			_cells = new List<List<Cell>>(configuration.Height);
			for (int row = 0; row < configuration.Height; row++)
			{
				var rowList = new List<Cell>(configuration.Width);
				for (int col = 0; col < configuration.Width; col++)
				{
					rowList.Add(new Cell(0));
				}
				_cells.Add(rowList);
			}
			
			// Generate the maze using the specified algorithm
			var algorithm = GetAlgorithm(configuration.Algorithm);
			algorithm.Generate(_cells, configuration);
			
			// Apply modifiers based on maze type
			ApplyModifiers(configuration);
		}
		
		/// <summary>
		/// Gets a cell at the specified position.
		/// </summary>
		/// <param name="row">The row index (0-based).</param>
		/// <param name="column">The column index (0-based).</param>
		/// <returns>The cell at the specified position.</returns>
		public Cell GetCell(int row, int column)
		{
			if (row < 0 || row >= Height)
				throw new ArgumentOutOfRangeException(nameof(row));
			
			if (column < 0 || column >= Width)
				throw new ArgumentOutOfRangeException(nameof(column));
			
			return _cells[row][column];
		}
		
		private void ApplyModifiers(MazeConfiguration config)
		{
			if (config.Type == MazeType.Braided && config.BraidingFactor > 0.0)
			{
				var braidingModifier = new BraidingModifier();
				braidingModifier.Apply(_cells, config);
			}
			
			if (config.Type == MazeType.WithRooms && config.RoomCount > 0)
			{
				var roomModifier = new RoomModifier();
				roomModifier.Apply(_cells, config);
			}
		}
		
		private IMazeAlgorithm GetAlgorithm(MazeAlgorithmType algorithmType)
		{
			switch (algorithmType)
			{
				case MazeAlgorithmType.Eller:
					return new EllerAlgorithm();
				
				case MazeAlgorithmType.RecursiveBacktracker:
					return new RecursiveBacktrackerAlgorithm();
				
				case MazeAlgorithmType.Prim:
					return new PrimAlgorithm();
				
				case MazeAlgorithmType.RecursiveDivision:
					return new RecursiveDivisionAlgorithm();
				
				case MazeAlgorithmType.AldousBroder:
					return new AldousBroderAlgorithm();
				
				default:
					throw new ArgumentException($"Unknown algorithm type: {algorithmType}", nameof(algorithmType));
			}
		}
	}
}
