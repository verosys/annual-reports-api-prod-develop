using System;
using System.Text;
using System.IO;

namespace AnnualReports.Utils
{
	public class Logger
	{
		StringBuilder logg = new StringBuilder();

		public void Append(String s)
		{
			logg.AppendLine(s);
		}

		public void Export(String fileName)
		{
            File.WriteAllText(Directory.GetCurrentDirectory() + "/logs/"+ fileName, logg.ToString());
		}

		public void Clear()
		{
			logg.Clear();
		}
	}
}
