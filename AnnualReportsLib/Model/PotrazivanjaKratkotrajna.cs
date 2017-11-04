using System;

using AnnualReports.Document;
using System.Collections.Generic;

namespace AnnualReports.Model
{
    public class KratkotrajnaPotrazivanja : Potrazivanja
    {
        public Stavka PotrazivanjaOdZaposlenikaIClanovaPoduzetnika { get; set; }
        public Stavka PotrazivanjaOdDrzaveIDrugihInstitucija { get; set; }

        private decimal ZaIsporucenuRobuIUsluge
        {

            get
            {
                return PotrazivanjaOdPoduzetnikaUnutarGrupe.TekucaGodina + PotrazivanjaOdDrustavaPovSudjelujucimInteresom.TekucaGodina
                                                           + PotrazivanjaOdKupaca.TekucaGodina; //TODO:Check this with Vesna
            }
        }

        private decimal OdZaposlenikaIClanova
        {

            get
            {
                return PotrazivanjaOdPoduzetnikaUnutarGrupe.TekucaGodina + PotrazivanjaOdDrustavaPovSudjelujucimInteresom.TekucaGodina
                                                           + PotrazivanjaOdKupaca.TekucaGodina; //TODO:Check this with Vesna
            }
        }

        public  List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));

            entries.Add(new Paragraph(ZaIsporucenuRobuIUslugeNote, ZaIsporucenuRobuIUsluge));

            entries.Add(new Paragraph(PotrazivanjaOdZaposlenikaIClanovaPoduzetnikaNote, PotrazivanjaOdZaposlenikaIClanovaPoduzetnika.TekucaGodina));

            entries.Add(new Paragraph(PotrazivanjaOdDrzaveIDrugihInstitucijaNote, PotrazivanjaOdDrzaveIDrugihInstitucija.TekucaGodina));

            entries.Add(new Paragraph(OstalaPotrazivanjaNote, OstalaPotrazivanja.TekucaGodina));

            return entries;
        }

        public static  Note Note = new Note()
        {
            Title = "Kratkoročna potraživanja",
            Subtitle = "Bilješka br. 15",
            Info = "Kratkoročna potraživanja sastoje se od potraživanja za isporučenu robu i usluge, potraživanja od zaposlenika i članova poduzetnika, potraživanja od države i drugih institucija i ostalih potraživanja za koja se očekuje da će se pretvoriti u novčani oblik u roku do jedne godine.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih potraživanja.",
            Value = "Kratkoročna potraživanja na dan izvještavanja iznosila su {0:N2} kn."
        };

        public static Note ZaIsporucenuRobuIUslugeNote = new Note()
        {
            Title = "Kratkoročna potraživanja za isporučenu robu i usluge",
            Subtitle = "Bilješka br. 15a",
            Info = "Kratkoročna potraživanja za isporučenu robu i usluge i ostala potraživanja razrađuju se na potraživanja od poduzetnika unutar grupe, od društava povezanih sudjelujućim interesom i od nepovezanih osoba.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih potraživanja za isporučenu robu i usluge.",
            Value = "Kratkoročna potraživanja za isporučenu robu i usluge na dan izvještavanja iznosila su {0:N2} kn."
        };

        public static Note PotrazivanjaOdZaposlenikaIClanovaPoduzetnikaNote = new Note()
        {
            Title = "Kratkoročna potraživanja od zaposlenika i članova poduzetnika",
            Subtitle = "Bilješka br. 15b",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih potraživanja od zaposlenika i članova poduzetnika.",
            Value = "Kratkoročna potraživanja od zaposlenika i članova poduzetnika na dan izvještavanja iznosila su {0:N2} kn."
        };

        public static Note PotrazivanjaOdDrzaveIDrugihInstitucijaNote = new Note()
        {
            Title = "Kratkoročna potraživanja od države i drugih institucija",
            Subtitle = "Bilješka br. 15c",
            Info = "Kratkoročna potraživanja od države i drugih institucija odnose se na potraživanja za porez na dobit, potraživanja za PDV, potraživanja za više plaćene članarine i slična potraživanja",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih potraživanja od države i drugih institucija.",
            Value = "Kratkoročna potraživanja od države i drugih institucija na dan izvještavanja iznosila su {0:N2} kn."
        };

        public static Note OstalaPotrazivanjaNote = new Note()
        {
            Title = "Ostala kratkoročna potraživanja",
            Subtitle = "Bilješka br. 15d",
            Info = "Ostala kratkoročna potraživanja sačinjavaju potraživanja stečena cesijom ili nekom drugom vrstom ugovora (otkup tražbine, prijenos i sl.) te potraživanja za dane predujmove.",
            Zero = "Društvo na dan izvještavanja nije imalo dugoročnih potraživanja.",
            Value = "Ostala kratkoročna potraživanja na dan izvještavanja iznosila su {0:N2} kn."
        };
    }
}
