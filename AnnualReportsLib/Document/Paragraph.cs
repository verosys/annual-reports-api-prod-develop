using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AnnualReports.Document
{
    public class Paragraph : Entry
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Text { get; set; }

        public Paragraph()
        {}

        public Paragraph(Note note, decimal amount)
		{
            this.Title = note.Title;
            this.Subtitle = note.Subtitle;
            this.Text = note.GetText(amount);
        }

        public Paragraph(XElement element, Params parameters)
        {
            var titleElement = element.Descendants("Title").FirstOrDefault();
            if (titleElement != null)
                Title = titleElement.Value;
			var subtitleElement = element.Descendants("Subtitle").FirstOrDefault();
			if (subtitleElement != null)
                Subtitle = subtitleElement.Value;
            var textElement = element.Descendants("Text").FirstOrDefault();
            if (textElement != null)
            {
                Text = textElement.Value.Replace("{godina}", parameters.Year.ToString());
            }
		}
    }
}