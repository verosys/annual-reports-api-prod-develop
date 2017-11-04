using System;
using Notes = AnnualReports.Document;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.rtf;
using System.IO;
using System.Linq;


namespace AnnualReports.Pdf
{
    public class NotesPdfWriter : PdfBase
    {
        Notes.NotesDocument notesDocument;

        Font titleFont;
        Font subtitleFont;

        Font headerFont;

        Font sectionTitleFont;

        Font paragraphtTitleFont;
        Font paragraphtSubtitleFont;
        Font paragraphTextFont;

        Font tableFont;
        Font tableHeaderFont;

        BaseColor darkColor = new BaseColor(54, 56, 68); //(50, 118, 180);
        BaseColor lightColor = new BaseColor(140, 140, 140);
        BaseColor accentColor = new BaseColor(70, 130, 180);//(65, 125, 215);

        public NotesPdfWriter(Notes.NotesDocument notesDocument)
        {

            this.notesDocument = notesDocument;

            titleFont = new Font(baseBoldFont)
            {
                Size = 16,
                Color = darkColor
            };

            subtitleFont = new Font(baseBoldFont)
            {
                Size = 12,
                Color = darkColor
            };

            headerFont = new Font(baseFont)
            {
                Size = 10,
                Color = lightColor
            };

            sectionTitleFont = new Font(baseBoldFont)
            {
                Size = 10,
                Color = accentColor
            };

            paragraphtTitleFont = new Font(baseBoldFont)
            {
                Size = 10,
                Color = darkColor
            };

            paragraphtSubtitleFont = new Font(baseItalicFont)
            {
                Size = 10,
                Color = lightColor
            };

            paragraphTextFont = new Font(baseFont)
            {
                Size = 10,
                Color = darkColor
            };


            tableFont = new Font(baseFont)
            {
                Size = 9,
                Color = darkColor
            };

            tableHeaderFont = new Font(baseBoldFont)
            {
                Size = 9,
                Color = accentColor
            };
        }

        public MemoryStream CreatePDF()
        {
            var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();

            if (notesDocument.Header != null)
            {
                document.Add(new Paragraph(notesDocument.Header.LineI.ToUpper(), sectionTitleFont));
                document.Add(new Paragraph(notesDocument.Header.LineII.ToUpper(), headerFont));
            }

            if (notesDocument.TitleI != null)
                document.Add(new Paragraph(notesDocument.TitleI, titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 30,
                    SpacingAfter = 0
                });

            if (notesDocument.TitleII != null)
                document.Add(new Paragraph(notesDocument.TitleII, subtitleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 30
                });


            foreach (var section in notesDocument.Sections)
            {
                document.Add(GetParagraph(((Notes.Section)section).Title, sectionTitleFont));


                foreach (var entry in ((Notes.Section)section).Entries)
                {
                    if (entry is Notes.Paragraph)
                    {
                        var title = ((Notes.Paragraph)entry).Title;
                        var text = ((Notes.Paragraph)entry).Text;
                        var subtitle = ((Notes.Paragraph)entry).Subtitle;

                        if (title != null)
                            document.Add(GetParagraph(title, paragraphtTitleFont));
                        if (subtitle != null)
                            document.Add(GetParagraph(subtitle, paragraphtSubtitleFont, Element.ALIGN_LEFT, 0));
                        if (text != null)
                            document.Add(GetParagraph(text, paragraphTextFont));
                    }
                    else if (entry is Notes.Table)
                    {
                        document.Add(GetTable((Notes.Table)entry));
                    }
                    else if (entry is Notes.PieChartEntry)
                    {
                        var chart = GetPieChart((Notes.PieChartEntry)entry);
                        if (chart != null)
                        {
                            chart.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin, document.PageSize.Height);
                            //chart.SetAbsolutePosition(0,0);
                            document.Add(chart);
                        }
                    }
                    else if (entry is Notes.BarChartEntry)
                    {
                        var chart = GetBarChart((Notes.BarChartEntry)entry);
                        if (chart != null)
                        {
                            chart.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin, document.PageSize.Height);
                            document.Add(chart);
                        }
                    }
                }
            }

