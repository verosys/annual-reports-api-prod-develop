using System;
using AnnualReports.Document;

namespace AnnualReports.Model
{

    public abstract class ZakljucakGodine
    {

        public VrstaDobiti Dobit { get; set; }

        internal bool uDobitku;

        internal decimal dobitTekuceGodine;
        internal decimal gubitakTekuceGodine;

        internal decimal zadrzanaDobitProslogRazdoblja;
        internal decimal preneseniGubitakProslogRazdoblja;

        internal string ovlastenaOsoba;

        internal string nazivObveznika;

        internal int godina;

        public enum VrstaDobiti
        {
            Zadrzana,
            Isplacena,
            DioZadrzanaDioIsplacena
        }

        internal abstract void GetDobit(StatementDocument document);
        internal abstract void GetGubitak(StatementDocument document);


        public StatementDocument GetDocument()
        {
            var document = new StatementDocument();

            document.Header = String.Format("Na temelju odredbi Zakona o trgovačkim društvima, odredbi Zakona o računovodstvu i društvenog ugovora, skupština društva {0} donijela je dana {1:dd.MM.yyyy.}", nazivObveznika, DateTime.Now);
            document.TitleI = "ODLUKU";
            if (uDobitku)
            {
                document.TitleII = String.Format("o raspodjeli i uporabi dobiti {0}. godine", godina);
            }
            else
            {
                document.TitleII = String.Format("o raspodjeli i pokriću gubitka {0}. godine", godina);
            }

            if (uDobitku)
            {
                GetDobit(document);
            }
            else
            {
                GetGubitak(document);
            }

            document.FooterI = "Predsjednik skupštine";
            document.FooterII = String.Format("{0}", ovlastenaOsoba);

            return document;
        }
    }
}