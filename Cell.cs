namespace MazeGenerator
{
	public class Cell
	{
		public bool Left { get; set; }
		public bool Right { get; set; }
		public bool Top { get; set; }
		public bool Bottom { get; set; }
		public int Set { get; set; }
		
		public Cell(int set)
		{
			Set = set;
		}

		public Cell(Cell srcCell)
		{
			if (srcCell.Bottom)
				Set = -1;
			else
				Set = srcCell.Set;
		}
	}
}