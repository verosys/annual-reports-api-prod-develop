using System.IO;

namespace AnnualReports.Document
{
    public interface IExportable
    {
        Stream GetPdf();
        Stream GetWord();
    }
}