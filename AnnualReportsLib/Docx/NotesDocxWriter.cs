using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Reports = AnnualReports.Document;

namespace AnnualReports.Docx
{
    public class NotesDocxWriter
    {
        private readonly Color darkColor = new Color() { Val = "363844" };
        private readonly Color lightColor = new Color() { Val = "8C8C8C" };
        private readonly Color accentColor = new Color() { Val = "4682B4" };

        private readonly FontSize titleSize = new FontSize() { Val = "28" };
        private readonly FontSize subtitleSize = new FontSize() { Val = "24" };
        private readonly FontSize sectionTitleSize = new FontSize() { Val = "22" };
        private readonly FontSize paragraphTitleSize = new FontSize() { Val = "22" };
        private readonly FontSize paragraphSubtitleSize = new FontSize() { Val = "22" };
        private readonly FontSize textSize = new FontSize() { Val = "22" };

        Reports.NotesDocument notesDocument;

        public NotesDocxWriter(Reports.NotesDocument notesDocument)
        {
            this.notesDocument = notesDocument;
        }

        public Stream Create(Stream stream)
        {
            stream.Position = 0;
            // Now open the copied file
            using (var wordDocument = WordprocessingDocument.Open(stream, true))
            {
                // MainDocumentPart, root Document and Body already exist just access them
                var mainPart = wordDocument.MainDocumentPart;
                var document = mainPart.Document;
                var body = document.Body;

                SectionProperties sectionProperties1 = new SectionProperties()
                {
                    RsidR = "00FC093D",
                    RsidSect = "00240B4C"
                };

                PageSize pageSize1 = new PageSize()
                {
                    Width = (UInt32Value)11906,
                    Height = (UInt32Value)16838,
                    Orient = PageOrientationValues.Portrait
                    //Code = (UInt16Value)9U
                };
                PageMargin pageMargin1 = new PageMargin()
                {
                    Top = 1440,
                    Right = (UInt32Value)1440,
                    Bottom = 1440,
                    Left = (UInt32Value)1440,
                    Header = (UInt32Value)0,
                    Footer = (UInt32Value)0,
                    Gutter = (UInt32Value)0U
                };
                //Columns columns1 = new Columns() { Space = "708" };
                //DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

                sectionProperties1.Append(pageSize1);
                sectionProperties1.Append(pageMargin1);

                mainPart.Document.Body.Append(sectionProperties1);


                if (notesDocument.Header != null)
                {
                    body.AppendChild(GetHeader(notesDocument.Header));
                }

                if (notesDocument.TitleI != null)
                {
                    var paragraph = new Paragraph()
                    {
                        ParagraphProperties = new ParagraphProperties()
                        {
                            Justification = new Justification() { Val = JustificationValues.Center }
                        }
                    };

                    var runI = paragraph.AppendChild(new Run());
                    runI.RunProperties = new RunProperties();
                    runI.RunProperties.FontSize = (FontSize)titleSize.Clone();
                    runI.RunProperties.Color = (Color)darkColor.Clone();
                    runI.RunProperties.Bold = new Bold();
                    runI.AppendChild(new Break());
                    runI.AppendChild(new Break());
                    runI.AppendChild(new Text(notesDocument.TitleI));
                    runI.AppendChild(new Break());

                    var runII = paragraph.AppendChild(new Run());
                    runII.RunProperties = new RunProperties();
                    runII.RunProperties.FontSize = (FontSize)subtitleSize.Clone();
                    runII.RunProperties.Color = (Color)darkColor.Clone();
                    runII.RunProperties.Bold = new Bold();
                    runII.AppendChild(new Text(notesDocument.TitleII));
                    runII.AppendChild(new Break());
                    runII.AppendChild(new Break());

                    body.AppendChild(paragraph);
                }

                foreach (var section in notesDocument.Sections)
                {
                    body.Append(new Paragraph(new Run(new Break())));

                    body.Append(GetSectionTitle(((Reports.Section)section).Title));

                    foreach (var entry in ((Reports.Section)section).Entries)
                    {
                        if (entry is Reports.Paragraph)
                        {
                            body.AppendChild(GetParagraph((Reports.Paragraph)entry));
                        }
                        else if (entry is Reports.Table)
                        {
                            body.AppendChild(TableHelper.GetTable((Reports.Table)entry));
                            body.Append(new Paragraph(new Run(new Break())));
                        }
                        else if (entry is Reports.PieChartEntry)
                        {
                            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (var image = ((Reports.PieChartEntry)entry).GetChart())
                                {
                                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    ms.Position = 0;
                                    imagePart.FeedData(ms);

                                    var height = 17 * image.Height / image.Width;

                                    var element = ImageHelper.GetImage(((Reports.PieChartEntry)entry).Title, 17 * 360000L, height * 360000L, mainPart.GetIdOfPart(imagePart));
                                    var paragraphProperties = new ParagraphProperties()
                                    {
                                        Justification = new Justification()
                                        {
                                            Val = JustificationValues.Center
                                        }
                                    };
                                    body.AppendChild(new Paragraph(new Run(element), paragraphProperties));
                                    body.Append(new Paragraph(new Run(new Break())));
                                }
                            }
                        }
                        else if (entry is Reports.BarChartEntry)
                        {
                            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                using (var image = ((Reports.BarChartEntry)entry).GetChart())
                                {
                                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    ms.Position = 0;
                                    imagePart.FeedData(ms);

                                    var height = 17 * image.Height / image.Width;

                                    var element = ImageHelper.GetImage(((Reports.BarChartEntry)entry).Title, 17 * 360000L, height * 360000L, mainPart.GetIdOfPart(imagePart));


                                    var paragraphProperties = new ParagraphProperties()
                                    {
                                        Justification = new Justification()
                                        {
                                            Val = JustificationValues.Center
                                        }
                                    };
                                    body.AppendChild(new Paragraph(new Run(element), paragraphProperties));
                                    body.Append(new Paragraph(new Run(new Break())));
                                }
                            }
                        }
                    }
                }

