namespace AnnualReports.Charting
{
	public struct Padding
	{
		public int Top { get; set; }
		public int Bottom { get; set; }
		public int Left { get; set; }
		public int Right { get; set; }


        public Padding (int all)
        {
            Top = Bottom = Left = Right = all;
        }
	}
}