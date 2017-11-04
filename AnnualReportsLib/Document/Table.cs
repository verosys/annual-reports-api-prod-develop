using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace AnnualReports.Document
{
    public class Table : Entry
    {
        public string[] Header { get; set; }

        public string[,] Data { get; set; }

        public int[] Widths { get; set; } //this is a bit dirty, because it's for pdf's widths

        public Table(XElement element)
        {
            var numberOfColumns = int.Parse(element.Attribute("Columns").Value);
            var rows = element.Descendants("Row");

            Data = new string[rows.Count(), numberOfColumns];

            Widths = element.Attribute("Widths").Value.Split(new char[] { ',' }).Select(int.Parse).ToArray(); ;

            for (int i = 0; i < rows.Count(); i++)
            {
                var row = rows.ElementAt(i);

                var entries = row.Descendants("Entry");
                for (int j = 0; j < entries.Count(); j++)
                {
                    Data[i, j] = entries.ElementAt(j).Value;
                }
            }
        }

        public Table()
        {
            Widths = new int[4] { 2, 1, 1, 1 };
        }

        public static string[] GetCommonHeader(int godina)
        {
            return new string[] { "Naziv", (godina - 1).ToString(), godina.ToString(), "Index " + godina + "/" + (godina - 1) };
        }
    }
}