using System;
using System.Collections.Generic;

namespace MazeGenerator.Modifiers
{
	/// <summary>
	/// Modifier that adds rectangular rooms (open spaces) to mazes.
	/// Creates dungeon-like layouts with chambers connected by corridors.
	/// </summary>
	public class RoomModifier : IMazeModifier
	{
		public string Name => "Room Modifier";
		
		public string Description => "Adds rectangular open spaces (rooms) to the maze. Creates dungeon-style layouts with chambers connected by corridors.";
		
		private Random _random;
		private int _width;
		private int _height;
		
		public void Apply(List<List<Cell>> cells, MazeConfiguration config)
		{
			if (config.RoomCount <= 0)
				return; // No rooms to add
			
			_width = config.Width;
			_height = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value + 2000) : new Random();
			
			var rooms = new List<Room>();
			int attempts = 0;
			int maxAttempts = config.RoomCount * 50; // Try up to 50 times per desired room
			
			// Try to place rooms
			while (rooms.Count < config.RoomCount && attempts < maxAttempts)
			{
				attempts++;
				
				var room = GenerateRandomRoom(config);
				
				// Check if room overlaps with existing rooms
				if (!OverlapsWithAny(room, rooms))
				{
					rooms.Add(room);
					CarveRoom(cells, room);
				}
			}
		}
		
		private Room GenerateRandomRoom(MazeConfiguration config)
		{
			int roomWidth = _random.Next(config.MinRoomSize, config.MaxRoomSize + 1);
			int roomHeight = _random.Next(config.MinRoomSize, config.MaxRoomSize + 1);
			
			// Random position, ensuring room fits in maze with at least 1 cell border
			int x = _random.Next(1, _width - roomWidth - 1);
			int y = _random.Next(1, _height - roomHeight - 1);
			
			return new Room(x, y, roomWidth, roomHeight);
		}
		
		private bool OverlapsWithAny(Room room, List<Room> existingRooms)
		{
			foreach (var existing in existingRooms)
			{
				// Add 1-cell buffer between rooms
				if (room.X < existing.X + existing.Width + 1 &&
				    room.X + room.Width + 1 > existing.X &&
				    room.Y < existing.Y + existing.Height + 1 &&
				    room.Y + room.Height + 1 > existing.Y)
				{
					return true;
				}
			}
			return false;
		}
		
		private void CarveRoom(List<List<Cell>> cells, Room room)
		{
			// Remove all internal walls within the room
			for (int row = room.Y; row < room.Y + room.Height; row++)
			{
				for (int col = room.X; col < room.X + room.Width; col++)
				{
					var cell = cells[row][col];
					
					// Remove right wall if not on room's right edge
					if (col < room.X + room.Width - 1)
					{
						cell.Right = false;
						// Also remove left wall of neighbor
						if (col + 1 < _width)
							cells[row][col + 1].Left = false;
					}
					
					// Remove bottom wall if not on room's bottom edge
					if (row < room.Y + room.Height - 1)
					{
						cell.Bottom = false;
						// Also remove top wall of neighbor
						if (row + 1 < _height)
							cells[row + 1][col].Top = false;
					}
				}
			}
			
			// Ensure room has at least one connection to the maze
			EnsureRoomConnection(cells, room);
		}
		
		private void EnsureRoomConnection(List<List<Cell>> cells, Room room)
		{
			// Pick a random side to connect
			int side = _random.Next(4); // 0=top, 1=right, 2=bottom, 3=left
			
			switch (side)
			{
				case 0: // Top
					if (room.Y > 0)
					{
						int col = _random.Next(room.X, room.X + room.Width);
						cells[room.Y][col].Top = false;
						cells[room.Y - 1][col].Bottom = false;
					}
					break;
				
				case 1: // Right
					if (room.X + room.Width < _width)
					{
						int row = _random.Next(room.Y, room.Y + room.Height);
						cells[row][room.X + room.Width - 1].Right = false;
						cells[row][room.X + room.Width].Left = false;
					}
					break;
				
				case 2: // Bottom
					if (room.Y + room.Height < _height)
					{
						int col = _random.Next(room.X, room.X + room.Width);
						cells[room.Y + room.Height - 1][col].Bottom = false;
						cells[room.Y + room.Height][col].Top = false;
					}
					break;
				
				case 3: // Left
					if (room.X > 0)
					{
						int row = _random.Next(room.Y, room.Y + room.Height);
						cells[row][room.X].Left = false;
						cells[row][room.X - 1].Right = false;
					}
					break;
			}
		}
		
		private class Room
		{
			public int X { get; }
			public int Y { get; }
			public int Width { get; }
			public int Height { get; }
			
			public Room(int x, int y, int width, int height)
			{
				X = x;
				Y = y;
				Width = width;
				Height = height;
			}
		}
	}
}
