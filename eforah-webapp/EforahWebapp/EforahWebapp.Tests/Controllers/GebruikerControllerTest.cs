using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Controllers;
using System.Collections.Generic;
using EforahWebapp.Models;
using Moq;
using System.Data.Entity;

namespace EforahWebapp.Tests.Controllers
{
    [TestClass]
    public class GebruikerControllerTest
    {
        GebruikerController controller;
        List<Lid> dataLid;
        List<Gebruiker> dataGebruiker;
        List<Vereniging> dataVereniging;
        List<Locatie> dataLocatie;

        [TestInitialize]
        public void TestInitialize()
        {
            dataGebruiker = new List<Gebruiker>
            {
                new Gebruiker { gebruikerId = 1, locatieId = 1, gebruikersnaam = "Koen967", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "Koen967@hotmail.com", telefoonnummer = "0630493112", voornaam = "Koen", achternaam = "van Helvoort" },
                new Gebruiker { gebruikerId = 2, locatieId = 2, gebruikersnaam = "Nielszs", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "Nilly@gmail.com", telefoonnummer = "0634689462", voornaam = "Niels", achternaam = "Wijers" },
                new Gebruiker { gebruikerId = 3, locatieId = 3, gebruikersnaam = "MarcoP", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "MarcoP@waterloo.defeat", telefoonnummer = "0688754132", voornaam = "Marco", achternaam = "Polio" }
            };

            dataLocatie = new List<Locatie>
            {
                new Locatie { locatieId = 1, postcode = "5123AK", huisnummer = 23, adres = "Locatielaan", plaats = "Lolicatie" },
                new Locatie { locatieId = 2, postcode = "1354ES", huisnummer = 41, adres = "Straatstraat", plaats = "Plaliplaats" },
                new Locatie { locatieId = 3, postcode = "3215FS", huisnummer = 37, adres = "Liliielaan", plaats = "Lolificatie" }
            };

            var setGebruiker = new Mock<DbSet<Gebruiker>>().SetupData(dataGebruiker);
            var setLocatie = new Mock<DbSet<Locatie>>().SetupData(dataLocatie);

            var context = new Mock<eforahbetaalappEntities>();

            context.Setup(s => s.Gebruiker).Returns(setGebruiker.Object);
            context.Setup(s => s.Locatie).Returns(setLocatie.Object);

            controller = new GebruikerController(context.Object);
        }
        [TestMethod]
        public void gebruikerTestIndexView()
        {
            var result = controller.Index();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gebruikerTestGetDetails()
        {
            var result = controller.Details(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gebruikerTestGetCreate()
        {
            var result = controller.Create();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gebruikerTestPostCreate()
        {
            var gebruiker = new Gebruiker { gebruikerId = 4, locatieId = 2, gebruikersnaam = "nieuw", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "niew@adres.mail", telefoonnummer = "55564165", voornaam = "nieuw", achternaam = "gebruikers" };

            var result = controller.Create(gebruiker);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gebruikerTestGetEdit()
        {
            var result = controller.Edit(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gebruikerTestGetDelete()
        {
            var result = controller.Delete(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gebruikerTestPostDelete()
        {
            var result = controller.DeleteConfirmed(1);

            Assert.IsNotNull(result);
        }
    }
}
