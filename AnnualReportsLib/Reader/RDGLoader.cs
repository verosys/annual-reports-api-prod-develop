using System;
using AnnualReports.Model;
using AnnualReports.Utils;

namespace AnnualReports.Reader
{
    public class RDGLoader
    {
        public RDGLoader()
        {

        }

        public static RDG Load(int godina, Sheet sheet)
        {
            var rdg = new RDG();

            rdg.Godina = godina;

            rdg.PoslovniPrihodi.Godina = godina;
            rdg.PoslovniPrihodi.Ukupno = GetEntry(sheet, 8);
            rdg.PoslovniPrihodi.PrihodiOdProdajeSPodUnGrupe = GetEntry(sheet, 9);
            rdg.PoslovniPrihodi.PrihodiOdProdajeIzvanGrupe = GetEntry(sheet, 10);
            rdg.PoslovniPrihodi.PrihodiNaTemeljuUpotrebeVlastitihProizvoda = GetEntry(sheet, 11);
            rdg.PoslovniPrihodi.OstaliPoslovniPrihodiSPodUnGrupe = GetEntry(sheet, 12);
            rdg.PoslovniPrihodi.OstaliPoslovniPrihodiIzvanGrupe = GetEntry(sheet, 13);

            rdg.PoslovniRashodi.Godina = godina;
            rdg.PoslovniRashodi.Ukupno = GetEntry(sheet, 14);
            rdg.PoslovniRashodi.PromjeneVrijednostiZalihaProizUTijekuIGotProizvoda = GetEntry(sheet, 15);
            rdg.PoslovniRashodi.MaterijalniTroskovi.Godina = godina;
            rdg.PoslovniRashodi.MaterijalniTroskovi.Ukupno = GetEntry(sheet, 16);
            rdg.PoslovniRashodi.MaterijalniTroskovi.TroskoviSirovinaIMaterijala = GetEntry(sheet, 17);
            rdg.PoslovniRashodi.MaterijalniTroskovi.TroskoviProdaneRobe = GetEntry(sheet, 18);
            rdg.PoslovniRashodi.MaterijalniTroskovi.OstaliVanjskiTroskovi = GetEntry(sheet, 19);
            rdg.PoslovniRashodi.TroskoviOsoblja.Godina = godina;
            rdg.PoslovniRashodi.TroskoviOsoblja.Ukupno = GetEntry(sheet, 20);
            rdg.PoslovniRashodi.TroskoviOsoblja.NetoPlaceINadnice = GetEntry(sheet, 21);
            rdg.PoslovniRashodi.TroskoviOsoblja.TroskoviPorezaIDoprinosaIzPlaca = GetEntry(sheet, 22);
            rdg.PoslovniRashodi.TroskoviOsoblja.DoprinosiNaPlace = GetEntry(sheet, 23);

            rdg.PoslovniRashodi.Amortizacija = GetEntry(sheet, 24);
            rdg.PoslovniRashodi.OstaliTroskovi = GetEntry(sheet, 25);
            rdg.PoslovniRashodi.VrijednosnaUskladjenja.Godina = godina;
            rdg.PoslovniRashodi.VrijednosnaUskladjenja.Ukupno = GetEntry(sheet, 26);
            rdg.PoslovniRashodi.VrijednosnaUskladjenja.DugotrajneImovineOsimFinImovine = GetEntry(sheet, 27);
            rdg.PoslovniRashodi.VrijednosnaUskladjenja.KratkotrajneImovineOsimFinImovine = GetEntry(sheet, 28);

            rdg.PoslovniRashodi.Rezerviranja.Godina = godina;
            rdg.PoslovniRashodi.Rezerviranja.Ukupno = GetEntry(sheet, 29);
            rdg.PoslovniRashodi.Rezerviranja.RezerviranjaZaMirovineOtpremnineISlObveze = GetEntry(sheet, 30);
            rdg.PoslovniRashodi.Rezerviranja.RezerviranjaZaPorezneObveze = GetEntry(sheet, 31);
            rdg.PoslovniRashodi.Rezerviranja.RezerviranjaZaZapoceteSudskeSporove = GetEntry(sheet, 32);
            rdg.PoslovniRashodi.Rezerviranja.RezerviranjaZaTroskoveObnavljanjaPrirodnihBogatstava = GetEntry(sheet, 33);
            rdg.PoslovniRashodi.Rezerviranja.RezerviranjaZaTroskoveUJamstvenimRokovima = GetEntry(sheet, 34);
            rdg.PoslovniRashodi.Rezerviranja.DrugaRezerviranja = GetEntry(sheet, 35);

            rdg.PoslovniRashodi.OstaliPoslovniRashodi = GetEntry(sheet, 36);

            rdg.FinancijskiPrihodi.Godina = godina;
            rdg.FinancijskiPrihodi.Ukupno = GetEntry(sheet, 37);
            rdg.FinancijskiPrihodi.PrihodiOdUlaganjaUUdjelePodUnGrupe = GetEntry(sheet, 38);
            rdg.FinancijskiPrihodi.PrihodiOdUlaganjaUUdjeleDrusPovSudInteresima = GetEntry(sheet, 39);
            rdg.FinancijskiPrihodi.PrihodiOdOstDugotrajnihFinUlaganjaIZajmova = GetEntry(sheet, 40);
            rdg.FinancijskiPrihodi.OstaliPrihodiSOsnKamataIzOdnSPodUnGrupe = GetEntry(sheet, 41);
            rdg.FinancijskiPrihodi.TecajneRazlikeIOstFinPrihodiIzOdnSPodUnGrupe = GetEntry(sheet, 42);
            rdg.FinancijskiPrihodi.PrihodiOdOstDugotrajnihFinUlaganjaIZajmova = GetEntry(sheet, 43);
            rdg.FinancijskiPrihodi.OstaliPrigodiSOsnoveKamata = GetEntry(sheet, 44);
            rdg.FinancijskiPrihodi.TecajneRazlikeIOstFinPrihodi = GetEntry(sheet, 45);
            rdg.FinancijskiPrihodi.NerealiziraniDobiciOdFinImovine = GetEntry(sheet, 46);
            rdg.FinancijskiPrihodi.OstaliFinPrihodi = GetEntry(sheet, 47);

            rdg.FinancijskiRashodi.Godina = godina;
            rdg.FinancijskiRashodi.Ukupno = GetEntry(sheet, 48);
            rdg.FinancijskiRashodi.RashodiSOsnoveKamataISlRashodiSPodUnGrupe = GetEntry(sheet, 49);
            rdg.FinancijskiRashodi.TecajneRazlikeIDrugiRashodiSPodUnGrupe = GetEntry(sheet, 50);
            rdg.FinancijskiRashodi.RashodiSOsnoveKamataISlRashodi = GetEntry(sheet, 51);
            rdg.FinancijskiRashodi.TecajneRazlikeIDrRashodi = GetEntry(sheet, 52);
            rdg.FinancijskiRashodi.NerealiziraniGubiciOdFinImovine = GetEntry(sheet, 53);
            rdg.FinancijskiRashodi.VrijednosnaUskladjenjaFinImovine = GetEntry(sheet, 54);
            rdg.FinancijskiRashodi.OstaliFinRashodi = GetEntry(sheet, 55);

            rdg.UdioUDobitiOdDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 56);
            rdg.UdioUDobitiOdZajednickihPothvata = GetEntry(sheet, 57);
            rdg.UdioUGubitkuOdDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 58);
            rdg.UdioUGubitkuOdZajednickihPothvata = GetEntry(sheet, 59);
            rdg.UkupniPrihodi = GetEntry(sheet, 60);
            rdg.UkupniRashodi = GetEntry(sheet, 61);

