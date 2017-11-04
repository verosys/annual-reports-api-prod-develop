using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class Bilanca
    {
        public int Godina { get; set; }

        public int ProslaGodina { get { return Godina - 1; } }

        public BilancaAktiva Aktiva { get; set; }

        public BilancaPasiva Pasiva { get; set; }

        public Bilanca()
        {
            Aktiva = new BilancaAktiva();
            Pasiva = new BilancaPasiva();
        }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();
            entries.AddRange(Aktiva.GetNotes());
            entries.AddRange(Pasiva.GetNotes());
            return entries;
        }

        public class BilancaAktiva
        {
            public Stavka PotrazivanjaZaUpisaniNeuplaceniKapital { get; set; }

            public DugotrajnaImovina DugotrajnaImovina { get; set; }

            public KratkotrajnaImovina KratkotrajnaImovina { get; set; }

            public Stavka PlaceniTroskoviBuducegRazdobljaIObracunatiPrihodi { get; set; }

            public Stavka UkupnoAktiva { get; set; }

            public Stavka IzvanBilancniZapisiAktiva { get; set; }

            public BilancaAktiva()
            {
                DugotrajnaImovina = new DugotrajnaImovina();
                KratkotrajnaImovina = new KratkotrajnaImovina();
            }


            public List<Entry> GetNotes()
            {
                var entries = new List<Entry>();

                entries.Add(new Paragraph(){Title ="AKTIVA" });
                entries.AddRange(DugotrajnaImovina.GetNotes());
                entries.AddRange(KratkotrajnaImovina.GetNotes());
                entries.Add(new Paragraph(TroskoviBuducegRazdobljaNote, PlaceniTroskoviBuducegRazdobljaIObracunatiPrihodi.TekucaGodina));
               
                return entries;
            }

            public static Note TroskoviBuducegRazdobljaNote = new Note()
            {
                Title = "Plaćeni troškovi budućeg razdoblja i obračunati prihodi",
                Subtitle = "Bilješka br. 18",
                Zero = "Društvo na dan izvještavanja nije imalo plaćenih troškova budućeg razdoblja i obračunatih prihoda.",
                Value = "Plaćeni troškovi budućeg razdoblja i obračunati prihodi na dan izvještavanja iznosili su {0:N2} kn."
            };
        }

        public class BilancaPasiva
        {
            public KapitaIRezerve KapitalIRezerve { get; set; }

            public Rezerviranja Rezerviranja { get; set; }

            public DugorocneObveze DugorocneObveze { get; set; }

            public KartkorocneObveze KratkorocneObveze { get; set; }

            public Stavka OdgodjenoPlacanjeTroskovaIPrihodBuducegRazdoblja { get; set; }

            public Stavka UkupnoPasiva { get; set; }

            public Stavka IzvanBilancniZapisiPasiva { get; set; }

            public BilancaPasiva()
            {
                KapitalIRezerve = new KapitaIRezerve();
                Rezerviranja = new Rezerviranja();
                DugorocneObveze = new DugorocneObveze();
                KratkorocneObveze = new KartkorocneObveze();
            }


            public List<Entry> GetNotes()
            {
                var entries = new List<Entry>();

                entries.Add(new Paragraph() { Title = "PASIVA" });
                entries.AddRange(KapitalIRezerve.GetNotes());
                entries.AddRange(Rezerviranja.GetNotes());
                entries.AddRange(DugorocneObveze.GetNotes());
                entries.AddRange(KratkorocneObveze.GetNotes());
                entries.Add(new Paragraph(OdgodjenoPlacanjeIPrihodNote, OdgodjenoPlacanjeTroskovaIPrihodBuducegRazdoblja.TekucaGodina));

                return entries;
            }

            public static Note OdgodjenoPlacanjeIPrihodNote = new Note()
            {
                Title = "Odgođeno plaćanje troškova i prihod budućeg razdoblja",
                Subtitle = "Bilješka br. 31",
                Value = "Na dan izvještavanja Društvo je imalo odgođenih plaćanja troškova i/ili prihoda budućeg razdoblja u iznosu {0:N2} kn.",
                Zero = "Društvo na dan izvještavanja nije imalo odgođenih plaćanja troškova i/ili prihoda budućeg razdoblja."
            };
        }
    }
}