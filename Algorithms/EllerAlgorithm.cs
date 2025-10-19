using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Algorithms
{
	/// <summary>
	/// Eller's algorithm for maze generation.
	/// Generates perfect mazes row by row with minimal memory usage.
	/// </summary>
	public class EllerAlgorithm : IMazeAlgorithm
	{
		public string Name => "Eller's Algorithm";
		
		public string Description => "Generates perfect mazes row by row. Memory efficient and suitable for large mazes. Creates mazes with a good balance of characteristics.";
		
		private Random _random;
		private int _columnsCount;
		private int _rowsCount;
		
		public void Generate(List<List<Cell>> cells, MazeConfiguration config)
		{
			_columnsCount = config.Width;
			_rowsCount = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();
			
			for (int rowIndex = 0; rowIndex < _rowsCount; rowIndex++)
			{
				var line = cells[rowIndex];
				
				if (rowIndex == 0)
				{
					InitializeFirstLine(line);
				}
				else
				{
					PrepareLineFromPrevious(line, cells[rowIndex - 1]);
				}
				
				AddVerticalWalls(line);
				AddHorizontalWalls(line);
				
				if (rowIndex == _rowsCount - 1)
				{
					PolishLastLine(line);
				}
			}
		}
		
		private void InitializeFirstLine(List<Cell> line)
		{
			for (int i = 0; i < _columnsCount; i++)
			{
				var cell = line[i];
				cell.Set = i;
				
				// Initialize all walls to true
				cell.Left = true;
				cell.Right = true;
				cell.Top = true;
				cell.Bottom = true;
			}
		}
		
		private void PrepareLineFromPrevious(List<Cell> line, List<Cell> previousLine)
		{
			for (int i = 0; i < _columnsCount; i++)
			{
				var cell = line[i];
				var prevCell = previousLine[i];
				
				if (prevCell.Bottom)
					cell.Set = -1;
				else
					cell.Set = prevCell.Set;
				
				if (cell.Set == -1)
					cell.Set = GetEmptySetNumber(line);
				
				// Initialize all walls to true
				cell.Left = true;
				cell.Right = true;
				cell.Top = prevCell.Bottom;  // Synchronize with previous row's bottom wall
				cell.Bottom = true;
			}
		}
		
		private void AddVerticalWalls(List<Cell> line)
		{
			for (int i = 0; i < line.Count - 1; i++)
			{
				var cell = line[i];
				var nextCell = line[i + 1];
				
				if (cell.Set == nextCell.Set || _random.Next(0, 2) > 0)
				{
					cell.Right = true;
					nextCell.Left = true;
				}
				else
				{
					cell.Right = false;
					nextCell.Left = false;
					UpdateSetForLine(nextCell.Set, cell.Set, line);
				}
			}
		}
		
		private void AddHorizontalWalls(List<Cell> line)
		{
			// First pass: randomly decide to add bottom walls
			for (int i = 0; i < line.Count; i++)
			{
				if (_random.Next(0, 2) > 0)
				{
					line[i].Bottom = true;  // Keep the wall
				}
				else
				{
					line[i].Bottom = false;  // Remove the wall (create passage)
				}
			}
			
			// Second pass: ensure each set has at least one opening down
			var sets = line.GroupBy(c => c.Set);
			foreach (var set in sets)
			{
				// If all cells in this set have bottom walls, remove one randomly
				if (set.All(c => c.Bottom))
				{
					var cellsInSet = set.ToList();
					var randomCell = cellsInSet[_random.Next(cellsInSet.Count)];
					randomCell.Bottom = false;
				}
			}
		}
		
		private void PolishLastLine(List<Cell> line)
		{
			for (int i = 0; i < _columnsCount - 1; i++)
			{
				var cell = line[i];
				var nextCell = line[i + 1];
				
				if (cell.Set != nextCell.Set)
				{
					cell.Right = false;
					nextCell.Left = false;
				}
			}
			
			int set = line[0].Set;
			
			foreach (var cell in line)
			{
				cell.Set = set;
				cell.Bottom = true;
			}
		}
		
		private int GetEmptySetNumber(List<Cell> line)
		{
			int setNumber = 0;
			
			while (setNumber < _columnsCount)
			{
				if (line.Any(c => c.Set == setNumber))
					setNumber++;
				else
					break;
			}
			
			return setNumber;
		}
		
		private void UpdateSetForLine(int oldSet, int newSet, List<Cell> line)
		{
			foreach (var cell in line)
			{
				if (cell.Set == oldSet)
					cell.Set = newSet;
			}
		}
	}
}
