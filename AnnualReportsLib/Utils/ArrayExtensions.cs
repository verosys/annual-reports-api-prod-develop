using System;
namespace AnnualReports.Utils
{
    public static class ArrayExtensions
    {
        public static void SetRow (this string[,] array, int rowIndex, string[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                array[rowIndex, i] = row[i];
            }
        }
    }
}
