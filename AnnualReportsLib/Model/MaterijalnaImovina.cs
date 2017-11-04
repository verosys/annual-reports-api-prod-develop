using System;
using System.Collections.Generic;
using AnnualReports.Document;
using AnnualReports.Utils;

namespace AnnualReports.Model
{
    public class MaterijalnaImovina
    {
        public int Godina { get; set; }
        public int ProslaGodina { get { return Godina - 1; } }

        public Stavka Zemljiste { get; set; }
        public Stavka GradjevinskiObjekti { get; set; }
        public Stavka PostrojenjaIOprema { get; set; }
        public Stavka AlatiPogonskiInventarTransportnaImovina { get; set; }
        public Stavka BioloskaImovina { get; set; }
        public Stavka PredujmoviZaMaterijalnuImovinu { get; set; }
        public Stavka MaterijalnaImovinaUPripremi { get; set; }
        public Stavka OstalaMaterijalnaImovina { get; set; }
        public Stavka UlaganjeUNekretnine { get; set; }
        public Stavka Ukupno { get; set; }

        public static string[,] GetStopeTable()
        {
            var table = new string[11, 2];
            table[0, 0] = "Građevinski objekti";
            table[1, 0] = "Brodovi veći od 1000 BRT";
            table[2, 0] = "Osobni automobili";
            table[3, 0] = "Osnovno stado";
            table[4, 0] = "Oprema";
            table[5, 0] = "Dostavna vozila";
            table[6, 0] = "Mehanička oprema";
            table[7, 0] = "Računalna oprema";
            table[8, 0] = "Telekomunikacijska oprema";
            table[9, 0] = "Software";
            table[10, 0] = "Ostala nespomenuta imovina";

            table[0, 1] = "5 %";
            table[1, 1] = "5 %";
            table[2, 1] = "20 %";
            table[3, 1] = "20 %";
            table[4, 1] = "25 %";
            table[5, 1] = "25 %";
            table[6, 1] = "25 %";
            table[7, 1] = "50 %";
            table[8, 1] = "50 %";
            table[9, 1] = "50 %";
            table[10, 1] = "10 %";

            return table;
        }

        public List<Entry> GetNotes()
        {

            var entries = new List<Entry>
            {
                new Paragraph(Note, Ukupno.TekucaGodina)
            };

            if (Ukupno.TekucaGodina > 0)
            {
                entries.Add(GetTable());

                entries.Add(AdditionI);
                entries.Add(AdditionII);
                entries.Add(AdditionIII);
            }

            return entries;
        }

        public Table GetTable()
        {
            var array = new string[10, 4];

            array.SetRow(0, Zemljiste.ToRow());
            array.SetRow(1, GradjevinskiObjekti.ToRow());
            array.SetRow(2, PostrojenjaIOprema.ToRow());
            array.SetRow(3, AlatiPogonskiInventarTransportnaImovina.ToRow());
            array.SetRow(4, BioloskaImovina.ToRow());
            array.SetRow(5, PredujmoviZaMaterijalnuImovinu.ToRow());
            array.SetRow(6, MaterijalnaImovinaUPripremi.ToRow());
            array.SetRow(7, OstalaMaterijalnaImovina.ToRow());
            array.SetRow(8, UlaganjeUNekretnine.ToRow());
            array.SetRow(9, Ukupno.Duplicate("UKUPNO").ToRow());
            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        public static Note Note = new Note()
        {
            Title = "Materijalna imovina",
            Subtitle = "Bilješka br. 10",
            Info = "Materijalna imovina sastoji se od zemljišta, građevinskih objekata, postrojenja i oprema, alata, pogonskog inventara i transportne imovine, biološke imovine, predujmova za materijalnu imovinu, materijalne imovine u pripremi, ostale materijalne imovine i ulaganja u nekretnine.",
            Value = "Knjigovodstvena vrijednost materijalne imovine na dan izvještavanja iznosila je {0:N2} kn."
        };

        public static Paragraph AdditionI = new Paragraph()
        {
            Text = "Amortizacija materijalne imovine (ispravak vrijednosti) obračunava se sukladno korisnom vijeku uporabe, linearnom metodom i primjenom stopa u skladu s računovodstvenim politikama."
        };
        public static Paragraph AdditionII = new Paragraph()
        {
            Text = "Knjigovodstvena vrijednost nekretnina koja je utvrđena u skladu sa standardima financijskog izvještavanja (tj. neto amortizirane vrijednosti) i njezine fer vrijednosti na datum promjene njezine namjene u ulaganja u nekretnine. Ova početna razlika evidentirala se kao revalorizacijska rezerva u skladu sa standardima financijskog izvještavanja."
        };
        public static Paragraph AdditionIII = new Paragraph()
        {
            Text = "Prilikom prijenosa nekretnine mjerene po fer vrijednosti na nekretninu koju koristi vlasnik(druga mogućnost koja je stavljena na zalihu) fer vrijednost na datum prijenosa jest trošak nabave."
        };
    }
}