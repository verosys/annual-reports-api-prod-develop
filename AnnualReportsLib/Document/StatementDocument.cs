using System;
using System.IO;
using System.Collections.Generic;
using AnnualReports.Pdf;
using AnnualReports.Utils;

namespace AnnualReports.Document
{
    public class StatementDocument : IExportable
    {
        public string Header { get; set; }

        public string TitleI { get; set; }
        public string TitleII { get; set; }

        public List<Paragraph> Paragraphs { get; set; }

        public string FooterI { get; set; }
        public string FooterII { get; set; }

        public StatementDocument()
        {
            Paragraphs = new List<Paragraph>();
        }

        public Stream GetPdf()
        {
            var pdf = new StatementPdfWriter(this);
            return pdf.CreatePdf();
        }

        public Stream GetWord()
        {
            var templateStream = ResourceHelper.GetEmptyDocxStream();
            return new Docx.StatementDocxWritter(this).Create(templateStream);
        }
    }
}