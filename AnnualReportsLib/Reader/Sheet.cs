using System.Collections.Generic;

namespace AnnualReports.Reader
{

    public class DataDocument
    {
        public Dictionary<string, Sheet> Sheets { get; set; }

        public DataDocument()
        {
            Sheets = new Dictionary<string, Sheet>();
        }
    }

    public class Sheet
    {

        public Cell[,] Table {get; set;}

        private int _rows;
        public int Rows { get { return _rows; } }
        private int _columns;
        public int Columns { get { return _columns; } }

        public Sheet()
        {
            Table = new Cell[124, 5];
        }

        public Sheet(int numberOfRows, int numberOfColumns)
		{
            this._rows = numberOfRows;
            this._columns = numberOfColumns;
            Table = new Cell[numberOfRows, numberOfColumns];
		}
    }

    public class Cell
    {
        public object Value { get; set; }
    }
}