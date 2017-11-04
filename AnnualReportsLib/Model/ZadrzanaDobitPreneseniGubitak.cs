using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class ZadrzanaDobitPreneseniGubitak
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka ZadrzanaDobit { get; set; }
        public Stavka PreneseniGubitak { get; set; }
        public Stavka Ukupno { get; set; }

        public List<Entry> GetNotes()
        {
            var paragraph = new Paragraph();
            paragraph.Title = "Zadržana dobit ili preneseni gubitak";
            paragraph.Subtitle = "Bilješka br. 25";

            if (ZadrzanaDobit.TekucaGodina > 0)
            {
                paragraph.Text = String.Format("Društvo je u poslovnoj godini {0}. imalo zadržanu dobit u iznosu od {1:N2} kn.", Godina, ZadrzanaDobit.TekucaGodina);
            }
            else if (PreneseniGubitak.TekucaGodina > 0)
            {
                paragraph.Text = String.Format("Društvo je u poslovnoj godini {0}. imalo preneseni gubitak u iznosu od {1:N2} kn.", Godina, PreneseniGubitak.TekucaGodina);
            }
            else
            {
                paragraph.Text = String.Format("Društvo u poslovnoj godini {0}. nije imalo zadržanu dobit niti preneseni gubitak", Godina);
            }

            return new List<Entry>() {
                paragraph
            };
        }
    }
}