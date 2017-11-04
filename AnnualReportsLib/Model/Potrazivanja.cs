using System;
using AnnualReports.Document;
using System.Collections.Generic;

namespace AnnualReports.Model
{
    public class Potrazivanja
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        public Stavka PotrazivanjaOdPoduzetnikaUnutarGrupe { get; set; }
        public Stavka PotrazivanjaOdDrustavaPovSudjelujucimInteresom { get; set; }
        public Stavka PotrazivanjaOdKupaca { get; set; }
        public Stavka OstalaPotrazivanja { get; set; }
        public Stavka Ukupno { get; set; }

       
    }

  
}