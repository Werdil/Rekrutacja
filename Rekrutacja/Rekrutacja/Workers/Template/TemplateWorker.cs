using Soneta.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Kadry;
using Soneta.KadryPlace;
using Soneta.Types;
using Rekrutacja.Workers.Template;
using Soneta.Tools;
using Rekrutacja.Workers.Enums;
using Rekrutacja.Workers.CalculationStrategies;
using Rekrutacja.Workers.Interfaces;

//Rejetracja Workera - Pierwszy TypeOf określa jakiego typu ma być wyświetlany Worker, Drugi parametr wskazuje na jakim Typie obiektów będzie wyświetlany Worker
[assembly: Worker(typeof(TemplateWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public class TemplateWorker
    {
        //Aby parametry działały prawidłowo dziedziczymy po klasie ContextBase
        public class TemplateWorkerParametry : ContextBase
        {
            [Caption("Zmienna X"), Priority(1)]
            public decimal ZmiennaX { get; set; }

            [Caption("Operacja"), Priority(2)]
            [Required]
            public Operacje Operacja { get; set; }

            [Caption("Zmienna Y"), Priority(3)]
            public decimal ZmiennaY { get; set; }

            [Caption("Data obliczeń"), Priority(4)]
            public Date DataObliczen { get; set; }

            public TemplateWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
                this.Operacja = Operacje.Dodawanie;
            }
        }
        //Obiekt Context jest to pudełko które przechowuje Typy danych, aktualnie załadowane w aplikacji
        //Atrybut Context pobiera z "Contextu" obiekty które aktualnie widzimy na ekranie
        [Context]
        public Context Cx { get; set; }
        //Pobieramy z Contextu parametry, jeżeli nie ma w Context Parametrów mechanizm sam utworzy nowy obiekt oraz wyświetli jego formatkę
        [Context]
        public TemplateWorkerParametry Parametry { get; set; }
        //Atrybut Action - Wywołuje nam metodę która znajduje się poniżej
        [Action("Kalkulator",
           Description = "Prosty kalkulator ",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public void WykonajAkcje()
        {
            //Włączenie Debug, aby działał należy wygenerować DLL w trybie DEBUG
            DebuggerSession.MarkLineAsBreakPoint();

            decimal wynik = Oblicz(Parametry.Operacja, Parametry.ZmiennaX, Parametry.ZmiennaY);

            //Pobieranie danych z Contextu
            Pracownik[] pracownicy = null;
            if (this.Cx.Contains(typeof(Pracownik[])))
            {
                pracownicy = (Pracownik[])this.Cx[typeof(Pracownik[])];
                if (pracownicy.Length == 0)
                    throw new InvalidOperationException("Brak zaznaczonych pracowników.");
            }
            //Modyfikacja danych
            //Aby modyfikować dane musimy mieć otwartą sesję, któa nie jest read only
            using (Session nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                //Otwieramy Transaction aby można było edytować obiekt z sesji
                using (ITransaction trans = nowaSesja.Logout(true))
                {
                    foreach (var pracownik in pracownicy)
                    {
                        //Pobieramy obiekt z Nowo utworzonej sesji
                        var pracownikZSesja = nowaSesja.Get(pracownik);
                        //Features - są to pola rozszerzające obiekty w bazie danych, dzięki czemu nie jestesmy ogarniczeni to kolumn jakie zostały utworzone przez producenta
                        pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;
                        pracownikZSesja.Features["Wynik"] = (double)wynik;
                    }
                    //Zatwierdzamy zmiany wykonane w sesji
                    trans.CommitUI();
                }
                //Zapisujemy zmiany
                nowaSesja.Save();
            }
        }

        public decimal Oblicz(Operacje o,decimal x,decimal y)
        {
            IOperacjaMatematyczna operacja = NowaOperacja(o);
            decimal wynik = operacja.WykonajOperacje(x, y);
            return wynik;
        }

        public IOperacjaMatematyczna NowaOperacja(Operacje operacja)
        {
            switch (operacja)
            {
                case Operacje.Dodawanie:
                    return new DodawanieStrategy();
                case Operacje.Odejmowanie:
                    return new OdejmowanieStrategy();
                case Operacje.Mnożenie:
                    return new MnozenieStrategy();
                case Operacje.Dzielenie:
                    return new DzielenieStrategy();
                default:
                    throw new InvalidOperationException("Nieznana operacja. Użyj jednej z następujących: +, -, *, /.");
            }
        }
    }
}