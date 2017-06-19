using System;
using NUnit.Framework;
using Android.App;
using Android.Widget;
using Android.Graphics;
using Eforah_BetaalApp.Droid.Components;

namespace Eforah_BetaalApp.Droid.Test.Mocks
{
    [TestFixture]
    public class QRCodeTests
    {
        //MainActivity mainactivity;
        QRCode QRService;
        Application app;

        [SetUp]
        public void Setup()
        {
            app = new Application();
            QRServiceMock qrmock = new QRServiceMock();
            QRService = new QRCode(app, qrmock);
        }

        [TearDown]
        public void Tear() { }

        [Test]
        public void GeneratorExceptionOnNullString()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { QRService.GenerateQRCode(null, 20, 20); });
            Assert.That(ex.Message, Is.EqualTo("Parameter cannot be null or empty.\nParameter name: Data"));
            Assert.That(ex.ParamName, Is.EqualTo("Data"));
        }

        [Test]
        public void GeneratorExceptionOnEmptyString()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { QRService.GenerateQRCode("", 20, 20); });
            Assert.That(ex.Message, Is.EqualTo("Parameter cannot be null or empty.\nParameter name: Data"));
            Assert.That(ex.ParamName, Is.EqualTo("Data"));
        }

        [Test]
        public void GeneratorExceptionOnNullWidth()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                   delegate { QRService.GenerateQRCode("Hallo", 0, 20); });
            Assert.That(ex.Message, Is.EqualTo("Width can't be smaller than 1.\nParameter name: width"));
            Assert.That(ex.ParamName, Is.EqualTo("width"));
        }

        [Test]
        public void GeneratorExceptionOnNullHeight()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                   delegate { QRService.GenerateQRCode("Hallo", 20, 0); });
            Assert.That(ex.Message, Is.EqualTo("Height can't be smaller than 1.\nParameter name: height"));
            Assert.That(ex.ParamName, Is.EqualTo("height"));
        }

        [Test]
        public void GeneratorCreatesBitmap()
        {
            Bitmap bit = QRService.GenerateQRCode("Hallo", 5, 5);
            int[] b = new int[25];
            Assert.NotNull(bit);
            bit.GetPixels(b, 0, 5, 0, 0, 5, 5);

            int compare = -84215041;

            for (int i = 0; i < 25; i++)
            {
                Assert.AreEqual(compare, b[i]);
            }
        }

    }
}