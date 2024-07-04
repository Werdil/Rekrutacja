using Rekrutacja.Workers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Workers.CalculationStrategies
{
    public class DzielenieStrategy : IOperacjaMatematyczna
    {
        public decimal WykonajOperacje(decimal a, decimal b)
        {
            if (b == 0) throw new InvalidOperationException("Dzielenie przez zero jest niedozwolone.");
            return a / b;
        }
    }
}
