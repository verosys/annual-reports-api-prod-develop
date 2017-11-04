using System;
using System.Collections.Generic;
using AnnualReports.Document;
using AnnualReports.Utils;

namespace AnnualReports.Model
{
    public class MaterijalniTroskovi
    {
        public int Godina { get; set; }
        public int ProslaGodina { get { return Godina - 1; } }

        public Stavka TroskoviSirovinaIMaterijala { get; set; }
        public Stavka TroskoviProdaneRobe { get; set; }
        public Stavka OstaliVanjskiTroskovi { get; set; }
        public Stavka Ukupno { get; set; }

        public Table GetTable()
        {
            var array = new string[4, 4];
            array.SetRow(0, TroskoviSirovinaIMaterijala.ToRow("Troškovi sirovina i materijala"));
            array.SetRow(1, TroskoviProdaneRobe.ToRow("Troškovi prodane robe"));
            array.SetRow(2, OstaliVanjskiTroskovi.ToRow("Ostali vanjski troškovi"));
            array.SetRow(3, Ukupno.Duplicate("UKUPNO").ToRow());

            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        public PieChartEntry GetPieChart()
        {
            var entries = new List<Tuple<string, decimal>>
            {
                TroskoviSirovinaIMaterijala.ToChartEntry("Troskovi sirovina i materijala"),
                TroskoviProdaneRobe.ToChartEntry("Troskovi prodane robe"),
                OstaliVanjskiTroskovi.ToChartEntry("Ostali vanjski troskovi")
            };

            return new PieChartEntry()
            {
                Title = "Struktura materijalnih troskova",
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
                notes.Add(GetPieChart());
            }
            notes.Add(new Paragraph() { Text = AdditionalText });

            return notes;
        }

        public static Note Note = new Note()
        {
            Title = "Materijalni troškovi",
            Subtitle = "Bilješka br. 5b",
            Info = "Materijalni troškovi sastoje se od troškova sirovina i materijala, troškova prodane robe i ostalih vanjskih troškova.",
            Value = "U poslovnoj godini materijalni troškovi iznosili su {0:N2} kn",
            Zero = "Društvo u poslovnoj godini nije imalo materijalnih troškova."
        };

        public static string AdditionalText = "Na poziciji vanjskih troškova prikazani su troškovi kao što su prijevozne usluge, poštanske i telekomunikacijske usluge, trošak vanjskih dorada, trošak servisnih usluga i održavanja, trošak zakupa i leasinga, troškovi promidžbe, sajmova i sponzorstava, trošak intelektualnih i drugih usluga, trošak komunalnih usluga, trošak usluga posredovanja i druge slične usluge.";
    }
}