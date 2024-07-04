using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Workers.Enums
{
    public enum Operacje
    {

        // Operacja dodawania
        [Caption("+")]
        Dodawanie,

        // Operacja odejmowania
        [Caption("-")]
        Odejmowanie,

        // Operacja mnożenia
        [Caption("*")]
        Mnożenie,

        // Operacja dzielenia
        [Caption("/")]
        Dzielenie

    }
}
