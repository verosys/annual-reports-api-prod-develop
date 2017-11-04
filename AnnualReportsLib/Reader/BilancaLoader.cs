using System;
using System.Text;
using System.IO;
using AnnualReports.Model;
using AnnualReports.Utils;

namespace AnnualReports.Reader
{
    public class BilancaLoader
    {
        public BilancaLoader()
        {
        }

        public static Bilanca Load(int godina, Sheet sheet)
        {
            var bilanca = new Bilanca();
            bilanca.Godina = godina;
            bilanca.Aktiva.PotrazivanjaZaUpisaniNeuplaceniKapital = GetEntry(sheet, 8);

            //dugotrajna/nematerijalna imovina

            bilanca.Aktiva.DugotrajnaImovina.Godina = godina;
            bilanca.Aktiva.DugotrajnaImovina.Ukupno = GetEntry(sheet, 10);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.Godina = godina;
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.Ukupno = GetEntry(sheet, 11);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.IzdaciZaRazvoj = GetEntry(sheet, 12);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.KoncesijePatentiLicencijeIDrugo = GetEntry(sheet, 13);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.Goodwill = GetEntry(sheet, 14);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.PredujmoviZaNabavuNematerijalneImovine = GetEntry(sheet, 15);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.NematerijalnaImovinaUPripremi = GetEntry(sheet, 16);
            bilanca.Aktiva.DugotrajnaImovina.NematerijalnaImovina.OstalaNematerijalnaImovina = GetEntry(sheet, 17);

            //dugotrajna/materijalna imovina
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.Godina = godina;
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.Ukupno = GetEntry(sheet, 18);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.Zemljiste = GetEntry(sheet, 19);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.GradjevinskiObjekti = GetEntry(sheet, 20);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.PostrojenjaIOprema = GetEntry(sheet, 21);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.AlatiPogonskiInventarTransportnaImovina = GetEntry(sheet, 22);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.BioloskaImovina = GetEntry(sheet, 23);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.PredujmoviZaMaterijalnuImovinu = GetEntry(sheet, 24);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.MaterijalnaImovinaUPripremi = GetEntry(sheet, 25);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.OstalaMaterijalnaImovina = GetEntry(sheet, 26);
            bilanca.Aktiva.DugotrajnaImovina.MaterijalnaImovina.UlaganjeUNekretnine = GetEntry(sheet, 27);

            //dugotrajna/financijska imovina
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.Godina = godina;
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.Ukupno = GetEntry(sheet, 28);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.UlaganjaUUdjeleUnGrupe = GetEntry(sheet, 29);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.UlaganjaUOstaleVrijednosnePapireUnGrupe = GetEntry(sheet, 30);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.DaniZajmoviDepozitiISlUnGrupe = GetEntry(sheet, 31);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.UlaganjaUUdjeleDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 32);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.UlaganjaUOstaleVrijednosnePapireDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 33);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.DaniZajmoviDepozitiISlDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 34);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.UlaganjaUVrijednosnePapire = GetEntry(sheet, 35);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.DaniZajmoviDepozitiISl = GetEntry(sheet, 36);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.OstalaUlaganjaMetodomUdjela = GetEntry(sheet, 37);
            bilanca.Aktiva.DugotrajnaImovina.FinancijskaImovina.OstalaFinancijskaImovina = GetEntry(sheet, 38);

            //dugotrajna/potrazivanja
            bilanca.Aktiva.DugotrajnaImovina.Potrazivanja.Godina = godina;
            bilanca.Aktiva.DugotrajnaImovina.Potrazivanja.Ukupno = GetEntry(sheet, 39);
            bilanca.Aktiva.DugotrajnaImovina.Potrazivanja.PotrazivanjaOdPoduzetnikaUnutarGrupe = GetEntry(sheet, 40);
            bilanca.Aktiva.DugotrajnaImovina.Potrazivanja.PotrazivanjaOdDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 41);
            bilanca.Aktiva.DugotrajnaImovina.Potrazivanja.PotrazivanjaOdKupaca = GetEntry(sheet, 42);
            bilanca.Aktiva.DugotrajnaImovina.Potrazivanja.OstalaPotrazivanja = GetEntry(sheet, 43);

            //dugotrajna/odgodjena porezna imovina
            bilanca.Aktiva.DugotrajnaImovina.OdgodjenaPoreznaImovina = GetEntry(sheet, 44);

            //kratkotrajna/zalihe
            bilanca.Aktiva.KratkotrajnaImovina.Godina = godina;
            bilanca.Aktiva.KratkotrajnaImovina.Ukupno = GetEntry(sheet, 45);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.Godina = godina;
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.Ukupno = GetEntry(sheet, 46);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.SirovineIMaterijal = GetEntry(sheet, 47);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.ProizvodnjaUTijeku = GetEntry(sheet, 48);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.GotoviProizvodi = GetEntry(sheet, 49);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.TrgovackaRoba = GetEntry(sheet, 50);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.PredujmoviZaZalihe = GetEntry(sheet, 51);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.DugotrajnaImovinaNamijenjenaProdaji = GetEntry(sheet, 52);
            bilanca.Aktiva.KratkotrajnaImovina.Zalihe.BioloskaImovina = GetEntry(sheet, 53);

            //kratkotrajna/potrazivanja
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.Godina = godina;
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.Ukupno = GetEntry(sheet, 54);
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.PotrazivanjaOdPoduzetnikaUnutarGrupe = GetEntry(sheet, 55);
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.PotrazivanjaOdDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 56);
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.PotrazivanjaOdKupaca = GetEntry(sheet, 57);
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.PotrazivanjaOdZaposlenikaIClanovaPoduzetnika = GetEntry(sheet, 58);
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.PotrazivanjaOdDrzaveIDrugihInstitucija = GetEntry(sheet, 59);
            bilanca.Aktiva.KratkotrajnaImovina.Potrazivanja.OstalaPotrazivanja = GetEntry(sheet, 60);

            //kratkotrajna/financijska imovina
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.Godina = godina;
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.Ukupno = GetEntry(sheet, 61);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.UlaganjaUUdjeleUnGrupe = GetEntry(sheet, 62);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.UlaganjaUOstaleVrijednosnePapireUnGrupe = GetEntry(sheet, 63);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.DaniZajmoviDepozitiISlUnGrupe = GetEntry(sheet, 64);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.UlaganjaUUdjeleDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 65);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.UlaganjaUOstaleVrijednosnePapireDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 66);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.DaniZajmoviDepozitiISlDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 67);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.UlaganjaUVrijednosnePapire = GetEntry(sheet, 68);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.DaniZajmoviDepozitiISl = GetEntry(sheet, 69);
            bilanca.Aktiva.KratkotrajnaImovina.FinancijskaImovina.OstalaFinancijskaImovina = GetEntry(sheet, 70);

            //kratkotrajna/novac u banci i blagajni
            bilanca.Aktiva.KratkotrajnaImovina.NovacUBanciIBlagajni = GetEntry(sheet, 71);
            bilanca.Aktiva.PlaceniTroskoviBuducegRazdobljaIObracunatiPrihodi = GetEntry(sheet, 72);
            bilanca.Aktiva.UkupnoAktiva = GetEntry(sheet, 73);
            bilanca.Aktiva.IzvanBilancniZapisiAktiva = GetEntry(sheet, 74);

            //PASIVA
            bilanca.Pasiva.KapitalIRezerve.Godina = godina;
            bilanca.Pasiva.KapitalIRezerve.Ukupno = GetEntry(sheet, 76);
            bilanca.Pasiva.KapitalIRezerve.TemeljniKapital = GetEntry(sheet, 77);
            bilanca.Pasiva.KapitalIRezerve.KapitalneRezerve = GetEntry(sheet, 78);

            //PASIVA/rezerve iz dobiti
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.Godina = godina;
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.Ukupno = GetEntry(sheet, 79);
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.ZakonskeRezerve = GetEntry(sheet, 80);
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.RezerveZaVlastiteDionice = GetEntry(sheet, 81);
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.VlastiteDioniceIUdjeli = GetEntry(sheet, 82);
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.StatutarneRezerve = GetEntry(sheet, 83);
            bilanca.Pasiva.KapitalIRezerve.RezerveIzDobiti.OstaleRezerve = GetEntry(sheet, 84);

            bilanca.Pasiva.KapitalIRezerve.RevalorizacijskeRezerve = GetEntry(sheet, 85);

            //PASIVA/Rezerve Fer Vrijednosti
            bilanca.Pasiva.KapitalIRezerve.RezerveFerVrijednosti.Godina = godina;
            bilanca.Pasiva.KapitalIRezerve.RezerveFerVrijednosti.Ukupno = GetEntry(sheet, 86);
            bilanca.Pasiva.KapitalIRezerve.RezerveFerVrijednosti.FerVrijednostFinImovineRaspoloziveZaProdaju = GetEntry(sheet, 87);
            bilanca.Pasiva.KapitalIRezerve.RezerveFerVrijednosti.UcinkovitiDioZastiteNovcanihTokova = GetEntry(sheet, 88);
            bilanca.Pasiva.KapitalIRezerve.RezerveFerVrijednosti.UcinkovitiDioZastiteNetoUlaganjaUIinozemstvu = GetEntry(sheet, 89);

            //PASIVA/zadrzana dobit ili preneseni dobitak
            bilanca.Pasiva.KapitalIRezerve.ZadrzanaDobitPreneseniGubitak.Godina = godina;
            bilanca.Pasiva.KapitalIRezerve.ZadrzanaDobitPreneseniGubitak.Ukupno = GetEntry(sheet, 90);
            bilanca.Pasiva.KapitalIRezerve.ZadrzanaDobitPreneseniGubitak.ZadrzanaDobit = GetEntry(sheet, 91);
            bilanca.Pasiva.KapitalIRezerve.ZadrzanaDobitPreneseniGubitak.PreneseniGubitak = GetEntry(sheet, 92);

            //PASIVA/dobit ili gubitak poslovne godine
            bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.Godina = godina;
            bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.Ukupno = GetEntry(sheet, 93);
            bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.DobitPoslovneGodine = GetEntry(sheet, 94);
            bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.GubitakPoslovneGodine = GetEntry(sheet, 95);

            bilanca.Pasiva.KapitalIRezerve.ManjinskiInteres = GetEntry(sheet, 96);

            //PASIVA/rezerviranja
            bilanca.Pasiva.Rezerviranja.Godina = godina;
            bilanca.Pasiva.Rezerviranja.Ukupno = GetEntry(sheet, 97);
            bilanca.Pasiva.Rezerviranja.RezerviranjaZaMirovineOtpremnineISlObveze = GetEntry(sheet, 98);
            bilanca.Pasiva.Rezerviranja.RezerviranjaZaPorezneObveze = GetEntry(sheet, 99);
            bilanca.Pasiva.Rezerviranja.RezerviranjaZaZapoceteSudskeSporove = GetEntry(sheet, 100);
            bilanca.Pasiva.Rezerviranja.RezerviranjaZaTroskoveObnavljanjaPrirodnihBogatstava = GetEntry(sheet, 101);
            bilanca.Pasiva.Rezerviranja.RezerviranjaZaTroskoveUJamstvenimRokovima = GetEntry(sheet, 102);
            bilanca.Pasiva.Rezerviranja.DrugaRezerviranja = GetEntry(sheet, 103);


            //PASIVA/dugorocne obveze
            bilanca.Pasiva.DugorocneObveze.Godina = godina;
            bilanca.Pasiva.DugorocneObveze.Ukupno = GetEntry(sheet, 104);
            bilanca.Pasiva.DugorocneObveze.ObvezePremaPoduzetnicimaUnGrupe = GetEntry(sheet, 105);
            bilanca.Pasiva.DugorocneObveze.ObvezeZaZajmoveDepoziteISlUnGrupe = GetEntry(sheet, 106);
            bilanca.Pasiva.DugorocneObveze.ObvezePremaDrustvimaPovSudjelujucimInteresom = GetEntry(sheet, 107);
            bilanca.Pasiva.DugorocneObveze.ObvezeZaZajmoveDepoziteISlDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 108);
            bilanca.Pasiva.DugorocneObveze.ObvezeZaZajmoveDepoziteISl = GetEntry(sheet, 109);
            bilanca.Pasiva.DugorocneObveze.ObvezePremaBankamaIDrugimFinInstitucijama = GetEntry(sheet, 110);
            bilanca.Pasiva.DugorocneObveze.ObvezeZaPredujmove = GetEntry(sheet, 111);
            bilanca.Pasiva.DugorocneObveze.ObvezePremaDobavljacima = GetEntry(sheet, 112);
            bilanca.Pasiva.DugorocneObveze.ObvezePoVrijednosnimPapirima = GetEntry(sheet, 113);
            bilanca.Pasiva.DugorocneObveze.OstaleObveze = GetEntry(sheet, 114);
            bilanca.Pasiva.DugorocneObveze.OdgodjenaPoreznaObveza = GetEntry(sheet, 115);

            //PASIVA/kratkorocne obveze
            bilanca.Pasiva.KratkorocneObveze.Godina = godina;
            bilanca.Pasiva.KratkorocneObveze.Ukupno = GetEntry(sheet, 116);
            bilanca.Pasiva.KratkorocneObveze.ObvezePremaPoduzetnicimaUnGrupe = GetEntry(sheet, 117);
            bilanca.Pasiva.KratkorocneObveze.ObvezeZaZajmoveDepoziteISlUnGrupe = GetEntry(sheet, 118);
            bilanca.Pasiva.KratkorocneObveze.ObvezePremaDrustvimaPovSudjelujucimInteresom = GetEntry(sheet, 119);
            bilanca.Pasiva.KratkorocneObveze.ObvezeZaZajmoveDepoziteISlDrustavaPovSudjelujucimInteresom = GetEntry(sheet, 120);
            bilanca.Pasiva.KratkorocneObveze.ObvezeZaZajmoveDepoziteISl = GetEntry(sheet, 121);
            bilanca.Pasiva.KratkorocneObveze.ObvezePremaBankamaIDrugimFinInstitucijama = GetEntry(sheet, 122);
            bilanca.Pasiva.KratkorocneObveze.ObvezeZaPredujmove = GetEntry(sheet, 123);
            bilanca.Pasiva.KratkorocneObveze.ObvezePremaDobavljacima = GetEntry(sheet, 124);
            bilanca.Pasiva.KratkorocneObveze.ObvezePoVrijednosnimPapirima = GetEntry(sheet, 125);
            bilanca.Pasiva.KratkorocneObveze.ObvezePremaZaposlenicima = GetEntry(sheet, 126);
            bilanca.Pasiva.KratkorocneObveze.ObvezeZaPorezeDoprinoseISlDavanja = GetEntry(sheet, 127);
            bilanca.Pasiva.KratkorocneObveze.ObvezeSOsnoveUdjelaURezultatu = GetEntry(sheet, 128);
            bilanca.Pasiva.KratkorocneObveze.ObvezePoOsnoviDugotrajneImovineNamjProdaji = GetEntry(sheet, 129);
            bilanca.Pasiva.KratkorocneObveze.OstaleObveze = GetEntry(sheet, 130);

            bilanca.Pasiva.OdgodjenoPlacanjeTroskovaIPrihodBuducegRazdoblja = GetEntry(sheet, 131);
            bilanca.Pasiva.UkupnoPasiva = GetEntry(sheet, 132);
            bilanca.Pasiva.IzvanBilancniZapisiPasiva = GetEntry(sheet, 133);

            //logger.Export("bilanca_log.txt");
            //logger.Clear();

            return bilanca;
        }

        //private static Logger logger = new Logger();

        private static Stavka GetEntry(Sheet sheet, int row)
        {
            row--;
            string label = sheet.Table[row, 0].Value.ToString();
            string aop = (sheet.Table[row, 6].Value ?? String.Empty).ToString().PadLeft(3,'0');
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