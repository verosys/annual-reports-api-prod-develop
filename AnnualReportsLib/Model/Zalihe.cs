using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class Zalihe
    {
        public int Godina { get; set; }
        public int ProslaGodina { get { return Godina - 1; } }
        public Stavka SirovineIMaterijal { get; set; }
        public Stavka ProizvodnjaUTijeku { get; set; }
        public Stavka GotoviProizvodi { get; set; }
        public Stavka TrgovackaRoba { get; set; }
        public Stavka PredujmoviZaZalihe { get; set; }
        public Stavka DugotrajnaImovinaNamijenjenaProdaji { get; set; }
        public Stavka BioloskaImovina { get; set; }
        public Stavka Ukupno { get; set; }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            var paragraph = new Paragraph()
            {
                Title = "Zalihe",
                Subtitle = "Bilješka br. 14",
                Text = Ukupno.TekucaGodina > 0 ? String.Format(ValueText, Ukupno.TekucaGodina) : ZeroText
            };

            entries.Add(paragraph);

            return entries;
        }

        private static String ZeroText = "Društvo na dan izvještavanja nije imalo zaliha.";
        private static String ValueText = "\nKnjigovodstvena vrijednost zaliha na dan izvještavanja iznosila je {0} kn.";

    }
}
