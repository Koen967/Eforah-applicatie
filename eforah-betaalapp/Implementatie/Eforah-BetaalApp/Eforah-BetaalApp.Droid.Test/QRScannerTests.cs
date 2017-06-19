using System;
using NUnit.Framework;
using Eforah_BetaalApp.Droid.Controllers;

namespace Eforah_BetaalApp.Droid.Test
{
    [TestFixture]
    public class QRScannerTests
    {
        private ScannerActivity scanneractivity;

        [SetUp]
        public void Setup()
        {
            scanneractivity = new ScannerActivity();
        }

        [Test]
        public void BedragNullControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { scanneractivity.BedragLessThanZeroControle(null); });
            Assert.That(ex.Message, Is.EqualTo("Te betalen bedrag is null\nParameter name: teBetalenBedrag"));
            Assert.That(ex.ParamName, Is.EqualTo("teBetalenBedrag"));
        }

        [Test]
        public void BedragUnknownControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { scanneractivity.BedragLessThanZeroControle(""); });
            Assert.That(ex.Message, Is.EqualTo("Te betalen bedrag is niet ingevuld\nParameter name: teBetalenBedrag"));
            Assert.That(ex.ParamName, Is.EqualTo("teBetalenBedrag"));
        }

        [Test]
        public void VerenigingIdNullControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { scanneractivity.VerenigingIdNullControle(null); });
            Assert.That(ex.Message, Is.EqualTo("VerenigingId is null\nParameter name: verenigingId"));
            Assert.That(ex.ParamName, Is.EqualTo("verenigingId"));
        }

        [Test]
        public void VerenigingIdUnknownControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { scanneractivity.VerenigingIdNullControle(""); });
            Assert.That(ex.Message, Is.EqualTo("VerenigingId is niet ingevuld\nParameter name: verenigingId"));
            Assert.That(ex.ParamName, Is.EqualTo("verenigingId"));
        }
    }
}