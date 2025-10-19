using System.Collections.Generic;
using MazeGenerator.Enums;

namespace MazeGenerator
{
	/// <summary>
	/// Maze class with support for various maze generation algorithms and types.
	/// Use MazeFactory for creating mazes with semantic factory methods.
	/// </summary>
	public class Maze
	{
		private readonly MazeGrid _grid;

		/// <summary>
		/// Gets the cells of the maze as a read-only collection.
		/// </summary>
		public IReadOnlyList<IReadOnlyList<Cell>> Cells => _grid.Cells;

		/// <summary>
		/// Gets the width of the maze (number of columns).
		/// </summary>
		public int Width => _grid.Width;

		/// <summary>
		/// Gets the height of the maze (number of rows).
		/// </summary>
		public int Height => _grid.Height;

		/// <summary>
		/// Gets the configuration used to generate this maze.
		/// </summary>
		public MazeConfiguration Configuration => _grid.Configuration;

		/// <summary>
		/// Gets the underlying maze grid.
		/// </summary>
		public MazeGrid Grid => _grid;

		/// <summary>
		/// Creates a new maze with the specified dimensions.
		/// Uses Eller's algorithm by default for backward compatibility.
		/// </summary>
		/// <param name="columnsCount">The number of columns (width).</param>
		/// <param name="rowsCount">The number of rows (height).</param>
		public Maze(int columnsCount, int rowsCount)
		{
			var config = MazeConfiguration.Default(columnsCount, rowsCount);
			_grid = new MazeGrid(config);
		}

		/// <summary>
		/// Creates a new maze with the specified dimensions and algorithm.
		/// </summary>
		/// <param name="columnsCount">The number of columns (width).</param>
		/// <param name="rowsCount">The number of rows (height).</param>
		/// <param name="algorithm">The algorithm to use for generation.</param>
		public Maze(int columnsCount, int rowsCount, MazeAlgorithmType algorithm)
		{
			var config = new MazeConfiguration
			{
				Width = columnsCount,
				Height = rowsCount,
				Algorithm = algorithm
			};
			_grid = new MazeGrid(config);
		}

		/// <summary>
		/// Creates a new maze with the specified dimensions, algorithm, and seed.
		/// </summary>
		/// <param name="columnsCount">The number of columns (width).</param>
		/// <param name="rowsCount">The number of rows (height).</param>
		/// <param name="algorithm">The algorithm to use for generation.</param>
		/// <param name="seed">The random seed for reproducible generation.</param>
		public Maze(int columnsCount, int rowsCount, MazeAlgorithmType algorithm, int seed)
		{
			var config = new MazeConfiguration
			{
				Width = columnsCount,
				Height = rowsCount,
				Algorithm = algorithm,
				Seed = seed
			};
			_grid = new MazeGrid(config);
		}

		/// <summary>
		/// Creates a new maze with the specified configuration.
		/// </summary>
		/// <param name="configuration">The maze configuration.</param>
		public Maze(MazeConfiguration configuration)
		{
			_grid = new MazeGrid(configuration);
		}

		/// <summary>
		/// Gets a cell at the specified position.
		/// </summary>
		/// <param name="row">The row index (0-based).</param>
		/// <param name="column">The column index (0-based).</param>
		/// <returns>The cell at the specified position.</returns>
		public Cell GetCell(int row, int column)
		{
			return _grid.GetCell(row, column);
		}
	}
}