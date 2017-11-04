using System.Collections.Generic;
using AnnualReports.Utils;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class RDG
    {
		public int Godina { get; set; }
		public int ProslaGodina { get { return Godina - 1; } }

        //POSLOVNI PRIHODI
        public PoslovniPrihodi PoslovniPrihodi { get; set; }

        //POSLOVNI RASHODI
        public PoslovniRashodi PoslovniRashodi { get; set; }

        public FinancijskiPrihodi FinancijskiPrihodi { get; set; }

        public FinancijskiRashodi FinancijskiRashodi { get; set; }

        public Stavka UdioUDobitiOdDrustavaPovSudjelujucimInteresom { get; set; }

        public Stavka UdioUDobitiOdZajednickihPothvata { get; set; }

        public Stavka UdioUGubitkuOdDrustavaPovSudjelujucimInteresom { get; set; }

        public Stavka UdioUGubitkuOdZajednickihPothvata { get; set; }

        //UKUPNI PRIHODI (AOP 125+154+173 + 174)
        public Stavka UkupniPrihodi { get; set; }

        //UKUPNI RASHODI (AOP 131+165+175 + 176)
        public Stavka UkupniRashodi { get; set; }

        public DobitIliGubitakPrijeOporezivanja DobitIliGubitakPrijeOporezivanja { get; set; }

        public Stavka PorezNaDobit { get; set; }

        public DobitIliGubitakRazdoblja DobitIliGubitakRazdoblja { get; set; }

        //PREKINUTO POSLOVANJE(popunjava poduzetnik obveznika MSFI-a samo ako ima prekinuto poslovanje)
        public PrekinutoPoslovanje PrekinutoPoslovanje { get; set; }

        //UKUPNO POSLOVANJE (popunjava samo poduzetnik obveznik MSFI-a koji ima prekinuto poslovanje)
        public UkupnoPoslovanje UkupnoPoslovanje { get; set; }

        //DODATAK RDG-u (popunjava poduzetnik koji sastavlja konsolidirani godišnji financijski izvještaj)    
        public DodatakIzvjODobiti DodatakRDGu { get; set; }

        //IZVJEŠTAJ O OSTALOJ SVEOBUHVATNOJ DOBITI (popunjava poduzetnik obveznik primjene MSFI-a)
        public IzvjestajOOstalojSveobDobiti IzvjestajOOstalojSveobDobiti { get; set; }

        //DODATAK Izvještaju o  ostaloj sveobuhvatnoj dobiti (popunjava poduzetnik koji sastavlja konsolidirani izvještaj)
        public DodatakIzvjODobiti DodatakIzvjestajuOSveobDobiti { get; set; }

        public RDG()
        {
            PoslovniPrihodi = new PoslovniPrihodi();
            PoslovniRashodi = new PoslovniRashodi();
            FinancijskiPrihodi = new FinancijskiPrihodi();
            FinancijskiRashodi = new FinancijskiRashodi();
            DobitIliGubitakPrijeOporezivanja = new DobitIliGubitakPrijeOporezivanja();
            DobitIliGubitakRazdoblja = new DobitIliGubitakRazdoblja();

            PrekinutoPoslovanje = new PrekinutoPoslovanje();
            UkupnoPoslovanje = new UkupnoPoslovanje();
            DodatakRDGu = new DodatakIzvjODobiti();
            IzvjestajOOstalojSveobDobiti = new IzvjestajOOstalojSveobDobiti();
            DodatakIzvjestajuOSveobDobiti = new DodatakIzvjODobiti();
        }

        public Table GetPorezNaDobitTable()
        {
            var array = new string[5, 4];
            array.SetRow(0, DobitIliGubitakPrijeOporezivanja.DobitPrijeOporezivanja.ToRow("Dobit prije oporezivanja"));
            array.SetRow(1, DobitIliGubitakPrijeOporezivanja.GubitakPrijeOporezivanja.ToRow("Gubitak prije oporezivanja"));
            array.SetRow(2, PorezNaDobit.ToRow("Porez na dobit"));
            array.SetRow(3, DobitIliGubitakRazdoblja.DobitRazdoblja.ToRow("Dobit razdoblja"));
            array.SetRow(4, DobitIliGubitakRazdoblja.GubitakRazdoblja.ToRow("Gubitak razdoblja"));

            return new Table() { Data = array, Header = Table.GetCommonHeader(Godina) };
        }

        public BarChartEntry GetPrihodiBarChart()
        {
            return new BarChartEntry(ProslaGodina.ToString(), Godina.ToString())
            {
                Title = "Usporedba s prethodnom godinom",
                Entries =new List<Charting.BarEntry>() { UkupniPrihodi.ToBarEntry("Ukupni prihodi") }
            };
        }

        public BarChartEntry GetRashodiBarChart()
        {
            return new BarChartEntry(ProslaGodina.ToString(), Godina.ToString())
            {
                Title = "Usporedba s prethodnom godinom",
                Entries = new List<Charting.BarEntry>() { UkupniRashodi.ToBarEntry("Ukupni rashodi") }
            };
        }

        public BarChartEntry GetPorezNaDobitBarChart()
        {
            return new BarChartEntry(ProslaGodina.ToString(), Godina.ToString())
            {
                Title = "Usporedba s prethodnom godinom",
                Entries = new List<Charting.BarEntry>()
                {
                    DobitIliGubitakPrijeOporezivanja.DobitPrijeOporezivanja.ToBarEntry("Dobit prije oporezivanja"),
                    DobitIliGubitakPrijeOporezivanja.GubitakPrijeOporezivanja.ToBarEntry("Gubitak prije oporezivanja"),
                    PorezNaDobit.ToBarEntry("Porez na dobit"),
                    DobitIliGubitakRazdoblja.DobitRazdoblja.ToBarEntry("Dobit razdoblja"),
                    DobitIliGubitakRazdoblja.GubitakRazdoblja.ToBarEntry("Gubitak razdoblja")
                }
            };
        }

        public List<Entry> GetNotes()
        {
            var entries = new List<Entry>();

            entries.Add(new Paragraph(PrihodiNote, UkupniPrihodi.TekucaGodina));
            entries.Add(GetPrihodiBarChart());
            entries.AddRange(PoslovniPrihodi.GetNotes());
            entries.AddRange(FinancijskiPrihodi.GetNotes());
            entries.Add((new Paragraph(RashodiNote, UkupniRashodi.TekucaGodina)));
            entries.Add(GetRashodiBarChart());
            entries.AddRange(PoslovniRashodi.GetNotes());
            entries.AddRange(FinancijskiRashodi.GetNotes());
            entries.Add(new Paragraph() { Title = PorezNaDobitNote.Title, Subtitle = PorezNaDobitNote.Subtitle, Text = PorezNaDobitNote.Info });
            entries.Add(GetPorezNaDobitTable());
            entries.Add(GetPorezNaDobitBarChart());

            return entries;
        }

        public static Note PrihodiNote = new Note()
        {
            Title = "PRIHODI",
            Subtitle = "Bilješka br. 1",
            Value = "Društvo je u poslovnoj godini ostvarilo ukupne prihode u visini od {0:N2} kn"
        };

        public static Note PorezNaDobitNote = new Note()
        {
            Title = "POREZ NA DOBIT",
            Subtitle = "Bilješka br. 7",
            Info = "Porez na dobit sukladno hrvatskom Zakonu o porezu na dobit izračunava se primjenom propisane stope na oporezivu osnovicu. Porezna osnovica uvećava se i umanjuje prema propisima navedenim u Zakonu o porezu na dobit.",
        };

        public static Note RashodiNote = new Note()
        {
            Title = "Rashodi",
            Subtitle = "Bilješka br. 4",
            Value = "Društvo je u poslovnoj godini ostvarilo ukupne rashode u visini od {0:N2} kn"
        };
    }

    public class DobitIliGubitakPrijeOporezivanja
    {
        public Stavka DobitPrijeOporezivanja { get; set; }
        public Stavka GubitakPrijeOporezivanja { get; set; }
        public Stavka Ukupno { get; set; }
    }

    public class DobitIliGubitakRazdoblja
    {
        public Stavka DobitRazdoblja { get; set; }
        public Stavka GubitakRazdoblja { get; set; }
        public Stavka Ukupno { get; set; }
    }

    public class PrekinutoPoslovanje
    {

        public DobitIliGubitakPrijeOporezivanja DobitIliGubitakPrekinutogPoslPrijeOporezivanja { get; set; }
        public DobitIliGubitakRazdoblja PorezNaDobitPrekinutogPoslovanja { get; set; }

        public PrekinutoPoslovanje()
        {
            DobitIliGubitakPrekinutogPoslPrijeOporezivanja = new DobitIliGubitakPrijeOporezivanja();
            PorezNaDobitPrekinutogPoslovanja = new DobitIliGubitakRazdoblja();
        }

    }

    public class UkupnoPoslovanje
    {
        public DobitIliGubitakPrijeOporezivanja DobitIliGubitakPrijeOporezivanja { get; set; }
        public Stavka PorezNaDobit { get; set; }
        public DobitIliGubitakRazdoblja DobitIliGubitakRazdoblja { get; set; }

        public UkupnoPoslovanje()
        {
            DobitIliGubitakRazdoblja = new DobitIliGubitakRazdoblja();
            DobitIliGubitakPrijeOporezivanja = new DobitIliGubitakPrijeOporezivanja();
        }

    }

    public class IzvjestajOOstalojSveobDobiti
    {
        public Stavka DobitIliGubitakRazdoblja { get; set; }
        public OstalaSveobDobitGubitakPrijePoreza OstalaSveobDobitGubitakPrijePoreza { get; set; }
        public Stavka PorezNaOstaluSveobuhvatnuDobitRazdoblja { get; set; }
        public Stavka NetoOstalaSveobuhvatnaDobitiLiGubitak { get; set; }
        public Stavka SveobuhvatnaDobitIliGubitakRazdoblja { get; set; }

        public IzvjestajOOstalojSveobDobiti()
        {
            OstalaSveobDobitGubitakPrijePoreza = new OstalaSveobDobitGubitakPrijePoreza();
        }
    }

    public class OstalaSveobDobitGubitakPrijePoreza
    {
        public Stavka TecajneRazlikeIzPreracunaInozPosl { get; set; }
        public Stavka PromjeneRevalorizacijskihRezerviDugImovine { get; set; }
        public Stavka DobGubSOsnoveNaknadnogVrednovanjaFinImovine { get; set; }
        public Stavka DobGubSOsnoveUcinkoviteZastiteNovcanihTokova { get; set; }
        public Stavka DobGubSOsnoveUcinkoviteZastiteNetoUlUIno { get; set; }
        public Stavka UdioUOstalojSveobDobGubDrustavaPovSudjInt { get; set; }
        public Stavka AktuarskiDobiciGubici { get; set; }
        public Stavka OstaleNeVlasnickePromjeneKapitala { get; set; }

        public Stavka Ukupno { get; set; }
    }

    public class DodatakIzvjODobiti
    {
        public Stavka PripisanaImateljimaKapitalaMatice { get; set; }
        public Stavka PripisanaManjinskomInteresu { get; set; }
        public Stavka Ukupo { get; set; }

        public bool Test()
        {
            return (Ukupo.ProslaGodina == PripisanaManjinskomInteresu.ProslaGodina + PripisanaManjinskomInteresu.ProslaGodina)
                && (Ukupo.TekucaGodina == PripisanaImateljimaKapitalaMatice.TekucaGodina + PripisanaImateljimaKapitalaMatice.TekucaGodina);
        }
    }
}