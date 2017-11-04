using System;
using System.Collections.Generic;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class Obveze
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka ObvezePremaPoduzetnicimaUnGrupe { get; set; }
        public Stavka ObvezeZaZajmoveDepoziteISlUnGrupe { get; set; }
        public Stavka ObvezePremaDrustvimaPovSudjelujucimInteresom { get; set; }
        public Stavka ObvezeZaZajmoveDepoziteISlDrustavaPovSudjelujucimInteresom { get; set; }
        public Stavka ObvezeZaZajmoveDepoziteISl { get; set; }
        public Stavka ObvezePremaBankamaIDrugimFinInstitucijama { get; set; }
        public Stavka ObvezeZaPredujmove { get; set; }
        public Stavka ObvezePremaDobavljacima { get; set; }
        public Stavka ObvezePoVrijednosnimPapirima { get; set; }
        public Stavka OstaleObveze { get; set; }
        public Stavka Ukupno { get; set; }

        //TODO: check with Vesna (aggregate field)
        public Stavka ObvezeRobaUkupno
        {
            get
            {
                return new Stavka()
                {
                    Label = "Obveze (roba)",
                    ProslaGodina = ObvezePremaPoduzetnicimaUnGrupe.ProslaGodina + ObvezePremaDrustvimaPovSudjelujucimInteresom.ProslaGodina,
                    TekucaGodina = ObvezePremaPoduzetnicimaUnGrupe.TekucaGodina + ObvezePremaDrustvimaPovSudjelujucimInteresom.TekucaGodina
                };
            }
        }
        //TODO: check with Vesna (aggregate field)
        public Stavka ObvezeZajmoviUkupno
        {
            get
            {
                return new Stavka()
                {
                    Label = "Obveze (zajmovi)",
                    ProslaGodina = ObvezeZaZajmoveDepoziteISl.ProslaGodina + ObvezeZaZajmoveDepoziteISlUnGrupe.ProslaGodina + ObvezeZaZajmoveDepoziteISlDrustavaPovSudjelujucimInteresom.ProslaGodina,
                    TekucaGodina = ObvezeZaZajmoveDepoziteISl.TekucaGodina + ObvezeZaZajmoveDepoziteISlUnGrupe.TekucaGodina + ObvezeZaZajmoveDepoziteISlDrustavaPovSudjelujucimInteresom.TekucaGodina
                };
            }
        }
    }
}