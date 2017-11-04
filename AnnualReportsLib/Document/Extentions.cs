using System;
using System.Text;


namespace AnnualReports.Document
{
    public static class Extentions
    {
        public static String ChangeEncoding (this string input)
        {
			var w1250 = CodePagesEncodingProvider.Instance.GetEncoding("Windows-1250");

			var utf8 = Encoding.GetEncoding("UTF-8");


			byte[] bytes = new byte[input.Length * sizeof(char)];
			System.Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);

			//Convert it to 1252:
			//Encoding w1252 = Encoding.GetEncoding(1252);
			byte[] output = Encoding.Convert(utf8, w1250, bytes);
			//Get the string back:

            return w1250.GetString(output);
        }
    }
}
