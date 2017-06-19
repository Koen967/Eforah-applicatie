using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Models;
using EforahWebapp.Controllers;
using Moq;
using System.Web.Mvc;
using System.Data.Entity;
using EforahWebapp.Tests.Mocks;

namespace EforahWebapp.Tests.Controllers
{
    /// <summary>
    /// Summary description for LidControllerTest
    /// </summary>
    [TestClass]
    public class LidControllerTest
    {
        LidController controller;
        LidController controllerZonderSessie;
        List<Lid> dataLid;
        List<Gebruiker> dataGebruiker;
        List<Vereniging> dataVereniging;
        List<Locatie> dataLocatie;

        [TestInitialize]
        public void TestInitialize()
        {
            dataLid = new List<Lid>
            {
                new Lid { lidId = 1, gebruikerId = 1, verenigingId = 1, saldo = 15.00m, rol = "lid" },
                new Lid { lidId = 2, gebruikerId = 2, verenigingId = 2, saldo = 10.50m, rol = "lid" },
                new Lid { lidId = 3, gebruikerId = 3, verenigingId = 1, saldo = 5.55m, rol = "lid" }
            };

            dataGebruiker = new List<Gebruiker>
            {
                new Gebruiker { gebruikerId = 1, locatieId = 1, gebruikersnaam = "Koen967", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "Koen967@hotmail.com", telefoonnummer = "0630493112", voornaam = "Koen", achternaam = "van Helvoort" },
                new Gebruiker { gebruikerId = 2, locatieId = 2, gebruikersnaam = "Nielszs", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "Nilly@gmail.com", telefoonnummer = "0634689462", voornaam = "Niels", achternaam = "Wijers" },
                new Gebruiker { gebruikerId = 3, locatieId = 3, gebruikersnaam = "MarcoP", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "MarcoP@waterloo.defeat", telefoonnummer = "0688754132", voornaam = "Marco", achternaam = "Polio" }
            };

            dataVereniging = new List<Vereniging>
            {
                new Vereniging { verenigingId = 1, locatieId = 1, naam = "Vereniging1", facebookAdminId = "01235468683", facebookGroupId = 135354545, agendaLink = "sda", telefoonnummer = "0678945231", email = "ver1iging@mail.net" },
                new Vereniging { verenigingId = 2, locatieId = 2, naam = "Vereniging2", facebookAdminId = "4564678645", facebookGroupId = 534534645, agendaLink = "asf", telefoonnummer = "0674563215", email = "ver2iging@mail.net" }
            };

            dataLocatie = new List<Locatie>
            {
                new Locatie { locatieId = 1, postcode = "5123AK", huisnummer = 23, adres = "Locatielaan", plaats = "Lolicatie" },
                new Locatie { locatieId = 2, postcode = "1354ES", huisnummer = 41, adres = "Straatstraat", plaats = "Plaliplaats" },
                new Locatie { locatieId = 3, postcode = "3215FS", huisnummer = 37, adres = "Liliielaan", plaats = "Lolificatie" }
            };

            var setLid = new Mock<DbSet<Lid>>().SetupData(dataLid);
            var setGebruiker = new Mock<DbSet<Gebruiker>>().SetupData(dataGebruiker);
            var setVereniging = new Mock<DbSet<Vereniging>>().SetupData(dataVereniging);
            var setLocatie = new Mock<DbSet<Locatie>>().SetupData(dataLocatie);

            var context = new Mock<eforahbetaalappEntities>();

            context.Setup(s => s.Lid).Returns(setLid.Object);
            context.Setup(s => s.Gebruiker).Returns(setGebruiker.Object);
            context.Setup(s => s.Vereniging).Returns(setVereniging.Object);
            context.Setup(s => s.Locatie).Returns(setLocatie.Object);

            var session = new HttpSessionMock();
            int[] i = { 1 };
            session["VerenigingIds"] = i;

            controller = new LidController(context.Object, session);
            controllerZonderSessie = new LidController(context.Object);
        }

        [TestMethod]
        public void lidTestIndexView()
        {
            var result = controller.Index();

            var leden = (List<Lid>)result.Model;
            Assert.AreEqual(dataLid[0], leden[0]);
            Assert.AreEqual(dataLid[2], leden[1]);
        }

        [TestMethod]
        public void lidTestGetDetails()
        {
            var result = controller.Details(2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestGetCreate()
        {
            var result = controller.Create();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestPostCreate()
        {
            var lid = new Lid { lidId = 4, gebruikerId = 2, verenigingId = 1, saldo = 11.00m, rol = "lid" };

            var result = controller.Create(lid);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestPostCreateUnvalid()
        {
            var lid = new Lid { lidId = 1, gebruikerId = 2, verenigingId = 1, saldo = 11.00m, rol = "lid" };

            var result = controller.Create(lid);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestGetEdit()
        {
            var result = controller.Edit(2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void lidTestGetEditNull()
        {
            var result = controller.Edit(null as int?);

            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void lidTestPostEdit()
        //{
        //    var lid = new Lid { lidId = 1, gebruikerId = 1, verenigingId = 1, saldo = 12.50m, rol = "lid" };

        //    var result = controller.Edit(lid);

        //    Assert.IsNotNull(result);
        //}

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
