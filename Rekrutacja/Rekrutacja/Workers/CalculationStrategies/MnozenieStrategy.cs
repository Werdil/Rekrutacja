using Rekrutacja.Workers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Workers.CalculationStrategies
{
    public class MnozenieStrategy : IOperacjaMatematyczna
    {
        public decimal WykonajOperacje(decimal a, decimal b) => a * b;
    }
}
