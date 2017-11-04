using System;
using System.Collections.Generic;
using AnnualReports.Document;
using AnnualReports.Utils;

namespace AnnualReports.Model
{
    public class TroskoviOsoblja
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka NetoPlaceINadnice { get; set; }
        public Stavka TroskoviPorezaIDoprinosaIzPlaca { get; set; }
        public Stavka DoprinosiNaPlace { get; set; }
        public Stavka Ukupno { get; set; }

        public Table GetTable()
        {
            var array = new string[4, 4];
            array.SetRow(0, NetoPlaceINadnice.ToRow("Neto plaće i nadnice"));
            array.SetRow(1, TroskoviPorezaIDoprinosaIzPlaca.ToRow("Troškovi poreza i doprinosa iz plaća"));
            array.SetRow(2, DoprinosiNaPlace.ToRow("Doprinosi na plaće"));
            array.SetRow(3, Ukupno.Duplicate("UKUPNO").ToRow());

            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        private PieChartEntry GetPieChart()
        {
            var entries = new List<Tuple<string, decimal>>
            {
                NetoPlaceINadnice.ToChartEntry("Neto place i nadnice"),
                TroskoviPorezaIDoprinosaIzPlaca.ToChartEntry("Troskovi poreza i doprinosa iz placa"),
                DoprinosiNaPlace.ToChartEntry("Doprinosi na place")
            };

            return new PieChartEntry()
            {
                Title = "Struktura troskova osoblja",
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
            return notes;
        }

        public static Note Note = new Note()
        {
            Title = "Troškovi osoblja",
            Subtitle = "Bilješka br. 5c",
            Info = "Troškovi osoblja se odnose na troškove neto plaća i nadnica, troškova poreza i doprinosa iz plaća i troškova doprinosa na plaće.",
            Value = "U poslovnoj godini troškovi osoblja iznosili su {0:N2} kn",
            Zero = "Društvo u poslovnoj godini nije imalo troškova osoblja."
        };
    }
}
