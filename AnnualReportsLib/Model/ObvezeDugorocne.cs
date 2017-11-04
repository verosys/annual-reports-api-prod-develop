using System;
using System.Collections.Generic;
using AnnualReports.Document;


namespace AnnualReports.Model
{
    public class DugorocneObveze : Obveze
    {
        public Stavka OdgodjenaPoreznaObveza { get; set; }


        private PieChartEntry GetPieChart()
        {
            var entries = new List<Tuple<string, decimal>>();

            entries.Add(ObvezeRobaUkupno.ToChartEntry("Dugorocne obveze (roba)"));
            entries.Add(ObvezeZajmoviUkupno.ToChartEntry("Dugorocne obveze (zajmovi)"));
            entries.Add(ObvezePremaBankamaIDrugimFinInstitucijama.ToChartEntry("Obveze prema bankama i sl."));
            entries.Add(ObvezeZaPredujmove.ToChartEntry("Obveze za predujmove"));
            entries.Add(ObvezePremaDobavljacima.ToChartEntry("Obveze prema dobavljacima"));
            entries.Add(ObvezePoVrijednosnimPapirima.ToChartEntry("Obveze po vrijednosnim papirima"));
            entries.Add(OstaleObveze.ToChartEntry("Ostale dugorocne obveze"));
            entries.Add(OdgodjenaPoreznaObveza.ToChartEntry("Odgodjena porezna obveza"));

            return new PieChartEntry()
            {
                Title = "Struktura dugorocnih obveza",
                Entries = entries
            };
        }

        //TODO: check with vesna if we should list paragraphs like with KratkorocneObveze
        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));

            if (Ukupno.TekucaGodina > 0)
            {
                entries.Add(GetPieChart());
            }

            return entries;
        }

        public static Note Note = new Note()
        {
            Title = "Dugoročne obveze",
            Subtitle = "Bilješka br. 29",
            Info = "Dugoročne obveze odnose se na obveze s rokom dospijeća dužim od godinu dana.",
            Value = "Na dan izvještavanja Društvo je imalo  dugoročnih obveza u iznosu {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo dugoročnih obveza."
        };
    }
}