using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace AnnualReports.Document
{
    public class Section : Entry
    {
        public string Title { get; set; }
        public List<Entry> Entries { get; set; }

        public Section (string title, List<Entry> entries)
        {
            Title = title;
            Entries = entries;
        }

        public Section(XElement element, Params parameters)
        {
            Title = element.Descendants("Title").FirstOrDefault().Value;

            //Console.WriteLine(Title);
            Entries = new List<Entry>();
            foreach (var subnode in element.Descendants())
            {
                var entry = EntryFactory.CreateEntry(subnode, parameters);
                if (entry != null)
                    Entries.Add(entry);
            }
        }
    }
}