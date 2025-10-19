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
				
				if (i == 0)
					cell.Left = true;
				else if (i == _columnsCount - 1)
					cell.Right = true;
				
				cell.Top = true;
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
				
				if (i == 0)
					cell.Left = true;
				
				if (i == _columnsCount - 1)
					cell.Right = true;
			}
		}
		
		private void AddVerticalWalls(List<Cell> line)
		{
			for (int i = 0; i < line.Count - 1; i++)
			{
				var cell = line[i];
				var nextCell = line[i + 1];
				
				if (cell.Set == nextCell.Set || _random.Next(0, 2) > 0)
					cell.Right = true;
				else
					UpdateSetForLine(nextCell.Set, cell.Set, line);
			}
		}
		
		private void AddHorizontalWalls(List<Cell> line)
		{
			for (int i = 0; i < line.Count; i++)
			{
				if (_random.Next(0, 2) > 0 && CanAddBottomToCellOfSet(line[i], line))
				{
					line[i].Bottom = true;
				}
			}
		}
		
		private bool CanAddBottomToCellOfSet(Cell cell, List<Cell> line)
		{
			return !line.Where(c => c.Set == cell.Set && c != cell).All(c => c.Bottom);
		}
		
		private void PolishLastLine(List<Cell> line)
		{
			for (int i = 0; i < _columnsCount - 1; i++)
			{
				var cell = line[i];
				var nextCell = line[i + 1];
				
				if (cell.Set != nextCell.Set)
					cell.Right = false;
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
