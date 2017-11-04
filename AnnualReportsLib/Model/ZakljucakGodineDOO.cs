using System;
using AnnualReports.Document;

namespace AnnualReports.Model
{
    public class ZakljucakGodineDOO : ZakljucakGodine
    {

        public ZakljucakGodineDOO(GodisnjiIzvjestaj godisnjiIzvjestaj)
        {
            uDobitku = godisnjiIzvjestaj.UDobitku;
            godina = godisnjiIzvjestaj.Godina;
            ovlastenaOsoba = godisnjiIzvjestaj.Obveznik.OvlastenaOsoba;
            nazivObveznika = godisnjiIzvjestaj.Obveznik.Naziv;

            dobitTekuceGodine = godisnjiIzvjestaj.Bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.DobitPoslovneGodine.TekucaGodina;
            gubitakTekuceGodine = godisnjiIzvjestaj.Bilanca.Pasiva.KapitalIRezerve.DobitIliGubitakPoslovneGodine.GubitakPoslovneGodine.TekucaGodina;

            zadrzanaDobitProslogRazdoblja = godisnjiIzvjestaj.Bilanca.Pasiva.KapitalIRezerve.ZadrzanaDobitPreneseniGubitak.ZadrzanaDobit.TekucaGodina;
            preneseniGubitakProslogRazdoblja = godisnjiIzvjestaj.Bilanca.Pasiva.KapitalIRezerve.ZadrzanaDobitPreneseniGubitak.PreneseniGubitak.TekucaGodina;
        }

        override internal void GetGubitak(StatementDocument document)
        {

            if (gubitakTekuceGodine > 0 && zadrzanaDobitProslogRazdoblja == 0)
            {
                //sav gubitak se prenosi

                document.Paragraphs.Add(new Paragraph()
                {
                    Title = "Točka I.",
                    Text = String.Format("Prema usvojenim financijskim izvještajima gubitak poslovne godine iznosi {0:N2}", gubitakTekuceGodine)
                });
                document.Paragraphs.Add(new Paragraph()
                {
                    Text = "Gubitak poslovne godine raspoređuje se na preneseni gubitak"
                });
                if (preneseniGubitakProslogRazdoblja > 0)
                {
                    document.Paragraphs.Add(new Paragraph()
                    {
                        Text = String.Format("Preneseni gubitak iz prethodnih godina iznosi {0:N2} kn te s rasporedom gubitka iz ove poslovne godine čini ukupnu svotu gubitka za prijenos.", preneseniGubitakProslogRazdoblja)
                    });
                }
                else
                {
                    document.Paragraphs.Add(new Paragraph()
                    {
                        Text = "Zadržane dobiti kao niti prenesenog gubitka iz prethodnih razdoblja nema pa gubitak poslovne godine čini ukupnu svotu gubitka za prijenos."
                    });
                }
            }
            else if (gubitakTekuceGodine > 0 && zadrzanaDobitProslogRazdoblja > 0)
            {
                //nadoplatit ce se iz zadrzane dobiti, ostatak se prenosi
                document.Paragraphs.Add(new Paragraph()
                {
                    Title = "Točka I.",
                    Text = String.Format("Prema usvojenim financijskim izvještajima gubitak poslovne godine iznosi {0:N2}", gubitakTekuceGodine)
                });
                document.Paragraphs.Add(new Paragraph()
                {
                    Text = String.Format("Zadržana dobit iz prethodnih razdoblja iznosi {0:N2} kn te će se iskoristiti za djelomično ili potpuno pokriće iznosa gubitka poslovne godine. Preostali iznos zadržane dobiti odnosno gubitka poslovne godine se zadržava odnosno prenosi u buduća razdoblja.", zadrzanaDobitProslogRazdoblja)
                });
                document.Paragraphs.Add(new Paragraph()
                {
                    Text = String.Format("Preneseni gubitak iz prethodnih godina iznosi {0:N2} kn te s rasporedom gubitka iz ove poslovne godine čini ukupnu svotu gubitka za prijenos.", preneseniGubitakProslogRazdoblja)
                });
            }

        }

