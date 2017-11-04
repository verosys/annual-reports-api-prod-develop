using System;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;


namespace AnnualReports.Utils
{
    public class ResourceHelper
    {

        public static Font GetFont(int size, FontStyle style)
        {
            string resourceId;
            if (style == FontStyle.Bold)
                resourceId = "AnnualReportsLib.Charting.fonts.FreeSerifBold.otf";
            else
                resourceId = "AnnualReportsLib.Charting.fonts.FreeSerif.otf";

            System.Drawing.Text.PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();

            var assembly = typeof(ResourceHelper).GetTypeInfo().Assembly;

            //string[] resources = assembly.GetManifestResourceNames();

            //Console.WriteLine(resources[0]);
            //Console.WriteLine(resources[1]);

            Stream fontStream = assembly.GetManifestResourceStream(resourceId);

            System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);

            // create a buffer to read in to
            byte[] fontdata = new byte[fontStream.Length];

            // read the font data from the resource
            fontStream.Read(fontdata, 0, (int)fontStream.Length);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);

            // pass the font to the font collection
            privateFonts.AddMemoryFont(data, (int)fontStream.Length);

            // close the resource stream
            fontStream.Close();

            // free up the unsafe memory
            Marshal.FreeCoTaskMem(data);

            return new Font(privateFonts.Families[0], size, FontStyle.Regular, GraphicsUnit.Pixel, 238);  // GraphicsUnit.Pixel, );
        }

        public static Stream GetEmptyDocxStream ()
        {

            string resourceId = "AnnualReportsLib.resources.empty.docx";

            var assembly = typeof(ResourceHelper).GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceId))
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                return memoryStream;
            }
        }


        public static XDocument GetNotesStaticText()
        {
            string resourceId = "AnnualReportsLib.resources.notes_static_text.xml";

            var assembly = typeof(ResourceHelper).GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceId))
            {
                return XDocument.Load(stream);
            }
        }
    }
}