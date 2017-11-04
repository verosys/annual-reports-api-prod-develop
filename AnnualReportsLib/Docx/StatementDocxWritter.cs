using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Reports = AnnualReports.Document;

namespace AnnualReports.Docx
{
    public class StatementDocxWritter
    {
        Reports.StatementDocument statementDocument;

        private readonly Color darkColor = new Color() { Val = "363844" };
        private readonly Color lightColor = new Color() { Val = "8C8C8C" };
        private readonly Color accentColor = new Color() { Val = "4682B4" };

        private readonly FontSize titleSize = new FontSize() { Val = "28" };
        private readonly FontSize subtitleSize = new FontSize() { Val = "24" };
        private readonly FontSize textSize = new FontSize() { Val = "22" };

        public StatementDocxWritter(Reports.StatementDocument document)
        {
            this.statementDocument = document;
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

                sectionProperties1.Append(pageSize1);
                sectionProperties1.Append(pageMargin1);

                mainPart.Document.Body.Append(sectionProperties1);

                if (statementDocument.Header != null)
                {
                    body.AppendChild(GetHeader(statementDocument.Header));
                }
                if (statementDocument.TitleI != null)
                {
                    body.AppendChild(new Paragraph(new Run(new Break())));
                    body.AppendChild(GetTitle(statementDocument.TitleI, titleSize));
                    body.AppendChild(GetTitle(statementDocument.TitleII, subtitleSize));
                }

                foreach (var paragraph in statementDocument.Paragraphs)
                {
                    if (!String.IsNullOrEmpty(paragraph.Title))
                    {
                        body.AppendChild(new Paragraph(new Run(new Break())));
                        body.AppendChild(GetParagraphTitle(paragraph.Title));
                        body.AppendChild(new Paragraph(new Run(new Break())));
                    }
                    body.AppendChild(GetParagraphText(paragraph.Text));

                }

                if (statementDocument.FooterI != null)
                {
                    body.AppendChild(new Paragraph(new Run(new Break())));
                    body.AppendChild(GetFooter(statementDocument.FooterI));
                    body.AppendChild(GetFooter(statementDocument.FooterII));
                }

                document.Save();
            }

            stream.Position = 0;

            return stream;
        }

        private Paragraph GetHeader(string header)
        {
            var paragraph = new Paragraph();
            var run = paragraph.AppendChild(new Run());
            run.RunProperties = new RunProperties();
            run.RunProperties.FontSize = (FontSize)textSize.Clone();
            run.RunProperties.Color = (Color)darkColor.Clone();
            run.AppendChild(new Text(header));

            return paragraph;
        }

        private Paragraph GetTitle (string text, FontSize size)
        {
            var paragraph = new Paragraph()
            {
                ParagraphProperties = new ParagraphProperties()
                {
                    Justification = new Justification() { Val = JustificationValues.Center }
                }
            };

            var run = paragraph.AppendChild(new Run());
            run.RunProperties = new RunProperties();
            run.RunProperties.FontSize = (FontSize) size.Clone();
            run.RunProperties.Color = (Color)accentColor.Clone();
            run.RunProperties.Bold = new Bold();
            run.AppendChild(new Text(text));

            return paragraph;
        }

        private Paragraph GetFooter(string footer)
        {
            var paragraph = new Paragraph()
            {
                ParagraphProperties = new ParagraphProperties()
                {
                    Justification = new Justification() { Val = JustificationValues.Right }
                }
            };
            var runTitle = paragraph.AppendChild(new Run());
            runTitle.RunProperties = new RunProperties();
            runTitle.RunProperties.FontSize = (FontSize)textSize.Clone();
            runTitle.RunProperties.Color = (Color)darkColor.Clone();
            runTitle.AppendChild(new Text(footer));

            return paragraph;
        }

        private Paragraph GetParagraphTitle (string text)
        {
            var paragraph = new Paragraph();
            paragraph.ParagraphProperties = new ParagraphProperties();
            paragraph.ParagraphProperties.Justification = new Justification() { Val = JustificationValues.Center };

            var run = paragraph.AppendChild(new Run());
            run.RunProperties = new RunProperties();
            run.RunProperties.FontSize = (FontSize)textSize.Clone();
            run.RunProperties.Bold = new Bold();
            run.AppendChild(new Text(text));

            return paragraph;
        }

        private Paragraph GetParagraphText(string text)
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

            var run = paragraph.AppendChild(new Run());
            run.RunProperties = new RunProperties();
            run.RunProperties.FontSize = (FontSize)textSize.Clone();
            run.RunProperties.Color = (Color)darkColor.Clone();
            run.AppendChild(new Text(text));

            return paragraph;
        }
    }
}