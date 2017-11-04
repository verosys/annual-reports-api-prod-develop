using System;
using System.Collections.Generic;
using AnnualReports.Document;


namespace AnnualReports.Model
{
    public class KartkorocneObveze : Obveze
    {
        public Stavka ObvezePremaZaposlenicima { get; set; }
        public Stavka ObvezeZaPorezeDoprinoseISlDavanja { get; set; }
        public Stavka ObvezeSOsnoveUdjelaURezultatu { get; set; }
        public Stavka ObvezePoOsnoviDugotrajneImovineNamjProdaji { get; set; }

        private PieChartEntry GetPieChart()
        {
            var entries = new List<Tuple<string, decimal>>();

            entries.Add(ObvezeRobaUkupno.ToChartEntry("Kratkorocne obveze (roba)"));
            entries.Add(ObvezeZajmoviUkupno.ToChartEntry("Kratkorocne obveze (zajmovi)"));
            entries.Add(ObvezePremaBankamaIDrugimFinInstitucijama.ToChartEntry("Obveze prema bankama i sl."));
            entries.Add(ObvezeZaPredujmove.ToChartEntry("Obveze za predujmove"));
            entries.Add(ObvezePoVrijednosnimPapirima.ToChartEntry("Obveze po vrijednosnim papirima"));
            entries.Add(ObvezePremaZaposlenicima.ToChartEntry("Obveze prema zaposlenicima"));
            entries.Add(ObvezeZaPorezeDoprinoseISlDavanja.ToChartEntry("Obveze za poreze, doprinose i sl."));
            entries.Add(ObvezeSOsnoveUdjelaURezultatu.ToChartEntry("Obveze s osnove udjela u rezultatu"));
            entries.Add(ObvezePoOsnoviDugotrajneImovineNamjProdaji.ToChartEntry("Obveze - dugotrajna imovina namjenjena prodaji"));
            entries.Add(OstaleObveze.ToChartEntry("Ostale kratkorocne obveze"));

            return new PieChartEntry()
            {
                Title = "Struktura kratkorocnih obveza",
                Entries = entries
            };
        }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(Note, Ukupno.TekucaGodina));
            if (Ukupno.TekucaGodina > 0)
            {
                entries.Add(GetPieChart());
            }
            entries.Add(new Paragraph(ObvezeZaRobuIUslugeNote, ObvezeRobaUkupno.TekucaGodina));
            entries.Add(new Paragraph(ObvezeZaZajmoveDepoziteISlNote, ObvezeZajmoviUkupno.TekucaGodina));
            entries.Add(new Paragraph(ObvezePremaBankamaIDrugimFinInstitucijamaNote, ObvezePremaBankamaIDrugimFinInstitucijama.TekucaGodina));
            entries.Add(new Paragraph(ObvezeZaPredujmoveNote, ObvezeZaPredujmove.TekucaGodina));
            entries.Add(new Paragraph(ObvezePoVrijednosnimPapirimaNote, ObvezePoVrijednosnimPapirima.TekucaGodina));
            entries.Add(new Paragraph(ObvezePremaZaposlenicimaNote, ObvezePremaZaposlenicima.TekucaGodina));
            entries.Add(new Paragraph(ObvezeZaPorezeDoprinoseISlDavanjaNote, ObvezeZaPorezeDoprinoseISlDavanja.TekucaGodina));
            entries.Add(new Paragraph(ObvezeSOsnoveUdjelaURezultatuNote, ObvezeSOsnoveUdjelaURezultatu.TekucaGodina));
            entries.Add(new Paragraph(ObvezePoOsnoviDugotrajneImovineNamjProdajiNote, ObvezePoOsnoviDugotrajneImovineNamjProdaji.TekucaGodina));
            entries.Add(new Paragraph(OstaleObvezeNote, OstaleObveze.TekucaGodina));

