using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class FinancijskaImovina
    {
		public int Godina { get; set; }
        public int ProslaGodina { get { return Godina - 1; } }

        public Stavka UlaganjaUUdjeleUnGrupe { get; set; }
        public Stavka UlaganjaUOstaleVrijednosnePapireUnGrupe { get; set; }
        public Stavka DaniZajmoviDepozitiISlUnGrupe { get; set; }
        public Stavka UlaganjaUUdjeleDrustavaPovSudjelujucimInteresom { get; set; }
        public Stavka UlaganjaUOstaleVrijednosnePapireDrustavaPovSudjelujucimInteresom { get; set; }
        public Stavka DaniZajmoviDepozitiISlDrustavaPovSudjelujucimInteresom { get; set; }
        public Stavka UlaganjaUVrijednosnePapire { get; set; }
        public Stavka DaniZajmoviDepozitiISl { get; set; }
        public Stavka OstalaUlaganjaMetodomUdjela { get; set; }
        public Stavka OstalaFinancijskaImovina { get; set; }
        public Stavka Ukupno { get; set; }

        public virtual List<Entry> GetNotes()
        {
            return new List<Entry>()
            {
                new Paragraph(Note, Ukupno.TekucaGodina)
            };
        }

        public static Note Note = new Note()
        {
            Title = "Dugotrajna financijska imovina",
            Subtitle = "Bilješka br. 11",
            Info = "Dugotrajna financijska imovina sastoji se od dugoročnih ulaganja u udjele (dionice), ulaganja u vrijednosne papire, danih dugoročnih zajmova, depozita i sličnog, ostalih dugoročnih ulaganja koja se obračunavaju metodom udjela i ostalih ulaganja u dugotrajnu financijsku imovinu.",
            Value = "Dugotrajna financijska imovina na dan izvještavanja iznosila je {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo dugotrajnu financijsku imovinu."
        };
    }


    public class KratkotrajnaFinancijskaImovina : FinancijskaImovina
    {
        public override List<Entry> GetNotes()
        {
            return new List<Entry>()
            {
                new Paragraph(Note, Ukupno.TekucaGodina)
            };
        }

        public static new Note Note = new Note()
        {
            Title = "Kratkotrajna financijska imovina",
            Subtitle = "Bilješka br. 16",
            Info = "Kratkotrajnu financijsku imovinu sačinjavaju ulaganja u udjele / dionice i vrijednosne papire, dani zajmovi, depoziti i slično te ostala financijska imovina za koju se očekuje da će se pretvori u novčani oblik unutar jedne godine.",
            Value = "Kratkotrajna financijska imovina na dan izvještavanja iznosila je {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo kratkotrajnu financijsku imovinu."
        };
    }
}