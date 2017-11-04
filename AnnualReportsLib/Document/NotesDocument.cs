using System;
using System.Collections.Generic;
using System.IO;
using AnnualReports.Model;
using AnnualReports.Pdf;
using AnnualReports.Docx;
using AnnualReports.Utils;

namespace AnnualReports.Document
{

    public class NotesDocument :IExportable
    {

        public GodisnjiIzvjestaj GodisnjiIzvjestaj { get; set; }

        public Header Header { get; set; }

        public string TitleI { get; set; }
        public string TitleII { get; set; }

        public List<Entry> Sections { get; set; }

        public Footer Footer { get; set; }

        public NotesDocument()
        {
			Sections = new List<Entry>();
            Header = new Header();
            Footer = new Footer();
        }

        public NotesDocument(GodisnjiIzvjestaj izvjestaj)
        {
            Sections = new List<Entry>();
            this.GodisnjiIzvjestaj = izvjestaj;
        }

        public Stream GetPdf()
        {
            var pdf = new NotesPdfWriter(this);
            return pdf.CreatePDF();
        }

        public Stream GetWord()
        {
            var templateStream = ResourceHelper.GetEmptyDocxStream();

            var docx = new NotesDocxWriter(this);
            return docx.Create(templateStream);
        }
	}
}