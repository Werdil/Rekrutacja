using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Workers.Interfaces
{
    // Interfejs dla operacji matematycznych
    public interface IOperacjaMatematyczna
    {
        decimal WykonajOperacje(decimal a, decimal b);
    }
}
