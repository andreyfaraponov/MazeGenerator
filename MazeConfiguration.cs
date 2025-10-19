using System;
using MazeGenerator.Enums;

namespace MazeGenerator
{
	/// <summary>
	/// Configuration for maze generation.
	/// </summary>
	public class MazeConfiguration
	{
		/// <summary>
		/// Gets or sets the width of the maze (number of columns).
		/// </summary>
		public int Width { get; set; }
		
		/// <summary>
		/// Gets or sets the height of the maze (number of rows).
		/// </summary>
		public int Height { get; set; }
		
		/// <summary>
		/// Gets or sets the random seed for reproducible maze generation.
		/// If null, a random seed will be used.
		/// </summary>
		public int? Seed { get; set; }
		
		/// <summary>
		/// Gets or sets the algorithm to use for maze generation.
		/// Default is Eller's algorithm.
		/// </summary>
		public MazeAlgorithmType Algorithm { get; set; } = MazeAlgorithmType.Eller;
		
		/// <summary>
		/// Gets or sets the type of maze to generate.
		/// Default is Perfect maze.
		/// </summary>
		public MazeType Type { get; set; } = MazeType.Perfect;
		
		/// <summary>
		/// Gets or sets the braiding factor for braided mazes.
		/// Value between 0.0 (no braiding, perfect maze) and 1.0 (remove all dead ends).
		/// Only applies when Type is Braided.
		/// </summary>
		public double BraidingFactor { get; set; } = 0.5;
		
		/// <summary>
		/// Gets or sets the number of rooms to add to the maze.
		/// Only applies when Type is WithRooms.
		/// Default is 5.
		/// </summary>
		public int RoomCount { get; set; } = 5;
		
		/// <summary>
		/// Gets or sets the minimum room size (width and height).
		/// Only applies when Type is WithRooms.
		/// Default is 3.
		/// </summary>
		public int MinRoomSize { get; set; } = 3;
		
		/// <summary>
		/// Gets or sets the maximum room size (width and height).
		/// Only applies when Type is WithRooms.
		/// Default is 7.
		/// </summary>
		public int MaxRoomSize { get; set; } = 7;
		
		/// <summary>
		/// Validates the configuration and throws an exception if invalid.
		/// </summary>
		public void Validate()
		{
			if (Width <= 0)
				throw new ArgumentException("Width must be greater than 0.", nameof(Width));
			
			if (Height <= 0)
				throw new ArgumentException("Height must be greater than 0.", nameof(Height));
			
			if (Width > 1000)
				throw new ArgumentException("Width cannot exceed 1000.", nameof(Width));
			
			if (Height > 1000)
				throw new ArgumentException("Height cannot exceed 1000.", nameof(Height));
			
			if (BraidingFactor < 0.0 || BraidingFactor > 1.0)
				throw new ArgumentException("BraidingFactor must be between 0.0 and 1.0.", nameof(BraidingFactor));
			
			if (RoomCount < 0)
				throw new ArgumentException("RoomCount must be non-negative.", nameof(RoomCount));
			
			if (MinRoomSize < 2)
				throw new ArgumentException("MinRoomSize must be at least 2.", nameof(MinRoomSize));
			
			if (MaxRoomSize < MinRoomSize)
				throw new ArgumentException("MaxRoomSize must be greater than or equal to MinRoomSize.", nameof(MaxRoomSize));
		}
		
		/// <summary>
		/// Creates a default configuration for a maze of the specified size.
		/// </summary>
		public static MazeConfiguration Default(int width, int height)
		{
			return new MazeConfiguration
			{
				Width = width,
				Height = height
			};
		}
	}
}
