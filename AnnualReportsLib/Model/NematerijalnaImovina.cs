using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class NematerijalnaImovina
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka IzdaciZaRazvoj { get; set; }
        public Stavka KoncesijePatentiLicencijeIDrugo { get; set; }
        public Stavka Goodwill { get; set; }
        public Stavka PredujmoviZaNabavuNematerijalneImovine { get; set; }
        public Stavka NematerijalnaImovinaUPripremi { get; set; }
        public Stavka OstalaNematerijalnaImovina { get; set; }
        public Stavka Ukupno { get; set; }

		public List<Entry> GetNotes()
		{
            return new List<Entry>
            {
                new Paragraph(Note, Ukupno.TekucaGodina)
            };
		}

		public static Note Note = new Note()
		{
			Title = "Nematerijalna imovina",
			Subtitle = "Bilješka br. 9",
			Info = "Nematerijalna imovina sastoji se od izdataka za razvoj, koncesija, patenata, licencija, robnih i uslužnih marki, softvera i ostalih prava, goodwilla, predujmova za nabavku nematerijalne imovine, nematerijalne imovine u pripremi i ostale nematerijalne imovine.",
			Value = "Knjigovodstvena vrijednost nematerijane imovine na dan izvještavanja iznosila je {0:N2} kn."
		};
    }
}