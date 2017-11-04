using System;
namespace AnnualReports.Pdf
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public abstract class PdfBase
    {
        //internal string FONT = System.IO.Directory.GetCurrentDirectory() + "/resources/fonts/FreeSerif.otf";
        //internal string BOLD_FONT = System.IO.Directory.GetCurrentDirectory() + "/resources/fonts/FreeSerifBold.otf";
        //internal string ITALIC_FONT = System.IO.Directory.GetCurrentDirectory() + "/resources/fonts/FreeSerifItalic.otf";

        internal Font baseFont;
        internal Font baseBoldFont;
        internal Font baseBoldItalicFont;
        internal Font baseItalicFont;

        //internal BaseFont baseFont;
        //internal BaseFont baseBoldFont;
        //internal BaseFont baseBoldItalicFont;


        internal PdfBase()
        {
            //baseFont = BaseFont.CreateFont(FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //baseBoldFont = BaseFont.CreateFont(BOLD_FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //baseBoldItalicFont = BaseFont.CreateFont(ITALIC_FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            //baseBoldFont = FontFactory.GetFont(BOLD_FONT, "Cp1250", true);
            //baseBoldItalicFont = FontFactory.GetFont(ITALIC_FONT, "Cp1250", true);

            baseFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);//(FONT, "Cp1250", true);
            baseBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            baseBoldItalicFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLDOBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            baseItalicFont = FontFactory.GetFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
        }

    }
}
