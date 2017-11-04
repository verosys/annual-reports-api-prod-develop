using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class PotrazivanjaDugotrajna:Potrazivanja
    {

        private decimal ZaIsporucenuRobuIUsluge
        {

            get
            {
                return PotrazivanjaOdPoduzetnikaUnutarGrupe.TekucaGodina + PotrazivanjaOdDrustavaPovSudjelujucimInteresom.TekucaGodina
                                                           + PotrazivanjaOdKupaca.TekucaGodina; //TODO:Check this with Vesna
            }
        }

        public List<Entry> GetNotes()
        {

            var entries = new List<Entry>()
            {
                new Paragraph (Note, Ukupno.TekucaGodina)
            };

            if (Ukupno.TekucaGodina > 0)
            { 
                entries.Add(new Paragraph(ZaIsporucenuRobuIUslugeNote, ZaIsporucenuRobuIUsluge));
                entries.Add(new Paragraph(OstalaPotrazivanjaNote, OstalaPotrazivanja.TekucaGodina ));
            }
            return entries;
        }

        public static Note Note = new Note()
        {
            Title = "Dugoročna potraživanja",
            Subtitle = "Bilješka br. 12",
            Zero = "Društvo na dan izvještavanja nije imalo dugoročnih potraživanja",
            Value = "Dugoročna potraživanja na dan izvještavanja iznosila su {0:N2} kn."
        };

        public static Note ZaIsporucenuRobuIUslugeNote = new Note()
        {
            Title = "Dugoročna potraživanja za isporučenu robu i usluge",
            Subtitle = "Bilješka br. 12a",
            Info = "Dugoročna potraživanja za isporučenu robu i usluge i ostala potraživanja razrađuju se na potraživanja od poduzetnika unutar grupe, od društava povezanih sudjelujućim interesom i od nepovezanih osoba.",
            Zero = "Društvo na dan izvještavanja nije imalo dugoročnih potraživanja za isporučenu robu i usluge.",
            Value = "Dugoročna potraživanja za isporučenu robu i usluge na dan izvještavanja iznosila su {0:N2} kn."
        };

        //public static Note PotrazivanjaOdKupacaNote = new Note()
        //{
        //    Title = "Potraživanja od kupaca",
        //    Subtitle = "Bilješka br. 12b",
        //    Info = "",
        //    Zero = "Društvo na dan izvještavanja nije imalo potraživanja od kupaca.",
        //    Value = "Potraživanja od kupaca na dan izvještavanja iznosila su {0:N2} kn."
        //};

     
        public static Note OstalaPotrazivanjaNote = new Note()
        {
            Title = "Ostala dugoročna potraživanja",
            Subtitle = "Bilješka br. 12c",
            Info = "",
            Zero = "Društvo na dan izvještavanja nije imalo ostalih dugoročnih potraživanja.",
            Value = "Ostala dugoročna potraživanja na dan izvještavanja iznosila su {0:N2} kn."
        };
    }
}