using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    //PASIVA
    public class KapitaIRezerve
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka TemeljniKapital { get; set; }

        public Stavka KapitalneRezerve { get; set; }

        public RezerveIzDobiti RezerveIzDobiti { get; set; }

        public Stavka RevalorizacijskeRezerve { get; set; }

        public RezerveFerVrijednosti RezerveFerVrijednosti { get; set; }

        public ZadrzanaDobitPreneseniGubitak ZadrzanaDobitPreneseniGubitak { get; set; }

        public DobitGubitakPoslovneGodine DobitIliGubitakPoslovneGodine { get; set; }

        public Stavka ManjinskiInteres { get; set; }

        public Stavka Ukupno { get; set; }

        public KapitaIRezerve()
        {
            RezerveIzDobiti = new RezerveIzDobiti();
            RezerveFerVrijednosti = new RezerveFerVrijednosti();
            ZadrzanaDobitPreneseniGubitak = new ZadrzanaDobitPreneseniGubitak();
            DobitIliGubitakPoslovneGodine = new DobitGubitakPoslovneGodine();
        }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));
            entries.Add(new Paragraph(TemeljniKapitalNote, TemeljniKapital.TekucaGodina));
            entries.Add(new Paragraph(KapitalneRezerveNote, KapitalneRezerve.TekucaGodina));
            entries.AddRange(RezerveIzDobiti.GetNotes());
            entries.Add(new Paragraph(RevalorizacijskeRezerveNote, RevalorizacijskeRezerve.TekucaGodina));
            entries.AddRange(RezerveFerVrijednosti.GetNotes());
            entries.AddRange(ZadrzanaDobitPreneseniGubitak.GetNotes());
            entries.AddRange(DobitIliGubitakPoslovneGodine.GetNotes());
            entries.Add(new Paragraph(ManjinskiInteresNote, ManjinskiInteres.TekucaGodina));


            return entries;
        }

        private static Note Note = new Note()
        {
			Title = "Kapital i rezerve",
			Subtitle = "Bilješka br. 19",
            Info = "Prema standardima financijskog izvještavanja kapital i rezerve se sastoje od upisanog kapitala, kapitalnih rezervi, rezervi iz dobiti, revalorizacijskih rezervi, rezervi fer vrijednosti, zadržane dobiti ili prenesenog gubitka, dobiti ili gubitka tekućeg razdobljate manjinskog interesa.",
            Value = "Na dan izvještavanja kapital Društva iznosio je {0:N2} kn."
        };

        private static Note TemeljniKapitalNote = new Note()
        {
			Title = "Temeljni(upisani) kapital",
			Subtitle = "Bilješka br. 20",
            Value = "Na dan izvještavanja temeljni kapital Društva iznosio je {0:N2} kn i u cijelosti je\nupisan u sudski registar kod nadležnog trgovačkog suda."
        };

        private static Note KapitalneRezerveNote = new Note()
        {
			Title = "Kapitalne rezerve",
			Subtitle = "Bilješka br. 21",
            Info = "Kapitalne rezerve odnose se na kapitalni dobitak na prodane dionice, kapitalni dobitak\niz prodaje otkupljenih vlastitih dionica i udjela te kapitalne pričuve iz drugih izvora.\nKapitalne rezerve mogu sačinjavati povećanje kapitala društva od ulaganja novca, stvari ili prava od strane vlasnika ili dioničara u neupisani kapital društva.",
            Zero = "Društvo na dan izvještavanja nije imalo kapitalnih rezervi.",
            Value = "Na dan izvještavanja kapitalne rezerve Društva iznosile su {0:N2} kn."
        };

		public static Note RevalorizacijskeRezerveNote = new Note()
		{
			Title = "Revalorizacijske rezerve",
			Subtitle = "Bilješka br. 23",
			Info = "Revalorizacijske rezerve nastaju ponovnom procjenom imovine iznad troškova nabave dugotrajne materijalne i nematerijalne imovine.",
			Value = "Na dan izvještavanja revalorizacijske rezerve Društva iznosile su {0:N2} kn.",
			Zero = "Društvo na dan izvještavanja nije imalo revalorizacijskih rezervi."
		};

		public static Note ManjinskiInteresNote = new Note()
		{
			Title = "Manjinski (nekontrolirajući) interes",
			Subtitle = "Bilješka br. 27",
			Info = "Manjinski interesi odnose se na ostvarenu dobit koja pripada Društvu po osnovi udjela u dobiti od društava kod kojih je postotak udjela u vlasništvu manji od 20%.",
			Value = "Društvo je u poslovnoj godini ostvarilo interes iz odnosa s nekontroliranim društvima u iznosu od {0:N2} kn.",
			Zero = "Društvo nije ostvarilo interes iz odnosa s nekontroliranim društvima."
		};

        //private static Note RezerveIzDobitiNote = new Note()
        //{
        //    Title = "Rezerve iz dobiti",
        //    Subtitle = "Bilješka br. 22",
        //    Info = "Rezerve iz dobiti obuhvaćaju zakonske rezerve, rezerve za vlastite udjele te statutarne i ostale rezerve.",
        //    Zero = "Društvo na dan izvještavanja nije imalo rezervi iz dobiti.",
        //    Value = "Na dan izvještavanja rezerve iz dobiti Društva iznosile su {0:N2} kn."
        //};


      

		//public static Note RezerveFerVrijednostiNote = new Note()
		//{
		//	Title = "Rezerve fer vrijednosti",
		//	Subtitle = "Bilješka br. 24",
		//	Info = "Rezerve fer vrijednosti sastoje se od fer vrijednosti financijske imovine raspoložive za prodaju, učinkovitog dijela zaštite novčanih tijekova i učinkovitog dijela zaštite ulaganja u inozemstvu.",
		//	Value = "Na dan izvještavanja rezerve iz fer vrijednosti Društva iznosile su {0:N2} kn.",
		//	Zero = "Društvo na dan izvještavanja nije oformilo rezerve fer vrijednosti."
		//};

		//public static Note ZadrzanaDobitPreneseniGubitakNote = new Note()
		//{
		//	Title = "Zadržana dobit ili preneseni gubitak",
		//	Subtitle = "Bilješka br. 25",
		//	Info = "",
		//	Value = "Na dan izvještavanja iznos zadrzane dobiti/prenesenog gubitka je bio {0:N2} kn.",
		//	Zero = "Društvo na dan izvještavanja nije imalo zadržanu dobit niti preneseni gubitak."
		//};

		//public static Note DobitIliGubitakPoslovneGodineNote = new Note()
		//{
		//	Title = "Dobit ili gubitak poslovne godine",
		//	Subtitle = "Bilješka br. 26",
		//	Info = "",
        //  Value = "Društvo je u poslovnoj godini ostvarilo neto dobit/gubitak u iznosu od {0:N2} kn.", //TODO: drugi text za negativnu vrijednost
		//	Zero = "Društvo na dan izvještavanja nije imalo zadržanu dobit niti preneseni gubitak."
		//};

	
    }
}
