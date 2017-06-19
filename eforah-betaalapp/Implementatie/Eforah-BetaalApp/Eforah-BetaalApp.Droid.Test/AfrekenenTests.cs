using System;
using NUnit.Framework;
using Eforah_BetaalApp.Droid.Controllers;

namespace Eforah_BetaalApp.Droid.Test
{
    [TestFixture]
    public class AfrekenenTests
    {

        private AfrekenenActivity afrekenenactivity;
        
        [SetUp]
        public void Setup()
        {
            afrekenenactivity = new AfrekenenActivity();
        }

        [Test]
        public void BedragNullControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { afrekenenactivity.BedragValidationControle(null); });
            Assert.That(ex.Message, Is.EqualTo("Te betalen bedrag is null\nParameter name: teBetalenBedrag"));
            Assert.That(ex.ParamName, Is.EqualTo("teBetalenBedrag"));
        }
    }
}