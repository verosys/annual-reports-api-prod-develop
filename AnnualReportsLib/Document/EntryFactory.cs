using System;
using System.Xml.Linq;

namespace AnnualReports.Document
{
    public static class EntryFactory
    {
        //factory pattern
        public static Entry CreateEntry (XElement element, Params parameters)
        {
            
            if (element.Name.ToString().Equals("Paragraph"))
            {
                return new Paragraph(element, parameters);
            }

            else if (element.Name.ToString().Equals("Table"))
			{

                return new Table(element);
			}

            else if (element.Name.ToString().Equals("Section"))
            {
                return new Section(element, parameters);
            }

            return null;
        }
    }
}
