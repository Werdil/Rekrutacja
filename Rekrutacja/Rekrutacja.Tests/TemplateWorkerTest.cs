using NUnit.Framework;
using Rekrutacja.Workers.Enums;
using Rekrutacja.Workers.Template;
using Soneta.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Tests
{
    [TestFixture]
    public class TemplateWorkerTests
    {
        private TemplateWorker worker;

        [SetUp]
        public void Setup()
        {
            worker = new TemplateWorker();
        }

        [Test]
        [TestCase(Operacje.Dodawanie, 3, 2, 5)]
        [TestCase(Operacje.Odejmowanie, 3, 2, 1)]
        [TestCase(Operacje.Mnożenie, 3, 2, 6)]
        [TestCase(Operacje.Dzielenie, 6, 2, 3)]
        public void Oblicz_PoprawneWyniki(Operacje operacja, decimal x, decimal y, decimal expected)
        {
            // Act
            var wynik = worker.Oblicz(operacja, x, y);

            // Assert
            Assert.AreEqual(expected, wynik);
        }

        [Test]
        public void Oblicz_DzieleniePrzezZero_ThrowsInvalidOperationException()
        {
            // Arrange
            var operacja = Operacje.Dzielenie;
            var x = 6;
            var y = 0;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => worker.Oblicz(operacja, x, y));
        }

        [Test]
        public void NowaOperacja_NieznanaOperacja_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => worker.NowaOperacja((Operacje)999));
        }
    }
}