                body.AppendChild(TableHelper.GetFooterTable(notesDocument.Footer));

                document.Save();
            }

            stream.Position = 0;

            return stream;
        }

        private Paragraph GetHeader(Reports.Header header)
        {
            var paragraph = new Paragraph();
            var runTitle = paragraph.AppendChild(new Run());
            runTitle.RunProperties = new RunProperties();
            runTitle.RunProperties.FontSize = (FontSize)sectionTitleSize.Clone();
            runTitle.RunProperties.Color = (Color)accentColor.Clone();
            runTitle.RunProperties.Bold = new Bold();
            runTitle.AppendChild(new Text(header.LineI.ToUpper()));
            runTitle.AppendChild(new Break());

            var runSubtitle = paragraph.AppendChild(new Run());
            runSubtitle.RunProperties = new RunProperties();
            runSubtitle.RunProperties.FontSize = (FontSize)sectionTitleSize.Clone();
            runSubtitle.RunProperties.Color = (Color)lightColor.Clone();
            runSubtitle.AppendChild(new Text(header.LineII.ToUpper()));
            runSubtitle.AppendChild(new Break());

            return paragraph;
        }

        private Paragraph GetSectionTitle(string title)
        {
            var paragraph = new Paragraph();
            paragraph.ParagraphProperties = new ParagraphProperties();
            paragraph.ParagraphProperties.Justification = new Justification() { Val = JustificationValues.Left };

            var runTitle = paragraph.AppendChild(new Run());
            runTitle.RunProperties = new RunProperties();
            runTitle.RunProperties.FontSize = (FontSize)sectionTitleSize.Clone();
            runTitle.RunProperties.Color = (Color)accentColor.Clone();
            runTitle.RunProperties.Bold = new Bold();
            runTitle.AppendChild(new Text(title.ToUpper()));

            return paragraph;
        }

        private Paragraph GetParagraph(Reports.Paragraph dataParagraph)
        {
            var paragraph = new Paragraph();
            paragraph.ParagraphProperties = new ParagraphProperties();
            paragraph.ParagraphProperties.Justification = new Justification() { Val = JustificationValues.Left };
            paragraph.ParagraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
            {
                Line = new StringValue("360"),
                LineRule = LineSpacingRuleValues.Auto,
                Before = new StringValue("0"),
                After = new StringValue("0")
            };

            if (!String.IsNullOrEmpty(dataParagraph.Title))
            {
                var runTitle = paragraph.AppendChild(new Run());
                runTitle.RunProperties = new RunProperties();
                runTitle.RunProperties.FontSize = (FontSize)paragraphTitleSize.Clone();
                runTitle.RunProperties.Bold = new Bold();
                runTitle.RunProperties.Color = (Color)darkColor.Clone();
                runTitle.AppendChild(new Break());
                runTitle.AppendChild(new Text(dataParagraph.Title));
                runTitle.Append(new Break());
            }
            if (!String.IsNullOrEmpty(dataParagraph.Subtitle))
            {
                var runSubtitle = paragraph.AppendChild(new Run());
                runSubtitle.RunProperties = new RunProperties();
                runSubtitle.RunProperties.FontSize = (FontSize)paragraphSubtitleSize.Clone();
                runSubtitle.RunProperties.Italic = new Italic();
                runSubtitle.RunProperties.Color = (Color)lightColor.Clone();
                runSubtitle.AppendChild(new Text(dataParagraph.Subtitle));
                runSubtitle.AppendChild(new Break());
            }
            if (!String.IsNullOrEmpty(dataParagraph.Text))
            {
                var run = paragraph.AppendChild(new Run());
                run.RunProperties = new RunProperties();
                run.RunProperties.FontSize = (FontSize)textSize.Clone();
                run.RunProperties.Color = (Color)darkColor.Clone();
                //run.AppendChild(new Break());
                run.AppendChild(new Text(dataParagraph.Text));
                //run.AppendChild(new Break());
                //run.RunProperties.Spacing = new Spacing()
                //{
                //	Val = 360
                //};
            }

            return paragraph;
        }
    }
}