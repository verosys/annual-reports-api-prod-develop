using System;

using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class Obveznik
    {
        public string OIB { get; set; }
        public string MaticniBroj { get; set; }
        public string MaticniBrojSubjekta { get; set; }
        public string Naziv { get; set; }
        public string UlicaIKucniBroj { get; set; }
        public string PostanskiBroj {get; set; }
        public string NazivNaselja { get; set; }
		public string Email { get; set; }
        public string Telefon { get; set; }
		public string InternetAdresa { get; set; }
        public string SifraGradaIliOpcine { get; set; }
		public string NazivGradaIliOpcine { get; set; }
		public string SifraZupanije { get; set; }
		public string NazivZupanije { get; set; }

        public string SifraVrstePoslovnogSubjekta { get; set; }
        public string NazivVrstePoslovnogSubjekta { get; set; }

        public string NKDSifra { get; set; }
		public string NKDOpis { get; set; }

        public string StatusAutonomnosti { get; set; }
        public string OpisStatusaAutonomnosti { get; set; }
        public string MaticniBrojNadredjenogDrustva { get; set; }
        public string SifraZemljeSjedistaNadredjenogDrustva { get; set; }
        public string NazivZemljeSjedistaNadredjenogDrustva { get; set; }

        public string OznakaVelicine { get; set; }
		public string OpisVelicine { get; set; }
		public string OznakaVlasnistva { get; set; }
        public string OpisVlasnistva { get; set; }

		public string PorijekloKapitalaDomaci { get; set; } //postotak
		public string PorijekloKapitalaStrano { get; set; } //postotak

        public string OvlastenaOsoba { get; set; }
        public string Uprava { get; set; }
        public string VoditeljRacunovodstva { get; set; }
     
        public List<Entry> GetNotes ()
        {
            var adresa = String.Format("{0}, {1} {2}", UlicaIKucniBroj, PostanskiBroj, NazivGradaIliOpcine);
            var uprava = Uprava != null ? Uprava : OvlastenaOsoba;

            return new List<Entry>()
            {
                new Paragraph()
                {
                    Text = String.Format("{0} (u nastavku: Društvo), OIB {1}, osnovano je prema zakonima i propisima Republike Hrvatske kao {2}.", Naziv, OIB, NazivVrstePoslovnogSubjekta)
                },
                new Paragraph()
                {
                    Text = String.Format("Sjedište Društva: {0},",adresa)
                },
                new Paragraph()
                {
                    Text = String.Format("Uprava društva: {0}, direktor Društva.", uprava)
                },
                new Paragraph()
                {
                    Text = String.Format("Osnovna djelatnost društva je: {0}.", NKDOpis)
                }
            };
        }
    }
}