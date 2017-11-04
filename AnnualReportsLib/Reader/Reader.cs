using System;
using System.IO;
using ExcelDataReader;
using System.Data;
using System.Linq;
using System.Collections.Generic;


namespace AnnualReports.Reader
{
    public class Reader
    {

        public static void Init()
        {
            //This is required to parse strings in binary BIFF2-5 Excel documents encoded with DOS-era code pages. 
            //These encodings are registered by default in the full .NET Framework, but not on .NET Core.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public DataDocument Read(Stream stream)
        {
            int[] sheets = new int[] { 2, 3, 4 };

            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx)
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {

                DataDocument document = new DataDocument();
                var sheetIndex = -1;

                do
                {
                    sheetIndex++;

                    if (!sheets.Contains(sheetIndex))
                        continue;

                    if (sheetIndex == 2)
                    {
                        var sheet = new Sheet(78, 14);
                        sheet = ExtractSheetData(sheet, reader);
                        document.Sheets.Add("osnovnipodaci", sheet);
                    }

                    else if (sheetIndex == 3)
                    {
                        var sheet = new Sheet(134, 10);
                        sheet = ExtractSheetData(sheet, reader);
                        document.Sheets.Add("bilanca", sheet);

                    }
                    else if (sheetIndex == 4)
                    {
                        var sheet = new Sheet(106, 10);
                        sheet = ExtractSheetData(sheet, reader);
                        document.Sheets.Add("rdg", sheet);

                    }

                } while (reader.NextResult());


                return document;
            }
        }

        private Sheet ExtractSheetData(Sheet sheet, IExcelDataReader reader)
        {

            int rowIndex = 0;

            while (reader.Read())
            {
                if (rowIndex >= sheet.Rows - 1)
                    break;

                for (int i = 0; i < sheet.Columns; i++) //i<reader.FieldCount - ali ima onda hrpa praznih polja
                {
                    Cell cell = new Cell { Value = reader.GetValue(i) };

                    sheet.Table[rowIndex, i] = cell;

                    //if (cell.Value != null)
                    //    Console.WriteLine(String.Format("{0},{1}:{2}", rowIndex, i, cell.Value.ToString()));

                }
                rowIndex++;
            }

            return sheet;
        }
    }
}