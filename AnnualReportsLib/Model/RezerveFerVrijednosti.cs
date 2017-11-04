using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class RezerveFerVrijednosti
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka FerVrijednostFinImovineRaspoloziveZaProdaju { get; set; }
        public Stavka UcinkovitiDioZastiteNovcanihTokova { get; set; }
        public Stavka UcinkovitiDioZastiteNetoUlaganjaUIinozemstvu { get; set; }
        public Stavka Ukupno { get; set; }

        public List<Entry> GetNotes()
        {
            return new List<Entry>() {
                new Paragraph(Note, Ukupno.TekucaGodina)
            };
        }

        public static Note Note = new Note()
        {
            Title = "Rezerve fer vrijednosti",
            Subtitle = "Bilješka br. 24",
            Info = "Rezerve fer vrijednosti sastoje se od fer vrijednosti financijske imovine raspoložive za prodaju, učinkovitog dijela zaštite novčanih tijekova i učinkovitog dijela zaštite ulaganja u inozemstvu.",
            Value = "Na dan izvještavanja rezerve iz fer vrijednosti Društva iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije oformilo rezerve fer vrijednosti."
        };
    }
}
