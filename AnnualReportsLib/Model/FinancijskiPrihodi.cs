using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class FinancijskiPrihodi
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka PrihodiOdUlaganjaUUdjelePodUnGrupe { get; set; }
        public Stavka PrihodiOdUlaganjaUUdjeleDrusPovSudInteresima { get; set; }
        public Stavka PrihodiOdOstDugotrajnihFinUlaganjaIZajmovaPodUnGrupe { get; set; }
        public Stavka OstaliPrihodiSOsnKamataIzOdnSPodUnGrupe { get; set; }
        public Stavka TecajneRazlikeIOstFinPrihodiIzOdnSPodUnGrupe { get; set; }
        public Stavka PrihodiOdOstDugotrajnihFinUlaganjaIZajmova { get; set; }
        public Stavka OstaliPrigodiSOsnoveKamata { get; set; }
        public Stavka TecajneRazlikeIOstFinPrihodi { get; set; }
        public Stavka NerealiziraniDobiciOdFinImovine { get; set; }
        public Stavka OstaliFinPrihodi { get; set; }
        public Stavka Ukupno { get; set; }

        public List<Entry> GetNotes()
        {

            var aggregate = PrihodiOdOstDugotrajnihFinUlaganjaIZajmova.TekucaGodina 
                                      + OstaliPrigodiSOsnoveKamata.TekucaGodina 
                                      + TecajneRazlikeIOstFinPrihodi.TekucaGodina;
                                                      
            return new List<Entry>()
            {
                new Paragraph(Note, Ukupno.TekucaGodina),
                new Paragraph(PrihodiOdUlaganjaUUdjeleDrustvaUnGrupeNote,PrihodiOdUlaganjaUUdjelePodUnGrupe.TekucaGodina ),
                new Paragraph(PrihodiOdUlaganjaUUdjeleDrustavaPovSudjInteresomNote, PrihodiOdUlaganjaUUdjeleDrusPovSudInteresima.TekucaGodina),
                new Paragraph(PrihodiOdOstDugotrajnihFinUlaganjaZajmovaNote, aggregate),
                new Paragraph(NerealiziraniDobiciNote, NerealiziraniDobiciOdFinImovine.TekucaGodina),
                new Paragraph(OstaliFinancijskiPrihodiNote, OstaliFinPrihodi.TekucaGodina)
            };
        }

        public static Note Note = new Note()
        {
            Title = "Financijski prihodi",
            Subtitle = "Bilješka br. 3",
            Info = "Financijski prihodi sastoje se od prihoda od ulaganja u dionice/udjele poduzetnika, kamata, tečajnih razlika i ostalih financijskih prihoda iz odnosa s poduzetnicima i financijskim institucijama.",
            Value = "Društvo je u poslovnoj godini ostvarilo financijske prihode u iznosu {0:N2} kn",
            Zero = "Društvo u poslovnoj godini nije ostvarilo financijske prihode."
        };

        public static Note PrihodiOdUlaganjaUUdjeleDrustvaUnGrupeNote = new Note()
        {
            Title = "Prihodi od ulaganja u udjele, kamata i tečajnih razlika iz odnosa s poduzetnicima unutar grupe",
            Subtitle = "Bilješka br. 3a",
            Info = "",
            Value = "Prihodi od ulaganja u udjele, kamata i tečajnih razlika iz odnosa s poduzetnicima unutar grupe na dan izvještavanja iznosili su {0:N2} kn",
            Zero = "Društvo u poslovnoj godini nije ostvarilo financijske prihode od ulaganja u udjele, kamata i tečajnih razlika iz odnosa s poduzetnicima unutar grupe."
        };

        public static Note PrihodiOdUlaganjaUUdjeleDrustavaPovSudjInteresomNote = new Note()
        {
            Title = "Prihodi od ulaganja u udjele društava povezanih sudjelujućim interesima",
            Subtitle = "Bilješka br. 3b",
            Info = "",
            Value = "Prihodi od ulaganja u udjele društava povezanih sudjelujućim interesima na dan izvještavanja iznosili su {0:N2} kn",
            Zero = "Društvo u poslovnoj godini nije ostvarilo financijske prihode od ulaganja u udjele društava povezanih sudjelujućim interesima."
        };

        public static Note PrihodiOdOstDugotrajnihFinUlaganjaZajmovaNote = new Note()
        {
            Title = "Prihodi od ostalih dugotrajnih financijskih ulaganja i zajmova, ostali prihodi od kamata i tečajne razlike",
            Subtitle = "Bilješka br. 3c",
            Info = "",
            Value = "Financijski prihodi koji su ostvareni iz poslovnog odnosa s poduzetnicima koji nisu unutar grupe te financijskih institucija, a koji se sastoje od prihoda od dugotrajnih financijskih ulaganja i zajmova, ostalih financijskih prihoda od kamata, tečajnih razlika i ostalih financijskih prihoda iznosili su {0:N2} kn.",
        };

        public static Note NerealiziraniDobiciNote = new Note()
        {
            Title = "Nerealizirani dobici od financijske imovine",
            Subtitle = "Bilješka br. 3d",
            Info = "",
            Value = "Vrijednost dionica Društva na dan izvještavanja iznosila je {0:N2} kn",
            Zero = "Društvo na dan izvještavanja nije posjedovalo dionice."
        };

        public static Note OstaliFinancijskiPrihodiNote = new Note()
        {
            Title = "Ostali financijski prihodi",
            Subtitle = "Bilješka br. 3e",
            Info = "Ostali financijski prihodi odnose se na financijske prihode koji su ostvareni od ulaganja u udjele i dionice društava s udjelom vlasništva manjim od 20% na rok kraći od godinu dana te drugih prihoda koji se ne odnose na kamate zajmova i tečajne razlike.",
            Value = "Društvo je na dan izvještavanja imalo ostalih financijskih prihoda u iznosu od {0:N2} kn.",
            Zero = "Društvo na dan izvještavanja nije imalo ostalih financijskih prihoda."
        };
    }
}