        override internal void GetDobit(StatementDocument document)
        {
            if (preneseniGubitakProslogRazdoblja > 0)
            {
                document.Paragraphs.Add(new Paragraph()
                {
                    Title = "Točka I.",
                    Text = String.Format("Prema usvojenim financijskim izvještajima dobit poslovne godine iznosi {0:N2} kn", dobitTekuceGodine)
                });

                document.Paragraphs.Add(new Paragraph()
                {
                    Text = String.Format("Preneseni gubitak iz prethodnih razdoblja iznosi {0:N2} kn. Dobit poslovne godine nakon oporezivanja raspoređuje se na djelomično ili potpuno pokriće gubitka prethodnih razdoblja.", preneseniGubitakProslogRazdoblja)
                });

                if (dobitTekuceGodine > preneseniGubitakProslogRazdoblja)
                {
                    switch (Dobit)
                    {
                        case VrstaDobiti.Zadrzana:
                            document.Paragraphs.Add(new Paragraph()
                            {
                                Title = "Točka II.",
                                Text = "Preostali iznos nakon pokrića gubitka u potpunosti se raspoređuje na zadržanu dobit."
                            });

                            document.Paragraphs.Add(new Paragraph()
                            {
                                Text = "Dobit se smatra zadržanom sve dok se zahtjevom pojedinog člana društva, a po ispunjenim uvjetima(likvidnost društva), ne zatraži i ne izvrši isplata(djelomična ili cjelokupna) na način definiran u zahtjevu člana društva. Članovi društva koji su dobit predujmljivali moraju donijeti odluku o pokriću tog predujma."
                            });
                            break;
                        case VrstaDobiti.Isplacena:
                            document.Paragraphs.Add(new Paragraph()
                            {
                                Title = "Točka II.",
                                Text = "Preostali iznos nakon pokrića gubitka se u potpunosti se raspoređuje na pokriće isplaćenih akontacija dobiti i isplatu."
                            });

                            document.Paragraphs.Add(new Paragraph()
                            {
                                Text = "Udio pojedinog člana društva u dobiti utvrđuje se prema omjeru udjela o temeljnom kapitalu. Iznimno, ako se financijsko stanje društva pogorša do dana isplate dobiti, članovi društva neće zahtijevati isplatu dobiti nego će se suzdržati od isplate dok takvo stanje traje. O takvom stanju Uprava društva pravodobno pisano izvještava članove društva uz opis nastalih događaja i prijedlog za rješenje."
                            });
                            break;
                        case VrstaDobiti.DioZadrzanaDioIsplacena:
                            document.Paragraphs.Add(new Paragraph()
                            {
                                Title = "Točka II.",
                                Text = "Preostali iznos nakon pokrića gubitka se dijelom raspoređuje na zadržanu dobit, a dijelom na pokriće isplaćenih akontacija dobiti i isplatu."
                            });
                            document.Paragraphs.Add(new Paragraph()
                            {
                                Text = "Dobit se smatra zadržanom sve dok se zahtjevom pojedinog člana društva, a po ispunjenim uvjetima (likvidnost društva),  ne zatraži i ne izvrši isplata (djelomična ili cjelokupna) na način definiran u zahtjevu člana društva. Članovi društva koji su dobit predujmljivali moraju donijeti odluku o pokriću tog predujma.Udio pojedinog člana društva u dobiti utvrđuje se prema omjeru udjela o temeljnom kapitalu. Iznimno, ako se financijsko stanje društva pogorša do dana isplate dobiti, članovi društva neće zahtijevati isplatu dobiti nego će se suzdržati od isplate dok takvo stanje traje. O takvom stanju Uprava društva pravodobno pisano izvještava članove društva uz opis nastalih događaja i prijedlog za rješenje."
                            });
                            break;
                    }
                }
                else if (dobitTekuceGodine < preneseniGubitakProslogRazdoblja)
                {
                    document.Paragraphs.Add(new Paragraph()
                    {
                        Text = "Preostali iznos gubitka poslovne godine se prenosi u buduća razdoblja."
                    });
                }

            }

            else if (preneseniGubitakProslogRazdoblja == 0)
            {
                //dobit isplacena/zadrzana/dio isplacena dio zadrzana

                document.Paragraphs.Add(new Paragraph()
                {
                    Title = "Točka I.",
                    Text = String.Format("Prema usvojenim financijskim izvještajima dobit poslovne godine iznosi {0:N2} kn", dobitTekuceGodine)
                });

                if (zadrzanaDobitProslogRazdoblja > 0)
                {
                    document.Paragraphs.Add(new Paragraph()
                    {
                        Text = String.Format("Zadržana dobit iz prethodnih razdoblja iznosi {0:N2} kn te s rasporedom dobiti iz ove poslovne godine čini ukupnu svotu dobiti.", zadrzanaDobitProslogRazdoblja)
                    });
                }

                switch (Dobit)
                {
                    case VrstaDobiti.Zadrzana:
                        document.Paragraphs.Add(new Paragraph()
                        {
                            Title = "Točka II.",
                            Text = "Ukupna dobit u potpunosti se raspoređuje na zadržanu dobit."
                        });

                        document.Paragraphs.Add(new Paragraph()
                        {
                            Text = "Dobit se smatra zadržanom sve dok se zahtjevom pojedinog člana društva, a po ispunjenim uvjetima(likvidnost društva), ne zatraži i ne izvrši isplata(djelomična ili cjelokupna) na način definiran u zahtjevu člana društva. Članovi društva koji su dobit predujmljivali moraju donijeti odluku o pokriću tog predujma."
                        });
                        break;
                    case VrstaDobiti.Isplacena:
                        document.Paragraphs.Add(new Paragraph()
                        {
                            Title = "Točka II.",
                            Text = "Ukupna dobit se u potpunosti se raspoređuje na pokriće isplaćenih akontacija dobiti i isplatu."
                        });

                        document.Paragraphs.Add(new Paragraph()
                        {
                            Text = "Udio pojedinog člana društva u dobiti utvrđuje se prema omjeru udjela o temeljnom kapitalu. Iznimno, ako se financijsko stanje društva pogorša do dana isplate dobiti, članovi društva neće zahtijevati isplatu dobiti nego će se suzdržati od isplate dok takvo stanje traje. O takvom stanju Uprava društva pravodobno pisano izvještava članove društva uz opis nastalih događaja i prijedlog za rješenje."
                        });
                        break;
                    case VrstaDobiti.DioZadrzanaDioIsplacena:
                        document.Paragraphs.Add(new Paragraph()
                        {
                            Title = "Točka II.",
                            Text = "Ukupna dobit se dijelom raspoređuje na zadržanu dobit, a dijelom na pokriće isplaćenih akontacija dobiti i isplatu."
                        });
                        document.Paragraphs.Add(new Paragraph()
                        {
                            Text = "Dobit se smatra zadržanom sve dok se zahtjevom pojedinog člana društva, a po ispunjenim uvjetima (likvidnost društva),  ne zatraži i ne izvrši isplata (djelomična ili cjelokupna) na način definiran u zahtjevu člana društva. Članovi društva koji su dobit predujmljivali moraju donijeti odluku o pokriću tog predujma.Udio pojedinog člana društva u dobiti utvrđuje se prema omjeru udjela o temeljnom kapitalu. Iznimno, ako se financijsko stanje društva pogorša do dana isplate dobiti, članovi društva neće zahtijevati isplatu dobiti nego će se suzdržati od isplate dok takvo stanje traje. O takvom stanju Uprava društva pravodobno pisano izvještava članove društva uz opis nastalih događaja i prijedlog za rješenje."
                        });
                        break;
                }

            }

        }

        //public StatementDocument GetDocument()
        //{
        //    var document = new StatementDocument();

        //    document.Header = String.Format("Na temelju odredbi Zakona o trgovačkim društvima, odredbi Zakona o računovodstvu i društvenog ugovora, skupština društva {0} donijela je dana {1:dd.MM.yyyy.}", nazivObveznika, DateTime.Now);
        //    document.TitleI = "ODLUKU";
        //    document.TitleII = String.Format("o raspodjeli i uporabi dobiti {0}. godine", godina);

        //    if (uDobitku)
        //    {
        //        GetDobit(document);
        //    }
        //    else
        //    {
        //        GetGubitak(document);
        //    }

        //    document.FooterI = "Predsjednik skupštine";
        //    document.FooterII = String.Format("{0}", ovlastenaOsoba);

        //    return document;
        //}
    }
}