            if (notesDocument.Footer != null)
            {
                document.Add(GetFooter(notesDocument.Footer));
            }

            document.Close();
            writer.Close();

            ms.Position = 0;
            return ms;
        }

        private PdfPTable GetTable(Notes.Table table)
        {
            PdfPTable ownerTable = new PdfPTable(table.Data.GetLength(1));
            ownerTable.HorizontalAlignment = Element.ALIGN_CENTER;
            ownerTable.SpacingBefore = 10;
            ownerTable.SetWidths(table.Widths);
            int rows = table.Data.GetLength(0);
            int columns = table.Data.GetLength(1);

            if (columns <= 2)
                ownerTable.WidthPercentage = 50f;
            else
                ownerTable.WidthPercentage = 100f;


            if (table.Header != null)
            {
                for (int i = 0; i < table.Header.Length; i++)
                {
                    int alignment = i == 0 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    PdfPCell cell = GetDataCell(table.Header[i], alignment, tableHeaderFont);
                    //cell.BackgroundColor = backgroundColor;
                    //cell.BorderColor = BaseColor.LightGray;
                    cell.BorderWidth = 0;
                    ownerTable.AddCell(cell);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {

                    int alignment = j == 0 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;

                    PdfPCell cell = GetDataCell(table.Data[i, j], alignment, tableFont);
                    ownerTable.AddCell(cell);

                }
            }
            return ownerTable;
        }

        private PdfPCell GetDataCell(String text, int alignment, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = alignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingTop = 6,
                PaddingBottom = 6,
                PaddingLeft = 6,
                PaddingRight = 6,
                BorderWidth = 0.5f,
                BorderColor = BaseColor.Gray,
                //FixedHeight = 20,

            };
            return cell;
        }

        private Paragraph GetParagraph(String text, Font font, int alignment = Element.ALIGN_LEFT, int spacingBefore = 10)
        {
            var paragraph = new Paragraph(text, font)
            {
                Alignment = alignment,
                SpacingBefore = spacingBefore,
            };
            paragraph.SetLeading(0.0f, 2.0f);
            return paragraph;
        }

        private Image GetPieChart(Notes.PieChartEntry pieChart)
        {
            if (pieChart.Entries.Count > 0)
            {
                var chart = pieChart.GetChart();

                //chart.Save(Directory.GetCurrentDirectory() + "/" + pieChart.Title + ".jpg");

                return Image.GetInstance(chart, BaseColor.White);
            }
            else
                return null;
        }

        private Image GetBarChart(Notes.BarChartEntry barChart)
        {
            if (barChart.Entries.Count > 0)
            {
                var chart = barChart.GetChart();

                //chart.Save(Directory.GetCurrentDirectory() + "/" + barChart.Title + ".jpg");

                return Image.GetInstance(chart, BaseColor.White);
            }
            else
                return null;
        }

        private PdfPTable GetFooter(Notes.Footer footer)
        {
            var table = new PdfPTable(2);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.SpacingBefore = 10;
            table.SetWidths(new float[] { 0.6f, 0.4f });
            table.WidthPercentage = 100.0f;
            table.DefaultCell.Padding = 6;
            table.DefaultCell.BorderWidth = 0;

            PdfPCell cell1 = new PdfPCell(new Phrase(footer.Left.Title, paragraphTextFont)) { BorderWidth = 0 };
            PdfPCell cell2 = new PdfPCell(new Phrase(footer.Right.Title, paragraphTextFont)) { BorderWidth = 0 };
            PdfPCell cell3 = new PdfPCell(new Phrase(footer.Left.Text.ToUpper(), paragraphTextFont)) { BorderWidth = 0 };
            PdfPCell cell4 = new PdfPCell(new Phrase(footer.Right.Text.ToUpper(), paragraphTextFont)) { BorderWidth = 0 };

            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);

            return table;
        }
    }
}