            return entries;
        }

        public static Note Note = new Note()
        {
            Title = "Kratkoročne obveze",
            Subtitle = "Bilješka br. 30",
            Info = "Kratkoročne obveze odnose se na obveze s rokom dospijeća kraćim od godinu dana.",
            Value = "Društvo je na dan izvještavanja imalo kratkoročne obveze u visini od {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza."
        };

        //agregratna
        //TODO: check with Vesna, postoji jos podrazrada 
        public static Note ObvezeZaRobuIUslugeNote = new Note()
        {
            Title = "Kratkoročne obveze za primljenu robu i usluge",
            Subtitle = "Bilješka br. 30a",
            Info = "",
            Value = "Društvo je na dan izvještavanja imalo kratkoročne obveze u visini od {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza."
        };

        //agregatna
        //TODO: check with Vesna, postoji jos podrazrada 
        public static Note ObvezeZaZajmoveDepoziteISlNote = new Note()
        {
            Title = "Kratkoročne obveze po osnovi zajmova, depozita i sličnog",
            Subtitle = "Bilješka br. 30b",
            Info = "",
            Value = "Kratkoročne obveze po osnovi primljenih zajmova, depozita i sličnog na dan izvještavanja iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza po osnovi primljenih zajmova, depozita i sličnog."
        };

        public static Note ObvezePremaBankamaIDrugimFinInstitucijamaNote = new Note()
        {
            Title = "Kratkoročne obveze prema bankama i drugim financijskim institucijama",
            Subtitle = "Bilješka br. 30c",
            Info = "",
            Value = "Kratkoročne obveze prema bankama i drugim financijskim institucijama na dan izvještavanja iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročne obveze prema bankama i drugim financijskim institucijama."
        };

        public static Note ObvezeZaPredujmoveNote = new Note()
        {
            Title = "Kratkoročne obveze za predujmove",
            Subtitle = "Bilješka br. 30d",
            Info = "",
            Value = "Obveze za primljene predujmove (avanse) u kojima se roba ili usluga treba isporučiti u roku koji je manji od godine dana na dan izvještavanja iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo obveze za primljene predujmove (avanse) u kojima se roba ili usluga treba isporučiti u roku koji je manji od godine dana."
        };

        public static Note ObvezePoVrijednosnimPapirimaNote = new Note()
        {
            Title = "Kratkoročne obveze po vrijednosnim papirima",
            Subtitle = "Bilješka br. 30e",
            Info = "",
            Value = "Kratkoročne obveze po vrijednosnim papirim na dan izvještavanja iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza po vrijednosnim papirima."
        };

        public static Note ObvezePremaZaposlenicimaNote = new Note()
        {
            Title = "Kratkoročne obveze prema zaposlenicima",
            Subtitle = "Bilješka br. 30f",
            Info = "Kratkoročne obveze prema zaposlenicima odnose se na obračunate, a neisplaćene neto plaće, naknade i slične obveze.",
            Value = "Kratkoročne obveze prema zaposlenicima na dan izvještavanja iznosile su ukupno {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza prema zaposlenicima."
        };

        public static Note ObvezeZaPorezeDoprinoseISlDavanjaNote = new Note()
        {
            Title = "Kratkoročne obveze za poreze, doprinose i slična davanja",
            Subtitle = "Bilješka br. 30g",
            Info = "Kratkoročne obveze za poreze, doprinose i slična davanja odnose se na obračunate, a neplaćene obveze.",
            Value = "Kratkoročne obveze za poreze, doprinose i slična davanja na dan izvještavanja iznosile su ukupno {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza za poreze, doprinose i slična davanja."
        };

        public static Note ObvezeSOsnoveUdjelaURezultatuNote = new Note()
        {
            Title = "Kratkoročne obveze s osnove udjela u rezultatu",
            Subtitle = "Bilješka br. 30h",
            Info = "Kratkoročne obveze s osnove udjela u rezultatu odnose se na obračunate, a neplaćene obveze vezane za obvezu isplate dobiti povezanim društvima.",
            Value = "Kratkoročne obveze s osnove udjela u rezultatu na dan izvještavanja iznosile su ukupno {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza s osnove udjela u rezultatu."
        };

        public static Note ObvezePoOsnoviDugotrajneImovineNamjProdajiNote = new Note()
        {
            Title = "Kratkoročne obveze po osnovi dugotrajne imovine namijenjene prodaji",
            Subtitle = "Bilješka br. 30i",
            Info = "Kratkoročne obveze po osnovi dugotrajne imovine namijenjene prodaji odnose se na\nobračunate, a neplaćene obveze vezane uz dugotrajnu imovinu namijenjenu prodaji.",
            Value = "Kratkoročne obveze po osnovi dugotrajne imovine namijenjene prodaji na dan izvještavanja iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkoročnih obveza po osnovi dugotrajne imovine namijenjene prodaji."
        };

        public static Note OstaleObvezeNote = new Note()
        {
            Title = "Ostale kratkoročne obaveze",
            Subtitle = "Bilješka br. 30j",
            Info = "",
            Value = "Ostale kratkoročne obveze Društva na dan izvještavanja iznosile su {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo ostalih kratkoročnih obveza."
        };
    }
}