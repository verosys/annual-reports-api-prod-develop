using System;
using System.IO;
using AnnualReports.Document;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace AnnualReports.Pdf
{
    public class StatementPdfWriter : PdfBase
    {
        StatementDocument dataDocument;

        Font titleFont;
        Font boldFont;
        Font normalFont;

        BaseColor darkColor = new BaseColor(54, 56, 68); //(50, 118, 180);
        BaseColor lightColor = new BaseColor(140, 140, 140);
        BaseColor accentColor = new BaseColor(70, 130, 180);//(65, 125, 215);

        public StatementPdfWriter(StatementDocument dataDocument)
        {
            this.dataDocument = dataDocument;

            normalFont = new Font(baseFont)
            {
                Size = 10,
                Color = darkColor
            };
            titleFont = new Font(baseBoldFont)
            {
                Size = 14,
                Color = accentColor
            };
            boldFont = new Font(baseBoldFont)
            {
                Size = 10,
                Color = darkColor
            };
        }

        public MemoryStream CreatePdf()
        {
            var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();

            if (dataDocument.Header != null)
            {
                document.Add(GetParagraph(dataDocument.Header, normalFont, Element.ALIGN_LEFT));
            }

            if (dataDocument.TitleI != null)
            {
                document.Add(GetParagraph(dataDocument.TitleI, titleFont, Element.ALIGN_CENTER));
                document.Add(GetParagraph(dataDocument.TitleII, titleFont, Element.ALIGN_CENTER, 0));
            }

            foreach (var par in dataDocument.Paragraphs)
            {
                if (!String.IsNullOrEmpty(par.Title))
                    document.Add(GetParagraph(par.Title, boldFont, Element.ALIGN_CENTER, 20));
                if (!String.IsNullOrEmpty(par.Text))
                    document.Add(GetParagraph(par.Text, normalFont, Element.ALIGN_LEFT));
            }

            if (dataDocument.FooterI != null)
            {
                document.Add(GetParagraph(dataDocument.FooterI, normalFont, Element.ALIGN_RIGHT, 30));
                document.Add(GetParagraph(dataDocument.FooterII, normalFont, Element.ALIGN_RIGHT));
            }

            document.Close();
            writer.Close();

            ms.Position = 0;
            return ms;
        }

        private iTextSharp.text.Paragraph GetParagraph(String text, Font font, int alignment = Element.ALIGN_LEFT, int spacingBefore = 10)
        {
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(text, font)
            {
                Alignment = alignment,
                SpacingBefore = spacingBefore
            };
            return paragraph;
        }
    }
}