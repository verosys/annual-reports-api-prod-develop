using System;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.IO;
using AnnualReports.Reader;
using AnnualReports.Document;
using System.Collections.Generic;
using AnnualReports.Utils;

namespace AnnualReports.Model
{
    public class GodisnjiIzvjestaj
    {
        public bool UDobitku => Bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.DobitPoslovneGodine.TekucaGodina > 0;

        public int Godina { get; set; }
        public int ProslaGodina { get { return Godina - 1; } }

        public Obveznik Obveznik { get; set; }

        public String ProsjekBrojaZaposlenihPrethodnaGodina { get; set; }
        public String ProsjekBrojaZaposlenihTekucaGodina { get; set; }

        public String BrojZaposlenihPremaSatimaRadaPrethGodina { get; set; }
        public String BrojZaposlenihPremaSatimaRadaTekucaGodina { get; set; }

        public String BrojMjeseciPoslovanjaPrethodnaGodina { get; set; }
        public String BrojMjeseciPoslovanjaTekucaGodina { get; set; }

        public Bilanca Bilanca { get; set; }
        public RDG RDG { get; set; }

        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }

        public GodisnjiIzvjestaj()
        {
            Godina = DateTime.Now.Year;
        }

        public static GodisnjiIzvjestaj LoadFromGFI(Stream stream)
        {
            AnnualReports.Reader.Reader.Init();

            var reader = new AnnualReports.Reader.Reader();
            DataDocument document = reader.Read(stream);

            GodisnjiIzvjestaj izvjestaj = OsnovniPodaciLoader.Load(document.Sheets["osnovnipodaci"]);
            izvjestaj.Bilanca = BilancaLoader.Load(izvjestaj.Godina, document.Sheets["bilanca"]);
            izvjestaj.RDG = RDGLoader.Load(izvjestaj.Godina, document.Sheets["rdg"]);

            return izvjestaj;
        }

        public NotesDocument GetNotesDocument(string xmlString)
        {
            var document = new NotesDocument();

            document.Header.LineI = Obveznik.Naziv;
            document.Header.LineII = Obveznik.NazivGradaIliOpcine + "," + Obveznik.UlicaIKucniBroj;

            document.TitleI = "Bilješke uz financijske izvještaje poduzetnika";
            document.TitleII = String.Format("za razdoblje {0:dd.MM.yyyy.} do {1:dd.MM.yyyy.} godine", this.DatumOd, this.DatumDo);

            var SectionI = new Section("I. INFORMACIJE O DRUŠTVU", Obveznik.GetNotes());
            document.Sections.Add(SectionI);

            document.Sections.AddRange(GetStaticSections(xmlString));

            var SectionV = new Section("V. RAČUN DOBITI I GUBITKA", RDG.GetNotes());
            document.Sections.Add(SectionV);

            var SectionVI = new Section("VI. BILANCA", Bilanca.GetNotes());
            document.Sections.Add(SectionVI);

            var endNote = new Paragraph()
            {
                Title = "Objava financijskih izvještaja",
                Text = String.Format("Uprava društva je svojom odlukom i ovjerom financijskih izvještaja prihvatila financijske izvještaje za {0}. godinu i odobrila njihovu objavu.", Godina)
            };
            var SectionVII = new Section("VII. PRIHVAĆANJE I OBJAVA FINANCIJSKIH IZVJEŠTAJA OD STRANE UPRAVE DRUŠTVA", new List<Entry>() { endNote });

            document.Sections.Add(SectionVII);

            document.Footer.Left = new Paragraph() { Title = "Voditelj računovodstva:", Text = Obveznik.VoditeljRacunovodstva };
            document.Footer.Right = new Paragraph() { Title = "Direktor", Text = Obveznik.OvlastenaOsoba };
            return document;
        }

        public StatementDocument GetGfiDefinitionDocument()
        {
            var document = new StatementDocument();

            document.Header = String.Format("Na temelju odredbi Zakona o trgovačkim društvima, odredbi Zakona o računovodstvu i društvenog ugovora, skupština društva {0} donijela je dana {1:dd.MM.yyyy.}", Obveznik.Naziv, DateTime.Now);
            document.TitleI = "ODLUKU";
            document.TitleII = String.Format("o utvrđivanju godišnjeg financijskog izvještaja za {0}. godinu", Godina);

            document.Paragraphs.Add(new Paragraph()
            {
                Title = "Točka I.",
                Text = String.Format("Uprava društva, sukladno propisima za {0}. godinu, sastavila je i skupštini društva predočila na prihvaćanje sljedeće temeljne financijske i porezne izvještaje:", Godina)
            });

            document.Paragraphs.Add(new Paragraph() { Text = "1.Bilanca" });
            document.Paragraphs.Add(new Paragraph() { Text = "2.Račun dobiti i gubitka" });
            document.Paragraphs.Add(new Paragraph() { Text = "3. Bilješke uz temeljne financijske izvještaje" });
            document.Paragraphs.Add(new Paragraph() { Text = String.Format("4.Prijava poreza na dobit za {0}.godinu(obrazac PD)", Godina) });
            document.Paragraphs.Add(new Paragraph() { Text = "5.Obrazac GFI - POD za potrebe javne objave." });

            document.Paragraphs.Add(new Paragraph()
            {
                Title = "Točka II.",
                Text = String.Format("Utvrđuje se račun dobiti i gubitka za {0}. godinu u kojem iskazana svota dobiti nakon oporezivanja iznosi {1:N2} kn.\nBilanca na dan {2:dd.MM.yyyy.} godine iskazuje zbroj aktive u svoti {3:N2} kn.", Godina, RDG.DobitIliGubitakRazdoblja.DobitRazdoblja.TekucaGodina, DatumDo, Bilanca.Aktiva.UkupnoAktiva.TekucaGodina)
            });

            document.FooterI = "Predsjednik skupštine";
            document.FooterII = String.Format("{0}", Obveznik.OvlastenaOsoba);

            return document;
        }

        public StatementDocument GetProfitLossDistributionDocument(ZakljucakGodine.VrstaDobiti vrstaDobiti)
        {
            ZakljucakGodine zakljucakGodine;

            if (Obveznik.SifraVrstePoslovnogSubjekta.Trim() == "81")
                zakljucakGodine = new ZakljucakGodineJDOO(this);
            else
                zakljucakGodine = new ZakljucakGodineDOO(this);

            zakljucakGodine.Dobit = vrstaDobiti;
            return zakljucakGodine.GetDocument();
        }

        private List<Section> GetStaticSections(string xmlString)
        {
            List<Section> sections = new List<Section>();

            //var utf8 = Encoding.GetEncoding("UTF-8");

            var document = ResourceHelper.GetNotesStaticText();

            var parameters = new Params { Year = Godina };

            foreach (var element in document.Root.Descendants("Sections").First().Descendants("Section"))
            {
                Entry entry = EntryFactory.CreateEntry(element, parameters);
                if (entry != null)
                    sections.Add((Section)entry);
            }

            return sections;
        }
    }
}