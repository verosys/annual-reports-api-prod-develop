using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class Rezerviranja
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka RezerviranjaZaMirovineOtpremnineISlObveze { get; set; }
        public Stavka RezerviranjaZaPorezneObveze { get; set; }
        public Stavka RezerviranjaZaZapoceteSudskeSporove { get; set; }
        public Stavka RezerviranjaZaTroskoveObnavljanjaPrirodnihBogatstava { get; set; }
        public Stavka RezerviranjaZaTroskoveUJamstvenimRokovima { get; set; }
        public Stavka DrugaRezerviranja { get; set; }
        public Stavka Ukupno { get; set; }


        public List<Entry> GetNotes ()
        {
			var entries = new List<Entry>();

			entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));

            return entries;
        }

        public List<Entry> GetNotesRDG()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(NoteRDG, Ukupno.TekucaGodina));

            return entries;
        }

		public static Note Note = new Note()
		{
			Title = "Rezerviranja",
			Subtitle = "Bilješka br. 28",
			Info = "Rezerviranja se provode u skladu s računovodstvenim politikama i standardima financijskog izvještavanja.",
            Value = "Na dan izvještavanja Društvo je imalo rezerviranja u iznosu {0:N2} kn.", 
            Zero = "Društvo na dan izvještavanja nije imalo provedena rezerviranja."
		};

        //TODO; za rdg provjeriti s vesnom
        public static Note NoteRDG = new Note()
        {
            Title = "Rezerviranja",
            Subtitle = "Bilješka br. 28",
            Value = "Na dan izvještavanja Društvo je imalo rezerviranja u iznosu {0:N2} kn.",
            Zero = "Društvo tijekom sastavljanja financijskih izvještaja, a u skladu s odredbama Zakona o porezu na dobit te standarada financijskog izvještavanja, nije imalo potrebe niti obavezu izvršiti rezerviranja."
        };
	}
}