            rdg.DobitIliGubitakPrijeOporezivanja.Ukupno = GetEntry(sheet, 62);
            rdg.DobitIliGubitakPrijeOporezivanja.DobitPrijeOporezivanja = GetEntry(sheet, 63);
            rdg.DobitIliGubitakPrijeOporezivanja.GubitakPrijeOporezivanja = GetEntry(sheet, 64);

            rdg.PorezNaDobit = GetEntry(sheet, 65);

            rdg.DobitIliGubitakRazdoblja.Ukupno = GetEntry(sheet, 66);
            rdg.DobitIliGubitakRazdoblja.DobitRazdoblja = GetEntry(sheet, 67);
            rdg.DobitIliGubitakRazdoblja.GubitakRazdoblja = GetEntry(sheet, 68);

            rdg.PrekinutoPoslovanje.DobitIliGubitakPrekinutogPoslPrijeOporezivanja.Ukupno = GetEntry(sheet, 70);
            rdg.PrekinutoPoslovanje.DobitIliGubitakPrekinutogPoslPrijeOporezivanja.DobitPrijeOporezivanja = GetEntry(sheet, 71);
            rdg.PrekinutoPoslovanje.DobitIliGubitakPrekinutogPoslPrijeOporezivanja.GubitakPrijeOporezivanja = GetEntry(sheet, 72);
            rdg.PrekinutoPoslovanje.PorezNaDobitPrekinutogPoslovanja.Ukupno = GetEntry(sheet, 73);
            rdg.PrekinutoPoslovanje.PorezNaDobitPrekinutogPoslovanja.DobitRazdoblja = GetEntry(sheet, 74);
            rdg.PrekinutoPoslovanje.PorezNaDobitPrekinutogPoslovanja.DobitRazdoblja = GetEntry(sheet, 75);

