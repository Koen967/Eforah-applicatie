using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Controllers;
using EforahWebapp.Models;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using EforahWebapp.Tests.Mocks;

namespace EforahWebapp.Tests.Controllers
{
    [TestClass]
    public class MededelingControllerTest
    {
        MededelingController controller;
        MededelingController controllerZonderSessie;
        List<Vereniging> dataVereniging;
        List<Locatie> dataLocatie;
        List<Mededeling> dataMededeling;

        [TestInitialize]
        public void TestInitialize()
        {

            DateTime testDatum = new DateTime(1997, 6, 13);

            dataLocatie = new List<Locatie>
            {
                new Locatie { locatieId = 1, postcode = "5123AK", huisnummer = 23, adres = "Locatielaan", plaats = "Lolicatie" },
                new Locatie { locatieId = 2, postcode = "1354ES", huisnummer = 41, adres = "Straatstraat", plaats = "Plaliplaats" },
                new Locatie { locatieId = 3, postcode = "3215FS", huisnummer = 37, adres = "Liliielaan", plaats = "Lolificatie" }
            };

            dataVereniging = new List<Vereniging>
            {
                new Vereniging { verenigingId = 1, locatieId = 1, naam = "Vereniging1", facebookAdminId = "1235468683", facebookGroupId = 135354545, agendaLink = "sda", telefoonnummer = "0678945231", email = "ver1iging@mail.net" },
                new Vereniging { verenigingId = 2, locatieId = 2, naam = "Vereniging2", facebookAdminId = "4564678645", facebookGroupId = 534534645, agendaLink = "asf", telefoonnummer = "0674563215", email = "ver2iging@mail.net" }
            };

            dataMededeling = new List<Mededeling>
            {
                new Mededeling { mededelingId = 1, verenigingId = 1, plaatsingDatum = testDatum, titel = "Titel1", mededeling1 = "Mededeling1"},
                new Mededeling { mededelingId = 2, verenigingId = 1, plaatsingDatum = testDatum, titel = "Titel2", mededeling1 = "Mededeling2"},
                new Mededeling { mededelingId = 3, verenigingId = 2, plaatsingDatum = testDatum, titel = "Titel3", mededeling1 = "Mededeling3"}
            };

            var setLocatie = new Mock<DbSet<Locatie>>().SetupData(dataLocatie);
            var setVereniging = new Mock<DbSet<Vereniging>>().SetupData(dataVereniging);
            var setMededeling = new Mock<DbSet<Mededeling>>().SetupData(dataMededeling);

            var context = new Mock<eforahbetaalappEntities>();

            context.Setup(s => s.Locatie).Returns(setLocatie.Object);
            context.Setup(s => s.Vereniging).Returns(setVereniging.Object);
            context.Setup(s => s.Mededeling).Returns(setMededeling.Object);

            var session = new HttpSessionMock();
            int[] i = { 1 };
            session["VerenigingIds"] = i;

            controller = new MededelingController(context.Object, session);
            controllerZonderSessie = new MededelingController(context.Object);
        }

        [TestMethod]
        public void mededelingTestIndexView()
        {
            var result = controller.Index();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void mededelingTestGetDetails()
        {
            var result = controller.Details(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void mededelingTestGetCreate()
        {
            var result = controller.Create();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void mededelingTestPostCreate()
        {
            DateTime testDatum = new DateTime(2015, 9, 26);
            var mededeling = new Mededeling { mededelingId = 4, verenigingId = 2, plaatsingDatum = testDatum, titel = "Titel4", mededeling1 = "Mededeling4" };

            var result = controller.Create(mededeling);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void mededelingTestGetEdit()
        {
            var result = controller.Edit(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void mededelingTestGetDelete()
        {
            var result = controller.Delete(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void mededelingTestPostDelete()
        {
            var result = controller.DeleteConfirmed(1);

            Assert.IsNotNull(result);
        }
    }
}
