using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Reports = AnnualReports.Document;

namespace AnnualReports.Docx
{
    public class TableHelper
    {
        public TableHelper()
        {
        }

        public static Table GetTable(Reports.Table dataTable)
        {
            var tableWidth = dataTable.Data.GetLength(1) > 2 ? "5000" : "3000";

            Table table = new Table();

            TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };
            table.AppendChild(tableStyle);

            var borderColor = "909090";

            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 2, Color = new StringValue(borderColor) },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 2, Color = new StringValue(borderColor) },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 2, Color = new StringValue(borderColor) },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 2, Color = new StringValue(borderColor) },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 2, Color = new StringValue(borderColor) },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 2, Color = new StringValue(borderColor) }
                ),
                new TableCellMargin()
                {
                    TopMargin = new TopMargin() { Width = new StringValue("100") },
                    BottomMargin = new BottomMargin() { Width = new StringValue("100") },
                    LeftMargin = new LeftMargin() { Width = new StringValue("200") },
                    RightMargin = new RightMargin() { Width = new StringValue("200") }
                },
                new TableJustification()
                {
                    Val = TableRowAlignmentValues.Center
                },
                new TableWidth() { Type = TableWidthUnitValues.Pct, Width = new StringValue(tableWidth) }
            );

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(tblProp);

            //table.AppendChild(grid);

            var cellProperties = new TableCellProperties()
            {
                TableCellWidth = new TableCellWidth() { Width = new StringValue("1250"), Type = TableWidthUnitValues.Pct },
                TableCellMargin = new TableCellMargin()
                {
                    BottomMargin = new BottomMargin() { Width = new StringValue("100"), Type = TableWidthUnitValues.Dxa },
                    TopMargin = new TopMargin() { Width = new StringValue("100"), Type = TableWidthUnitValues.Dxa },
                    LeftMargin = new LeftMargin() { Width = new StringValue("100"), Type = TableWidthUnitValues.Dxa },
                    RightMargin = new RightMargin() { Width = new StringValue("100"), Type = TableWidthUnitValues.Dxa }
                },
                TableCellVerticalAlignment = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            };

            if (dataTable.Header != null)
            {

                TableRow tr = new TableRow();

                for (int i = 0; i < dataTable.Header.Length; i++)
                {
                    var title = dataTable.Header[i];
                    TableCell tc1 = new TableCell();

                    var justification = ((i > 0) ? JustificationValues.Right : JustificationValues.Left);

                    var paragraph = new Paragraph()
                    {
                        ParagraphProperties = new ParagraphProperties()
                        {
                            Justification = new Justification() { Val = justification },
                            ContextualSpacing = new ContextualSpacing() { Val = false },
                        }
                    };

                    var run = paragraph.AppendChild(new Run());
                    run.AppendChild(new Text(title));
                    run.RunProperties = new RunProperties();
                    run.RunProperties.AppendChild(new Bold());
                    run.RunProperties.AppendChild(new Color() { Val = "4682B4" });

                    var properties = (TableCellProperties)cellProperties.Clone();
                    properties.TableCellWidth = new TableCellWidth() { Width = getWidthPercentage(i).ToString(), Type = TableWidthUnitValues.Pct };
                    tc1.AppendChild<TableCellProperties>(properties);
                    tc1.Append(paragraph);

                    tr.Append(tc1);
                }

                table.Append(tr);
            }

            int rows = dataTable.Data.GetLength(0);
            int columns = dataTable.Data.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                TableRow tr = new TableRow();
                for (int j = 0; j < columns; j++)
                {
                    TableCell tc1 = new TableCell();
                    var properties = (TableCellProperties)cellProperties.Clone();
                    properties.TableCellWidth = new TableCellWidth() { Width = getWidthPercentage(j).ToString(), Type = TableWidthUnitValues.Pct };
                    tc1.AppendChild<TableCellProperties>(properties);

                    var justification = ((j > 0) ? JustificationValues.Right : JustificationValues.Left);

                    var paragraph = new Paragraph()
                    {
                        ParagraphProperties = new ParagraphProperties()
                        {
                            Justification = new Justification() { Val = justification }
                        }
                    };

                    var run = paragraph.AppendChild(new Run());
                    run.RunProperties = new RunProperties();
                    run.RunProperties.Color = new Color() { Val = "363844" };
                    run.AppendChild(new Text(dataTable.Data[i, j]));
                    tc1.Append(paragraph);

                    tr.Append(tc1);
                }
                table.Append(tr);
            }
            return table;
        }

        private static int getWidthPercentage(int column)
        {
            switch (column)
            {
                case 0:
                    return 1750;
                case 1:
                    return 1250;
                case 2:
                    return 1250;
                case 4:
                    return 750;
            }

            return 5000;
        }


        public static Table GetFooterTable(Reports.Footer footer)
        {
            Table table = new Table();
            TableStyle tableStyle = new TableStyle() { Val = "TableGrid" };
            table.AppendChild(tableStyle);

            TableProperties tblProp = new TableProperties(
                new TableBorders(
                        new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                        new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                        new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                        new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) },
                        new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.None) }
                ),
                //new TableCellMargin()
                //{
                //    TopMargin = new TopMargin() { Width = new StringValue("400") },
                //    BottomMargin = new BottomMargin() { Width = new StringValue("400") },
                //    LeftMargin = new LeftMargin() { Width = new StringValue("400") },
                //    RightMargin = new RightMargin() { Width = new StringValue("400") }
                //},
                new TableJustification()
                {
                    Val = TableRowAlignmentValues.Left
                },
                new TableWidth() { Type = TableWidthUnitValues.Pct, Width = new StringValue("5000") }
            );

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(tblProp);

            TableGrid grid = new TableGrid(new GridColumn()
            {
                Width = new StringValue("1500")
            }, new GridColumn()
            {
                Width = new StringValue("1500")
            });

            table.AppendChild<TableGrid>(grid);


            var cellProperties = new TableCellProperties()
            {
                TableCellWidth = new TableCellWidth() { Width = new StringValue("2500"), Type = TableWidthUnitValues.Pct }
            };

            TableRow tr1 = new TableRow();
            TableCell tc1 = new TableCell(new Paragraph(new Run(new Text(footer.Left.Title))));
            tc1.AppendChild<TableCellProperties>((TableCellProperties)cellProperties.Clone());
            tr1.Append(tc1);

            TableCell tc2 = new TableCell(new Paragraph(new Run(new Text(footer.Right.Title))));
            tc2.AppendChild<TableCellProperties>((TableCellProperties)cellProperties.Clone());
            tr1.Append(tc2);

            table.Append(tr1);

            TableRow tr2 = new TableRow();

            TableCell tc3 = new TableCell(new Paragraph(new Run(new Text(footer.Left.Text))));
            tc3.AppendChild<TableCellProperties>((TableCellProperties)cellProperties.Clone());
            tr2.Append(tc3);

            TableCell tc4 = new TableCell(new Paragraph(new Run(new Text(footer.Right.Text))));
            tc4.AppendChild<TableCellProperties>((TableCellProperties)cellProperties.Clone());
            tr2.Append(tc4);

            table.Append(tr2);

            return table;
        }

    }
}