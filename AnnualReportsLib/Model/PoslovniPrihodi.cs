using System;
using System.Collections.Generic;
using AnnualReports.Document;
using AnnualReports.Utils;
using AnnualReports.Charting;

namespace AnnualReports.Model
{
    public class PoslovniPrihodi
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka PrihodiOdProdajeSPodUnGrupe { get; set; }
        public Stavka PrihodiOdProdajeIzvanGrupe { get; set; }
        public Stavka PrihodiNaTemeljuUpotrebeVlastitihProizvoda { get; set; }
        public Stavka OstaliPoslovniPrihodiSPodUnGrupe { get; set; }
        public Stavka OstaliPoslovniPrihodiIzvanGrupe { get; set; }
        public Stavka Ukupno { get; set; }

        public decimal PrihodiOdProdaje => PrihodiOdProdajeIzvanGrupe.TekucaGodina + PrihodiOdProdajeSPodUnGrupe.TekucaGodina;

        public decimal OstaliPoslovniPrihodi => OstaliPoslovniPrihodiSPodUnGrupe.TekucaGodina + OstaliPoslovniPrihodiIzvanGrupe.TekucaGodina;

        public Table GetTable()
        {
            var array = new string[6, 4];
            array.SetRow(0, PrihodiOdProdajeSPodUnGrupe.ToRow("Prihodi od prodaje s poduzetnicima unutar grupe"));
            array.SetRow(1, PrihodiOdProdajeIzvanGrupe.ToRow("Prihodi od prodaje"));
            array.SetRow(2, PrihodiNaTemeljuUpotrebeVlastitihProizvoda.ToRow("Prihodi na temelju upotrebe vlastitih proizvoda, robe i usluga"));
            array.SetRow(3, OstaliPoslovniPrihodiSPodUnGrupe.ToRow("Ostali poslovni prihodi s poduzetnicima unutar grupe"));
            array.SetRow(4, OstaliPoslovniPrihodiIzvanGrupe.ToRow("Ostali poslovni prihodi"));
            array.SetRow(5, Ukupno.Duplicate("UKUPNO").ToRow());

            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        public Table GetPrihodiOdProdajeTable()
        {
            var array = new string[3, 4];
            array.SetRow(0, PrihodiOdProdajeSPodUnGrupe.ToRow("Prihodi od prodaje s poduzetnicima unutar grupe"));
            array.SetRow(1, PrihodiOdProdajeIzvanGrupe.ToRow("Prihodi od prodaje"));

            var ukupnoProslaGodina = PrihodiOdProdajeIzvanGrupe.ProslaGodina + PrihodiOdProdajeSPodUnGrupe.ProslaGodina;
            var ukupnoTekucaGodina = PrihodiOdProdajeIzvanGrupe.TekucaGodina + PrihodiOdProdajeSPodUnGrupe.TekucaGodina;
            Stavka ukupno = new Stavka("UKUPNO", "", ukupnoProslaGodina, ukupnoTekucaGodina);
            array.SetRow(2, ukupno.ToRow());
            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        private PieChartEntry GetPieChart()
        {
            var entries = new List<Tuple<string, decimal>>();

            entries.Add(PrihodiOdProdajeSPodUnGrupe.ToChartEntry("Prihodi od prodaje (grupa)"));
            entries.Add(PrihodiOdProdajeIzvanGrupe.ToChartEntry("Prihodi od prodaje"));
            entries.Add(PrihodiNaTemeljuUpotrebeVlastitihProizvoda.ToChartEntry("Upotreba vl.proizvoda, robe i usluga"));
            entries.Add(OstaliPoslovniPrihodiSPodUnGrupe.ToChartEntry("Ostali prihodi (grupa)"));
            entries.Add(OstaliPoslovniPrihodiIzvanGrupe.ToChartEntry("Ostali prihodi"));

            return new PieChartEntry()
            {
                Title = "Struktura poslovnih prihoda",
                Entries = entries
            };
        }

        private BarChartEntry GetBarChart()
        {
            var entries = new List<BarEntry>();

            entries.Add(PrihodiOdProdajeSPodUnGrupe.ToBarEntry("Prihodi od prodaje (grupa)"));
            entries.Add(PrihodiOdProdajeIzvanGrupe.ToBarEntry("Prihodi od prodaje"));
            entries.Add(PrihodiNaTemeljuUpotrebeVlastitihProizvoda.ToBarEntry("Upotreba vl.proizvoda, robe i usluga"));
            entries.Add(OstaliPoslovniPrihodiSPodUnGrupe.ToBarEntry("Ostali prihodi (grupa)"));
            entries.Add(OstaliPoslovniPrihodiIzvanGrupe.ToBarEntry("Ostali prihodi"));

            return new BarChartEntry(ProslaGodina.ToString(), Godina.ToString())
            {
                Title = "Grafikon usporedbe s prethodnom godinom",
                Entries = entries
            };
        }

        public List<Entry> GetNotes()
        {
            var notes = new List<Entry>();
            notes.Add(new Paragraph(Note, Ukupno.TekucaGodina));

            if (Ukupno.TekucaGodina > 0)
            {
                notes.Add(GetTable());
                notes.Add(GetBarChart());
                notes.Add(GetPieChart());
            }
            notes.Add(new Paragraph(PrihodiOdProdajeNote, PrihodiOdProdaje));
            notes.Add(GetPrihodiOdProdajeTable());
            notes.Add(new Paragraph(PrihodiNaTemeljuUpotrebeNote, PrihodiNaTemeljuUpotrebeVlastitihProizvoda.TekucaGodina));
            notes.Add(new Paragraph(OstaliPoslovniPrihodiNote, OstaliPoslovniPrihodi));

            return notes;
        }

        public static Note Note = new Note()
        {
            Title = "Poslovni prihodi",
            Subtitle = "Bilješka br. 2",
            Info = "Poslovni prihodi sastoje se od prihoda od prodaje, prihod na temelju uporabe vlastitih proizvoda i ostalih poslovnih prihoda.",
            Value = "Poslovni prihodi Društva u poslovnoj godini ukupno su iznosili {0:N2} kn.",
        };

        public static Note PrihodiOdProdajeNote = new Note()
        {
            Title = "Prihodi od prodaje",
            Subtitle = "Bilješka br. 2a",
            Info = "Prema odredbama Zakona o računovodstvu iznos bruto nastalih prihoda s osnove otuđenja dugotrajne imovine u godinama koje su prethodile 2016. godini iskazivao se na poziciji izvanrednih prihoda, a od 2016. godine iskazuje se na poziciji prihoda od prodaje.",
            Value = "Društvo je u poslovnoj godini ostvarilo prihode od prodaje u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo prihoda od prodaje."
        };

        public static Note PrihodiNaTemeljuUpotrebeNote = new Note()
        {
            Title = "Prihodi na temelju upotrebe vlastitih proizvoda, robe i usluga",
            Subtitle = "Bilješka br. 2b",
            Value = "Društvo je u poslovnoj godini ostvarilo prihode na temelju upotrebe vlastitih proizvoda, robe i usluga u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije ostvarilo prihode s osnove upotrebe vlastitih proizvoda, robe i usluga."
        };

        public static Note OstaliPoslovniPrihodiNote = new Note()
        {
            Title = "Ostali poslovni prihodi",
            Subtitle = "Bilješka br. 2c",
            Value = "Društvo je u poslovnoj godini ostvarilo ostalih prihoda u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo ostalih poslovnih prihoda."
        };
    }
}