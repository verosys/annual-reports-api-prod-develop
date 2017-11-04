using System;
using System.Collections.Generic;
using AnnualReports.Document;
using AnnualReports.Utils;
using AnnualReports.Charting;

namespace AnnualReports.Model
{
    public class PoslovniRashodi
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka PromjeneVrijednostiZalihaProizUTijekuIGotProizvoda { get; set; }
        public MaterijalniTroskovi MaterijalniTroskovi { get; set; }
        public TroskoviOsoblja TroskoviOsoblja { get; set; }
        public Stavka Amortizacija { get; set; }
        public Stavka OstaliTroskovi { get; set; }
        public VrijednosnaUskladjenja VrijednosnaUskladjenja { get; set; }
        public Rezerviranja Rezerviranja { get; set; }
        public Stavka OstaliPoslovniRashodi { get; set; }
        public Stavka Ukupno { get; set; }

        public PoslovniRashodi()
        {
            MaterijalniTroskovi = new MaterijalniTroskovi();
            TroskoviOsoblja = new TroskoviOsoblja();
            VrijednosnaUskladjenja = new VrijednosnaUskladjenja();
            Rezerviranja = new Rezerviranja();
        }

        public Table GetTable()
        {
            var array = new string[9, 4];
            array.SetRow(0, PromjeneVrijednostiZalihaProizUTijekuIGotProizvoda.ToRow("Promjene vrijednosti"));
            array.SetRow(1, MaterijalniTroskovi.Ukupno.ToRow("Materijalni troškovi"));
            array.SetRow(2, TroskoviOsoblja.Ukupno.ToRow("Troškovi osoblja"));
            array.SetRow(3, Amortizacija.ToRow("Amortizacija"));
            array.SetRow(4, OstaliTroskovi.ToRow("Ostali troškovi"));
            array.SetRow(5, VrijednosnaUskladjenja.Ukupno.ToRow("Vrijednosna usklađenja"));
            array.SetRow(6, Rezerviranja.Ukupno.ToRow("Rezerviranja"));
            array.SetRow(7, OstaliPoslovniRashodi.ToRow("Ostali poslovni rashodi"));
            array.SetRow(8, Ukupno.Duplicate("UKUPNO").ToRow());

            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        public BarChartEntry GetBarChart()
        {
            var entriesTekuca = new List<BarEntry>
            {
                PromjeneVrijednostiZalihaProizUTijekuIGotProizvoda.ToBarEntry("Promjene vrijednosti"),
                MaterijalniTroskovi.Ukupno.ToBarEntry("Materijalni troskovi"),
                TroskoviOsoblja.Ukupno.ToBarEntry("Troskovi osoblja"),
                Amortizacija.ToBarEntry("Amortizacija"),
                OstaliTroskovi.ToBarEntry("Ostali troskovi"),
                VrijednosnaUskladjenja.Ukupno.ToBarEntry("Vrijednosna uskladjenja"),
                Rezerviranja.Ukupno.ToBarEntry("Rezerviranja")
                //OstaliPoslovniRashodi.ToBarEntry()
            };

            return new BarChartEntry(ProslaGodina.ToString(), Godina.ToString())
            {
                Title = "Grafikon usporedbe s prethodnom godinom",
                Entries = entriesTekuca
            };
        }

        public PieChartEntry GetPieChart()
        {
            var entries = new List<Tuple<string, decimal>>
            {
                PromjeneVrijednostiZalihaProizUTijekuIGotProizvoda.ToChartEntry("Promjene vrijednosti"),
                MaterijalniTroskovi.Ukupno.ToChartEntry("Materijalni troskovi"),
                TroskoviOsoblja.Ukupno.ToChartEntry("Troskovi osoblja"),
                Amortizacija.ToChartEntry("Amortizacija"),
                OstaliTroskovi.ToChartEntry("Ostali troskovi"),
                VrijednosnaUskladjenja.Ukupno.ToChartEntry("Vrijednosna uskladjenja"),
                Rezerviranja.Ukupno.ToChartEntry("Rezerviranja"),
                OstaliPoslovniRashodi.ToChartEntry("Ostali poslovni rashodi")
            };

            return new PieChartEntry()
            {
                Title = "Struktura poslovnih rashoda",
                Entries = entries
            };
        }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();
            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));

            if (Ukupno.TekucaGodina > 0)
            {
                entries.Add(GetTable());
                entries.Add(GetPieChart());
                entries.Add(GetBarChart());
            }

            entries.Add(new Paragraph(PromjeneVrijednostiZalihaNote, PromjeneVrijednostiZalihaProizUTijekuIGotProizvoda.TekucaGodina));
            entries.AddRange(MaterijalniTroskovi.GetNotes());
            entries.AddRange(TroskoviOsoblja.GetNotes());
            entries.Add(new Paragraph(AmortizacijaNote, Amortizacija.TekucaGodina));
            entries.Add(new Paragraph(OstaliTroskoviNote, OstaliTroskovi.TekucaGodina));
            entries.AddRange(VrijednosnaUskladjenja.GetNotes());
            entries.AddRange(Rezerviranja.GetNotes()); //TODO: check this note, it's shared with bilanca but probably needs to be different for rdg
            entries.Add(new Paragraph(OstaliPoslovniRashodiNote, OstaliPoslovniRashodi.TekucaGodina));

            return entries;
        }

        public static Note Note = new Note()
        {
            Title = "Poslovni rashodi",
            Subtitle = "Bilješka br. 5",
            Info = "Poslovni rashodi sastoje se od promjene vrijednosti zaliha proizvodnje u tijeku i gotovih proizvoda, materijalnih troškova, troškova osoblja, amortizacije, ostalih troškova, vrijednosnih usklađenja, rezerviranja i ostalih poslovnih rashoda.",
            Value = "Društvo je u poslovnoj godini iskazalo poslovne rashode u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo poslovnih rashoda."
        };

        public static Note PromjeneVrijednostiZalihaNote = new Note()
        {
            Title = "Promjene vrijednosti zaliha proizvodnje u tijeku i gotovih proizvoda",
            Subtitle = "Bilješka br. 5a",
            Value = "Društvo je u poslovnoj godini imalo poslovnih rashoda po osnovi promjene zaliha proizvodnje u tijeku i gotovih proizvoda u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo poslovnih rashoda po osnovi promjene zaliha proizvodnje u tijeku i gotovih proizvoda."
        };

        public static Note AmortizacijaNote = new Note()
        {
            Title = "Amortizacija",
            Subtitle = "Bilješka br. 5d",
            Value = "Troškova amortizacije u poslovnoj godini iznosili su {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo troškova amortizacije."
        };

        public static Note OstaliTroskoviNote = new Note()
        {
            Title = "Ostali troškovi",
            Subtitle = "Bilješka br. 5e",
            Info = "Ostale troškove čine premije osiguranja, bankarske usluge i troškovi platnog prometa, zdravstvene usluge, troškovi prava korištenja, troškovi članarina, troškovi poreza koji ne ovise o dobitku i ostali nematerijalni troškovi poslovanja.",
            Value = "Društvo je u poslovnoj godini imalo ukupno {0:N2} kn ostalih troškova",
            Zero = "Društvo u poslovnoj godini nije imalo ostalih troškova."
        };

        public static Note OstaliPoslovniRashodiNote = new Note()
        {
            Title = "Ostali poslovni rashodi",
            Subtitle = "Bilješka br. 5f",
            Value = "Ostali poslovni rashodi u poslovnoj godini iznosili su {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo ostalih poslovnih rashoda."
        };
    }
}