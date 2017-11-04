using System;
using AnnualReports.Model;
using System.Globalization;


namespace AnnualReports.Reader
{
    public class OsnovniPodaciLoader
    {
        private static string dateFormat = "dd.MM.yyyy";
        private static CultureInfo hr = new CultureInfo("hr-HR");

        public static GodisnjiIzvjestaj Load(Sheet sheet)
        {
            GodisnjiIzvjestaj izvjestaj = new GodisnjiIzvjestaj();

            var obveznik = new Obveznik();

            int A = 0, C = 2, D = 3, F = 5, H = 7, J = 9, K = 10, L = 11, M = 12;

            if (sheet.Table[3, C].Value != null)
                izvjestaj.DatumOd = DateTime.ParseExact(sheet.Table[3, C].Value.ToString(), dateFormat, hr);

            if (sheet.Table[3, F].Value != null)
                izvjestaj.DatumDo = DateTime.ParseExact(sheet.Table[3, F].Value.ToString(), dateFormat, hr);

            if (sheet.Table[11, F].Value != null)
                izvjestaj.Godina = Int32.Parse(sheet.Table[11, F].Value.ToString());

            if (sheet.Table[6, C].Value != null)
                obveznik.SifraVrstePoslovnogSubjekta = sheet.Table[6, C].Value.ToString();

            if (sheet.Table[6, D].Value != null)
                obveznik.NazivVrstePoslovnogSubjekta = sheet.Table[6, D].Value.ToString();

            if (sheet.Table[26, C].Value != null)
                obveznik.OIB = sheet.Table[26, C].Value.ToString();

            if (sheet.Table[26, H].Value != null)
                obveznik.MaticniBroj = sheet.Table[26, H].Value.ToString();

            if (sheet.Table[26, M].Value != null)
                obveznik.MaticniBrojSubjekta = sheet.Table[26, M].Value.ToString();

            if (sheet.Table[28, C].Value != null)
                obveznik.Naziv = sheet.Table[28, C].Value.ToString();

            if (sheet.Table[30, C].Value != null)
                obveznik.PostanskiBroj = sheet.Table[30, C].Value.ToString();

            if (sheet.Table[30, F].Value != null)
                obveznik.NazivNaselja = sheet.Table[30, F].Value.ToString();

            if (sheet.Table[32, C].Value != null)
                obveznik.UlicaIKucniBroj = sheet.Table[32, C].Value.ToString();

            if (sheet.Table[34, C].Value != null)
                obveznik.Email = sheet.Table[34, C].Value.ToString();
            if (sheet.Table[34, L].Value != null)
                obveznik.Telefon = sheet.Table[34, L].Value.ToString();
            if (sheet.Table[36, C].Value != null)
                obveznik.InternetAdresa = sheet.Table[36, C].Value.ToString();

            if (sheet.Table[38, C].Value != null)
                obveznik.SifraGradaIliOpcine = sheet.Table[38, C].Value.ToString();

            if (sheet.Table[38, D].Value != null)
                obveznik.NazivGradaIliOpcine = sheet.Table[38, D].Value.ToString();

            if (sheet.Table[38, J].Value != null)
                obveznik.SifraZupanije = sheet.Table[38, J].Value.ToString();

            if (sheet.Table[38, K].Value != null)
                obveznik.NazivZupanije = sheet.Table[38, K].Value.ToString();

            if (sheet.Table[41, C].Value != null)
                obveznik.NKDSifra = sheet.Table[41, C].Value.ToString();

            if (sheet.Table[41, D].Value != null)
                obveznik.NKDOpis = sheet.Table[41, D].Value.ToString();

            if (sheet.Table[43, C].Value != null)
                obveznik.StatusAutonomnosti = sheet.Table[43, C].Value.ToString();

            if (sheet.Table[43, D].Value != null)
                obveznik.OpisStatusaAutonomnosti = sheet.Table[43, D].Value.ToString();

            if (sheet.Table[45, C].Value != null)
                obveznik.SifraZemljeSjedistaNadredjenogDrustva = sheet.Table[45, C].Value.ToString();
            if (sheet.Table[45, D].Value != null)
                obveznik.NazivZemljeSjedistaNadredjenogDrustva = sheet.Table[45, D].Value.ToString();
            if (sheet.Table[45, M].Value != null)
                obveznik.MaticniBrojNadredjenogDrustva = sheet.Table[45, M].Value.ToString();

            if (sheet.Table[49, C].Value != null)
                obveznik.OznakaVelicine = sheet.Table[49, C].Value.ToString();
            if (sheet.Table[49, D].Value != null)
                obveznik.OpisVelicine = sheet.Table[49, D].Value.ToString();

            if (sheet.Table[51, C].Value != null)
                obveznik.OznakaVlasnistva = sheet.Table[51, C].Value.ToString();
            if (sheet.Table[51, D].Value != null)
                obveznik.OpisVlasnistva = sheet.Table[51, D].Value.ToString();

            if (sheet.Table[53, C].Value != null)
                obveznik.PorijekloKapitalaDomaci = sheet.Table[53, C].Value.ToString();
            if (sheet.Table[53, F].Value != null)
                obveznik.PorijekloKapitalaStrano = sheet.Table[53, F].Value.ToString();

			if (sheet.Table[74, A].Value != null)
				obveznik.OvlastenaOsoba = sheet.Table[74, A].Value.ToString();
            if (sheet.Table[67,C].Value != null) 
                obveznik.VoditeljRacunovodstva = sheet.Table[67,C].Value.ToString();


			izvjestaj.Obveznik = obveznik;

            if (sheet.Table[55, C].Value != null)
                izvjestaj.ProsjekBrojaZaposlenihPrethodnaGodina = sheet.Table[55, C].Value.ToString();
            if (sheet.Table[55, F].Value != null)
                izvjestaj.ProsjekBrojaZaposlenihTekucaGodina = sheet.Table[55, F].Value.ToString();
            if (sheet.Table[57, C].Value != null)
                izvjestaj.BrojZaposlenihPremaSatimaRadaPrethGodina = sheet.Table[57, C].Value.ToString();
            if (sheet.Table[57, F].Value != null)
                izvjestaj.BrojZaposlenihPremaSatimaRadaTekucaGodina = sheet.Table[57, F].Value.ToString();
            if (sheet.Table[59, C].Value != null)
                izvjestaj.BrojMjeseciPoslovanjaPrethodnaGodina = sheet.Table[59, C].Value.ToString();
            if (sheet.Table[59, F].Value != null)
                izvjestaj.BrojMjeseciPoslovanjaTekucaGodina = sheet.Table[59, F].Value.ToString();


            return izvjestaj;
        }

    }
}
