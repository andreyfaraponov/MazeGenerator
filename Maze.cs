using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
	public class Maze
	{
		private readonly int _columnsCount;
		private readonly int _rowsCount;
		private readonly Random _random;

		public IReadOnlyList<IReadOnlyList<Cell>> Cells => _mazeCells;

		private readonly List<List<Cell>> _mazeCells;

		public Maze(int columnsCount, int rowsCount)
		{
			_columnsCount = columnsCount;
			_rowsCount = rowsCount;
			_random = new Random();

			_mazeCells = new List<List<Cell>>(rowsCount);

			for (int i = 0; i < _rowsCount; i++)
				_mazeCells.Add(CreateLine(i));
		}

		private List<Cell> CreateLine(int rowIndex)
		{
			List<Cell> resultLine = rowIndex == 0
				? CreateFirstLine()
				: CreateCommonLine(row: rowIndex);

			if (rowIndex == _rowsCount - 1)
				PolishLastLine(resultLine);

			return resultLine;
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

		private List<Cell> CreateCommonLine(int row)
		{
			List<Cell> line = CopyAndPrepareLine(_mazeCells[row - 1]);
			AddVerticalWalls(line);
			AddHorizontalWalls(line);

			return line;
		}

		private List<Cell> CreateFirstLine()
		{
			List<Cell> line = new List<Cell>(_columnsCount);

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

			return line;
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