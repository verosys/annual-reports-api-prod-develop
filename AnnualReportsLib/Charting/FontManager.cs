using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using AnnualReports.Utils;

namespace AnnualReports.Charting
{
    public static class FontManager
    {
        //private static Font GetFont(int size, FontStyle style)
        //{
        //    string resourceId;
        //    if (style == FontStyle.Bold)
        //        resourceId = "AnnualReportsLib.Charting.fonts.FreeSerifBold.otf";
        //        //resourceId = "AnnualReportsTest.AnnualReportsLib.Charting.fonts.FreeSerifBold.otf";
            
        //    else
        //        resourceId = "AnnualReportsLib.Charting.fonts.FreeSerif.otf";
        //        //resourceId = "AnnualReportsTest.AnnualReportsLib.Charting.fonts.FreeSerif.otf";

        //    System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();

        //    var assembly = typeof(FontManager).GetTypeInfo().Assembly;

        //    //string[] resources = assembly.GetManifestResourceNames();

        //    //Console.WriteLine(resources[0]);
        //    //Console.WriteLine(resources[1]);

        //    Stream fontStream = assembly.GetManifestResourceStream(resourceId);

        //    System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);

        //    // create a buffer to read in to
        //    byte[] fontdata = new byte[fontStream.Length];

        //    // read the font data from the resource
        //    fontStream.Read(fontdata, 0, (int)fontStream.Length);

        //    // copy the bytes to the unsafe memory block
        //    Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);

        //    // pass the font to the font collection
        //    privateFonts.AddMemoryFont(data, (int)fontStream.Length);

        //    // close the resource stream
        //    fontStream.Close();

        //    // free up the unsafe memory
        //    Marshal.FreeCoTaskMem(data);

        //    return new Font(privateFonts.Families[0], size, FontStyle.Regular, GraphicsUnit.Pixel, 238);  // GraphicsUnit.Pixel, );
        //}


        public static Font GetFont(int size)
        {
            return ResourceHelper.GetFont(size, FontStyle.Regular);

            //var fontPath = System.IO.Directory.GetCurrentDirectory() + @"/AnnualReportsLib/Charting/fonts/FreeSerif.otf"; //Roboto-Regular.ttf
            //System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();
            //privateFonts.AddFontFile(fontPath);
            //return new Font(privateFonts.Families[0], size, FontStyle.Regular, GraphicsUnit.Pixel, 238);  // GraphicsUnit.Pixel, );
        }

        public static Font GetBoldFont(int size)
        {
            return ResourceHelper.GetFont(size, FontStyle.Bold);

            //var fontPath = System.IO.Directory.GetCurrentDirectory() + @"/AnnualReportsLib/Charting/fonts/FreeSerifBold.otf";//Roboto-Bold.ttf";
            //System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();
            //privateFonts.AddFontFile(fontPath);

            //return new Font(privateFonts.Families[0], size);
        }
    }
}