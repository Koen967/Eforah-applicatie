using System;
using NUnit.Framework;
using Eforah_BetaalApp.Implementation.Services;
using Eforah_BetaalApp.Implementation.Test.Mocks;

namespace Eforah_BetaalApp.Implementation.Test
{
    [TestFixture]
    public class QRGeneratorTests
    {
        //MainActivity mainactivity;
        IQRService QRService;

        [SetUp]
        public void Setup()
        {
            BarcodeWriterMock qrmock = new BarcodeWriterMock();
            QRService = new QRService(qrmock);
        }

        [TearDown]
        public void Tear() { }

        [Test]
        public void GeneratorExceptionOnNullString()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { QRService.GenerateQRCode(null, 20, 20, 0); });
            Assert.That(ex.Message, Is.EqualTo("Parameter cannot be null or empty.\r\nParameternaam: Data"));
            Assert.That(ex.ParamName, Is.EqualTo("Data"));
        }

        [Test]
        public void GeneratorExceptionOnEmptyString()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { QRService.GenerateQRCode("", 20, 20, 0); });
            Assert.That(ex.Message, Is.EqualTo("Parameter cannot be null or empty.\r\nParameternaam: Data"));
            Assert.That(ex.ParamName, Is.EqualTo("Data"));
        }

        [Test]
        public void GeneratorExceptionOnNullWidth()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                   delegate { QRService.GenerateQRCode("Hallo", 0, 20, 0); });
            Assert.That(ex.Message, Is.EqualTo("Width can't be smaller than 1.\r\nParameternaam: width"));
            Assert.That(ex.ParamName, Is.EqualTo("width"));
        }

        [Test]
        public void GeneratorExceptionOnNullHeight()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                   delegate { QRService.GenerateQRCode("Hallo", 20, 0, 0); });
            Assert.That(ex.Message, Is.EqualTo("Height can't be smaller than 1.\r\nParameternaam: height"));
            Assert.That(ex.ParamName, Is.EqualTo("height"));
        }

        [Test]
        public void GeneratorGivesByteArray()
        {
            byte[] b = QRService.GenerateQRCode("Hallo", 5, 5, 1);

            Assert.NotNull(b);

            Assert.GreaterOrEqual(b.Length, 1);
        }

    }
}