            rdg.UkupnoPoslovanje.DobitIliGubitakPrijeOporezivanja.Ukupno = GetEntry(sheet, 77);
            rdg.UkupnoPoslovanje.DobitIliGubitakPrijeOporezivanja.DobitPrijeOporezivanja = GetEntry(sheet, 78);
            rdg.UkupnoPoslovanje.DobitIliGubitakPrijeOporezivanja.GubitakPrijeOporezivanja = GetEntry(sheet, 79);
            rdg.UkupnoPoslovanje.PorezNaDobit = GetEntry(sheet, 80);
            rdg.UkupnoPoslovanje.DobitIliGubitakRazdoblja.Ukupno = GetEntry(sheet, 81);
            rdg.UkupnoPoslovanje.DobitIliGubitakRazdoblja.DobitRazdoblja = GetEntry(sheet, 82);
            rdg.UkupnoPoslovanje.DobitIliGubitakRazdoblja.GubitakRazdoblja = GetEntry(sheet, 83);

            rdg.DodatakRDGu.Ukupo = GetEntry(sheet, 85);
            rdg.DodatakRDGu.PripisanaImateljimaKapitalaMatice = GetEntry(sheet, 86);
            rdg.DodatakRDGu.PripisanaManjinskomInteresu = GetEntry(sheet, 87);

            rdg.IzvjestajOOstalojSveobDobiti.DobitIliGubitakRazdoblja = GetEntry(sheet, 89);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.Ukupno = GetEntry(sheet, 90);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.TecajneRazlikeIzPreracunaInozPosl = GetEntry(sheet, 91);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.PromjeneRevalorizacijskihRezerviDugImovine = GetEntry(sheet, 92);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.DobGubSOsnoveNaknadnogVrednovanjaFinImovine = GetEntry(sheet, 93);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.DobGubSOsnoveUcinkoviteZastiteNovcanihTokova = GetEntry(sheet, 94);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.DobGubSOsnoveUcinkoviteZastiteNetoUlUIno = GetEntry(sheet, 95);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.UdioUOstalojSveobDobGubDrustavaPovSudjInt = GetEntry(sheet, 96);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.AktuarskiDobiciGubici = GetEntry(sheet, 97);
            rdg.IzvjestajOOstalojSveobDobiti.OstalaSveobDobitGubitakPrijePoreza.OstaleNeVlasnickePromjeneKapitala = GetEntry(sheet, 98);
            rdg.IzvjestajOOstalojSveobDobiti.PorezNaOstaluSveobuhvatnuDobitRazdoblja = GetEntry(sheet, 99);
            rdg.IzvjestajOOstalojSveobDobiti.NetoOstalaSveobuhvatnaDobitiLiGubitak = GetEntry(sheet, 100);
            rdg.IzvjestajOOstalojSveobDobiti.SveobuhvatnaDobitIliGubitakRazdoblja = GetEntry(sheet, 101);

            rdg.DodatakIzvjestajuOSveobDobiti.Ukupo = GetEntry(sheet, 103);
            rdg.DodatakIzvjestajuOSveobDobiti.PripisanaImateljimaKapitalaMatice = GetEntry(sheet, 104);
            rdg.DodatakIzvjestajuOSveobDobiti.PripisanaManjinskomInteresu = GetEntry(sheet, 105);

            //logger.Export("rdg_log.txt");
            //logger.Clear();

            return rdg;

        }
        //private static Logger logger = new Logger();

        private static Stavka GetEntry(Sheet sheet, int row)
        {
            row--;
            string label = sheet.Table[row, 0].Value.ToString();
            string aop = (sheet.Table[row, 6].Value ?? String.Empty).ToString().PadLeft(3, '0');
            string redniBroj = (string)(sheet.Table[row, 7].Value ?? String.Empty);
            double proslaGodina = (double)(sheet.Table[row, 8].Value ?? 0.0);
            double tekucaGodina = (double)(sheet.Table[row, 9].Value ?? 0.0);

            label = label.Trim().TrimStart(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }).TrimStart('.').Trim();
            
            //logger.Append(String.Format("{0} {1}  {2}  {3}  {4}", label, aop, redniBroj, proslaGodina, tekucaGodina));

            var entry = new Stavka
            {
                Label = label,
                AOP = aop,
                RedniBroj = redniBroj,
                ProslaGodina = Convert.ToDecimal(proslaGodina),
                TekucaGodina = Convert.ToDecimal(tekucaGodina)
            };

            return entry;
        }
    }
}