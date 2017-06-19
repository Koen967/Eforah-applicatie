using EforahWebapp.Controllers;
using EforahWebapp.Models;
using EforahWebapp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace EforahWebapp.Tests.Controllers
{
    [TestClass]
    public class VerenigingControllerTest
    {
        VerenigingController controller;
        List<Vereniging> dataVereniging;
        List<Locatie> dataLocatie;

        [TestInitialize]
        public void TestInitialize()
        {
            dataVereniging = new List<Vereniging>
            {
                new Vereniging { verenigingId = 1, locatieId = 1, naam = "Vereniging1", facebookAdminId = "1235468683", facebookGroupId = 135354545, agendaLink = "sda", telefoonnummer = "0678945231", email = "ver1iging@mail.net" },
                new Vereniging { verenigingId = 2, locatieId = 2, naam = "Vereniging2", facebookAdminId = "4564678645", facebookGroupId = 534534645, agendaLink = "asf", telefoonnummer = "0674563215", email = "ver2iging@mail.net" }
            };

            dataLocatie = new List<Locatie>
            {
                new Locatie { locatieId = 1, postcode = "5123AK", huisnummer = 23, adres = "Locatielaan", plaats = "Lolicatie" },
                new Locatie { locatieId = 2, postcode = "1354ES", huisnummer = 41, adres = "Straatstraat", plaats = "Plaliplaats" },
                new Locatie { locatieId = 3, postcode = "3215FS", huisnummer = 37, adres = "Liliielaan", plaats = "Lolificatie" }
            };

            var setVereniging = new Mock<DbSet<Vereniging>>().SetupData(dataVereniging);
            var setLocatie = new Mock<DbSet<Locatie>>().SetupData(dataLocatie);
            
            var context = new Mock<eforahbetaalappEntities>();

            context.Setup(s => s.Vereniging).Returns(setVereniging.Object);
            context.Setup(s => s.Locatie).Returns(setLocatie.Object);

            var session = new HttpSessionMock();
            int[] i = { 1 };
            session["VerenigingIds"] = i;

            controller = new VerenigingController(context.Object, session);
        }

        [TestMethod]
        public void verenigingTestIndexView()
        {
            var result = controller.Index();

            var verenigingen = (List<Vereniging>)result.Model;
            Assert.AreEqual(dataVereniging[0], verenigingen[0]);
        }

        [TestMethod]
        public void verenigingTestGetDetails()
        {
            var result = controller.Details(2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void verenigingTestGetCreate()
        {
            var result = controller.Create();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void verenigingTestPostCreate()
        {
            var vereniging = new Vereniging { verenigingId = 2000, locatieId = 1, naam = "Test vereniging", agendaLink = "link", telefoonnummer = "0612345678", email = "test@test.nl"};

            var locatie = new Locatie();

            var result = controller.Create(vereniging, locatie);

            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        public void verenigingTestGetEdit()
        {
            var result = controller.Edit(2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestGetDelete()
        {
            var result = controller.Delete(2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestPostDelete()
        {
            var result = controller.DeleteConfirmed(2);

            Assert.IsNotNull(result);
        }
    }
}
