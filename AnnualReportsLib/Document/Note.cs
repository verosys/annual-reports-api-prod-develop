using System;
namespace AnnualReports.Document
{
    public struct Note
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Info { get; set; }
        public string Zero { get; set; }
        public string Value { get; set; }

        public string GetText (decimal amount)
        {
            var text = amount > 0 ? String.Format(Value, amount) : Zero;
            return Info + text;
        }
    }
}
