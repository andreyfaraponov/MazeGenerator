using System;
using MazeGenerator.Enums;

namespace MazeGenerator
{
	/// <summary>
	/// Factory for creating maze instances with various configurations.
	/// Provides semantic methods for creating specific types of mazes.
	/// </summary>
	public static class MazeFactory
	{
		/// <summary>
		/// Creates a maze with the specified configuration.
		/// </summary>
		/// <param name="configuration">The maze configuration.</param>
		/// <returns>A generated maze.</returns>
		public static Maze Create(MazeConfiguration configuration)
		{
			return new Maze(configuration);
		}
		
		/// <summary>
		/// Creates a maze with the specified width and height using default settings.
		/// </summary>
		/// <param name="width">The width of the maze (number of columns).</param>
		/// <param name="height">The height of the maze (number of rows).</param>
		/// <returns>A generated maze.</returns>
		public static Maze Create(int width, int height)
		{
			var config = MazeConfiguration.Default(width, height);
			return new Maze(config);
		}
		
		/// <summary>
		/// Creates a maze with the specified width, height, and algorithm.
		/// </summary>
		/// <param name="width">The width of the maze (number of columns).</param>
		/// <param name="height">The height of the maze (number of rows).</param>
		/// <param name="algorithm">The algorithm to use for generation.</param>
		/// <returns>A generated maze.</returns>
		public static Maze Create(int width, int height, MazeAlgorithmType algorithm)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = algorithm
			};
			return new Maze(config);
		}
		
		/// <summary>
		/// Creates a maze with the specified width, height, algorithm, and seed for reproducibility.
		/// </summary>
		/// <param name="width">The width of the maze (number of columns).</param>
		/// <param name="height">The height of the maze (number of rows).</param>
		/// <param name="algorithm">The algorithm to use for generation.</param>
		/// <param name="seed">The random seed for reproducible generation.</param>
		/// <returns>A generated maze.</returns>
		public static Maze Create(int width, int height, MazeAlgorithmType algorithm, int seed)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = algorithm,
				Seed = seed
			};
			return new Maze(config);
		}
		
		/// <summary>
		/// Creates a maze with long, winding corridors (Recursive Backtracker).
		/// Best for challenging puzzles.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateChallenging(int width, int height, int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = MazeAlgorithmType.RecursiveBacktracker,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a maze with many short branches (Prim's Algorithm).
		/// Best for exploration and adventure games.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateExploratory(int width, int height, int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = MazeAlgorithmType.Prim,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a maze with room-like chambers (Recursive Division).
		/// Best for dungeon-style layouts.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateDungeon(int width, int height, int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = MazeAlgorithmType.RecursiveDivision,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a balanced maze with good performance (Eller's Algorithm).
		/// Default algorithm, good for general purpose use.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateBalanced(int width, int height, int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = MazeAlgorithmType.Eller,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates an unbiased maze (Aldous-Broder Algorithm).
		/// Best for research or when uniform distribution is required.
		/// Note: Slower for large mazes.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateUnbiased(int width, int height, int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = MazeAlgorithmType.AldousBroder,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a braided maze with loops (removes dead ends).
		/// Provides multiple solution paths.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="braidingFactor">How many dead ends to remove (0.0 to 1.0).</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateBraided(int width, int height, double braidingFactor = 0.5, int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Type = MazeType.Braided,
				BraidingFactor = braidingFactor,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a random maze using a randomly selected algorithm.
		/// Good for variety in generated mazes.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateRandom(int width, int height, int? seed = null)
		{
			var random = seed.HasValue ? new Random(seed.Value) : new Random();
			var algorithms = new[]
			{
				MazeAlgorithmType.Eller,
				MazeAlgorithmType.RecursiveBacktracker,
				MazeAlgorithmType.Prim,
				MazeAlgorithmType.RecursiveDivision,
				MazeAlgorithmType.AldousBroder
			};

			var selectedAlgorithm = algorithms[random.Next(algorithms.Length)];

			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = selectedAlgorithm,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a maze optimized for the specified size.
		/// Automatically selects the best algorithm based on maze dimensions.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateOptimized(int width, int height, int? seed = null)
		{
			MazeAlgorithmType algorithm;
			int cellCount = width * height;

			if (cellCount < 100)
			{
				// Small mazes - any algorithm works, use Prim for variety
				algorithm = MazeAlgorithmType.Prim;
			}
			else if (cellCount < 10000)
			{
				// Medium mazes - use Recursive Backtracker for good characteristics
				algorithm = MazeAlgorithmType.RecursiveBacktracker;
			}
			else if (cellCount < 100000)
			{
				// Large mazes - use Eller for efficiency
				algorithm = MazeAlgorithmType.Eller;
			}
			else
			{
				// Very large mazes - use Eller for best memory usage
				algorithm = MazeAlgorithmType.Eller;
			}

			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Algorithm = algorithm,
				Seed = seed
			};
			return new Maze(config);
		}

		/// <summary>
		/// Creates a maze with rectangular rooms (open spaces).
		/// Creates dungeon-style layouts with chambers connected by corridors.
		/// </summary>
		/// <param name="width">The width of the maze.</param>
		/// <param name="height">The height of the maze.</param>
		/// <param name="roomCount">Number of rooms to add (default: 5).</param>
		/// <param name="minRoomSize">Minimum room size (default: 3).</param>
		/// <param name="maxRoomSize">Maximum room size (default: 7).</param>
		/// <param name="seed">Optional seed for reproducibility.</param>
		/// <returns>A generated maze.</returns>
		public static Maze CreateWithRooms(
			int width, 
			int height, 
			int roomCount = 5,
			int minRoomSize = 3,
			int maxRoomSize = 7,
			int? seed = null)
		{
			var config = new MazeConfiguration
			{
				Width = width,
				Height = height,
				Type = MazeType.WithRooms,
				RoomCount = roomCount,
				MinRoomSize = minRoomSize,
				MaxRoomSize = maxRoomSize,
				Seed = seed
			};
			return new Maze(config);
		}
	}
}
