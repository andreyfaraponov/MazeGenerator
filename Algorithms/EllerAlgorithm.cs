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

		public string Description =>
			"Generates perfect mazes row by row. Memory efficient and suitable for large mazes. Creates mazes with a good balance of characteristics.";


		private int _columnsCount;
		private int _rowsCount;
		private Random _random;
		private List<List<Cell>> _cells;


		public void Generate(List<List<Cell>> cells, MazeConfiguration config)
		{
			_cells = cells;
			_columnsCount = config.Width;
			_rowsCount = config.Height;
			_random = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();

			InitializeCells(cells);

			for (int i = 0; i < _rowsCount; i++)
				CreateLine(cells, i);
		}

		private void InitializeCells(List<List<Cell>> cells)
		{
			for (int y = 0; y < _rowsCount; y++)
			{
				for (int x = 0; x < _columnsCount; x++)
				{
					Console.WriteLine($"{y} {x}");
					var cell = cells[y][x];
					cell.Left = false;
					cell.Right = false;
					cell.Top = false;
					cell.Bottom = false;
					cell.Set = 0;
				}
			}

			// Set outer walls
			for (int y = 0; y < _rowsCount; y++)
			{
				cells[y][0].Left = true;
				cells[y][_columnsCount - 1].Right = true;
			}

			for (int x = 0; x < _columnsCount; x++)
			{
				cells[0][x].Top = true;
				cells[_rowsCount - 1][x].Bottom = true;
			}
		}

		private void CreateLine(List<List<Cell>> cells, int rowIndex)
		{
			if (rowIndex == 0)
			{
				FillFirstLine(cells[rowIndex]);
			}
			else
			{
				FillCommonLine(cells[rowIndex], row: rowIndex);
			}

			if (rowIndex == _rowsCount - 1)
			{
				FillLastLine(cells[rowIndex]);
			}
		}

		private void FillLastLine(List<Cell> line)
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

		private void FillCommonLine(List<Cell> line, int row)
		{
			List<Cell> tmp = CopyAndPrepareLine(_cells[row - 1]);

			for (int i = 0; i < line.Count; i++) 
				line[i] = tmp[i];
			
			AddVerticalWalls(line);
			AddHorizontalWalls(line);
		}

		private void FillFirstLine(List<Cell> line)
		{
			for (int i = 0; i < _columnsCount; i++)
			{
				var cell = new Cell(i);

				if (i == 0)
					cell.Left = true;
				else if (i == _columnsCount - 1)
					cell.Right = true;

				cell.Top = true;

				line.Add(cell);
			}

			AddVerticalWalls(line);
			AddHorizontalWalls(line);
		}

		private void AddHorizontalWalls(List<Cell> line)
		{
			for (int i = 0; i < line.Count; i++)
			{
				if (_random.Next(0, 2) > 0 &&
					CanAddBottomToCellOfSet(line[i]))
				{
					line[i].Bottom = true;
				}
			}

			bool CanAddBottomToCellOfSet(Cell cell) =>
				!line.Where(c => c.Set == cell.Set && c != cell).All(c => c.Bottom);
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

		private List<Cell> CopyAndPrepareLine(List<Cell> from)
		{
			List<Cell> line = new List<Cell>(_columnsCount);

			foreach (var prevCell in from)
			{
				var cell = new Cell(prevCell);
				line.Add(cell);
			}

			for (int i = 0; i < line.Count; i++)
			{
				if (line[i].Set == -1)
					line[i].Set = GetEmptySetNumber(line);

				if (i == 0)
					line[i].Left = true;

				if (i == _columnsCount - 1)
					line[i].Right = true;
			}

			return line;
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

		private void UpdateSetForLine(int oldSet,
			int newSet,
			List<Cell> line)
		{
			foreach (var cell in line)
			{
				if (cell.Set == oldSet)
					cell.Set = newSet;
			}
		}
	}
}