using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class RezerveIzDobiti
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka ZakonskeRezerve { get; set; }
        public Stavka RezerveZaVlastiteDionice { get; set; }
        public Stavka VlastiteDioniceIUdjeli { get; set; }
        public Stavka StatutarneRezerve { get; set; }
        public Stavka OstaleRezerve { get; set; }
        public Stavka Ukupno { get; set; }

        public List<Entry> GetNotes()
        {
            return new List<Entry>() {
                new Paragraph(Note, Ukupno.TekucaGodina)
            };
        }

        private static Note Note = new Note()
        {
            Title = "Rezerve iz dobiti",
            Subtitle = "Bilješka br. 22",
            Info = "Rezerve iz dobiti obuhvaćaju zakonske rezerve, rezerve za vlastite udjele te statutarne i ostale rezerve.",
            Zero = "Društvo na dan izvještavanja nije imalo rezervi iz dobiti.",
            Value = "Na dan izvještavanja rezerve iz dobiti Društva iznosile su {0:N2} kn."
        };
    }
}