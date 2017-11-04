using System;
using System.Collections.Generic;
using AnnualReports.Document;
using AnnualReports.Utils;

namespace AnnualReports.Model
{
    public class FinancijskiRashodi
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka RashodiSOsnoveKamataISlRashodiSPodUnGrupe { get; set; }
        public Stavka TecajneRazlikeIDrugiRashodiSPodUnGrupe { get; set; }
        public Stavka RashodiSOsnoveKamataISlRashodi { get; set; }
        public Stavka TecajneRazlikeIDrRashodi { get; set; }
        public Stavka NerealiziraniGubiciOdFinImovine { get; set; }
        public Stavka VrijednosnaUskladjenjaFinImovine { get; set; }
        public Stavka OstaliFinRashodi { get; set; }
        public Stavka Ukupno { get; set; }

        public Table GetTable()
        {
            var array = new string[8, 4];

            array.SetRow(0, RashodiSOsnoveKamataISlRashodiSPodUnGrupe.ToRow("Rashodi s osnove kamata i slični rashodi s poduzetnicima unutar grupe"));
            array.SetRow(1, TecajneRazlikeIDrugiRashodiSPodUnGrupe.ToRow("Tečajne razlike i drugi rashodi s poduzetnicima unutar grupe"));
            array.SetRow(2, RashodiSOsnoveKamataISlRashodi.ToRow("Rashodi s osnove kamata i slični rashodi"));
            array.SetRow(3, TecajneRazlikeIDrRashodi.ToRow("Tečajne razlike i drugi rashodi"));
            array.SetRow(4, NerealiziraniGubiciOdFinImovine.ToRow("Nerealizirani gubici (rashodi) od financijske imovine"));
            array.SetRow(5, VrijednosnaUskladjenjaFinImovine.ToRow("Vrijednosna usklađenja financijske imovine (neto)"));
            array.SetRow(6, OstaliFinRashodi.ToRow("Ostali financijski rashodi"));
            array.SetRow(7, Ukupno.Duplicate("UKUPNO").ToRow());

            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        public BarChartEntry GetBarChart()
        {
            return new BarChartEntry(ProslaGodina.ToString(), Godina.ToString())
            {
                Entries = new List<Charting.BarEntry>()
                {
                    RashodiSOsnoveKamataISlRashodi.ToBarEntry("Kamate i sl.(grupa)"),
                    TecajneRazlikeIDrugiRashodiSPodUnGrupe.ToBarEntry("Tecajne razlike i sl. (grupa)"),
                    RashodiSOsnoveKamataISlRashodi.ToBarEntry("Kamate i sl."),
                    TecajneRazlikeIDrRashodi.ToBarEntry("Tecajne razlike"),
                    NerealiziraniGubiciOdFinImovine.ToBarEntry("Nerealizirani gubici"),
                    VrijednosnaUskladjenjaFinImovine.ToBarEntry("Vrijednosna uskladjenja"),
                    OstaliFinRashodi.ToBarEntry("Ostalo")
                }
            };
        }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina)); ;
            if (Ukupno.TekucaGodina > 0)
            {
                entries.Add(GetTable());
                entries.Add(GetBarChart());
            }
            entries.Add(new Paragraph(FinRashodiUnutarGrupeNote, RashodiSOsnoveKamataISlRashodiSPodUnGrupe.TekucaGodina + TecajneRazlikeIDrugiRashodiSPodUnGrupe.TekucaGodina));
            entries.Add(new Paragraph(FinRashodiSOsnoveKamataISlIzvanGrupeNote, RashodiSOsnoveKamataISlRashodi.TekucaGodina));
            entries.Add(new Paragraph(NerealiziraniGubiciNote, NerealiziraniGubiciOdFinImovine.TekucaGodina));
            entries.Add(new Paragraph(NetoVrijednosnaUskladjenjaNote, VrijednosnaUskladjenjaFinImovine.TekucaGodina));
            entries.Add(new Paragraph(OstaliFinancijskiRashodiNote, OstaliFinRashodi.TekucaGodina));

            return entries;
        }

        public static Note Note = new Note()
        {
            Title = "Financijski rashodi",
            Subtitle = "Bilješka br. 6",
            Info = "Financijski rashodi se odnose na kamate, tečajne razlike i druge rashode, nerealizirane gubitke (rashode) od financijske imovine, neto vrijednosna usklađenja financijske imovine i ostale financijske rashode.",
            Value = "Društvo je u poslovnoj godini iskazalo financijske rashode u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo financijskih rashoda."
        };

        public static Note FinRashodiUnutarGrupeNote = new Note()
        {
            Title = "Financijski rashodi s poduzetnicima unutar grupe",
            Subtitle = "Bilješka br. 6a",
            Value = "Financijski rashodi s poduzetnicima unutar grupe u poslovnoj godini iznosili su {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo financijskih rashoda s poduzetnicima unutar grupe."
        };

        public static Note FinRashodiSOsnoveKamataISlIzvanGrupeNote = new Note()
        {
            Title = "Financijski rashodi s osnove kamata i sličnih rashoda, tečajnih razlika i drugih rashoda s društvima koji nisu unutar grupe",
            Subtitle = "Bilješka br. 6b",
            Value = "Financijski rashodi koji su ostvareni iz poslovnog odnosa s poduzetnicima koji nisu unutar grupe ili iz odnosa s financijskim institucijama, a koji se sastoje od rashoda kamata, tečajnih razlika i ostalih financijskih rashoda iznosili su {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo financijskih rashoda s osnove kamata i sličnih rashoda, tečajnih razlika i drugih rashoda s društvima koji nisu unutar grupe."
        };

        public static Note NerealiziraniGubiciNote = new Note()
        {
            Title = "Nerealizirani gubici od financijske imovine",
            Subtitle = "Bilješka br. 6c",
            Zero = "Društvo ne posjeduje financijsku imovinu koja se svodi na prodajnu vrijednost prema tržišnoj vrijednosti izlistanoj na Burzi i/ili uređenom tržištu vrijednosnih papira odnosno nije ostvarilo gubitke na dionicama iz vlastitog portfelja.",
            Value = "Nerealizirani gubici od financijske imovine u poslovnoj godini iznosili su {0:N2} kn."
        };

        public static Note NetoVrijednosnaUskladjenjaNote = new Note()
        {
            Title = "Neto vrijednosna usklađenja financijske imovine",
            Subtitle = "Bilješka br. 6d",
            Info = "Neto vrijednosno usklađenje financijske imovine proizlazi iz promjene njene vrijednosti prema mjerenju po fer vrijednosti.",
            Value = "Neto vrijednosna usklađenja financijske imovine u poslovnoj godini iznosila su {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo neto vrijednosnih usklađenja."
        };

        public static Note OstaliFinancijskiRashodiNote = new Note()
        {
            Title = "Ostali financijski rashodi",
            Subtitle = "Bilješka br. 6e",
            Info = "Ostali financijski rashodi se odnose na financijske rashode koji su ostvareni od ulaganja u udjele i dionice društava s udjelom vlasništva manjim od 20% (realizirani gubici) te drugih financijskih rashoda koji se ne odnose na kamate zajmova i tečajne razlike.",
            Value = "Društvo je u poslovnoj godini iskazalo ostale financijske rashode u visini od {0:N2} kn.",
            Zero = "Društvo u poslovnoj godini nije imalo ostalih financijskih rashoda."
        };
    }
}