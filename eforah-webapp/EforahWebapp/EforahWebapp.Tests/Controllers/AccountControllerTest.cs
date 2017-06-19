using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Controllers;
using EforahWebapp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using Moq;
using System.Web.Mvc;

namespace EforahWebapp.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        AccountController controller;
        List<Gebruiker> dataGebruiker;
        List<Lid> dataLid;
        LoginViewModel loginModelLid;
        LoginViewModel loginModelAdmin;

        [TestInitialize]
        public void TestInitialize()
        {
            dataLid = new List<Lid>
            {
                new Lid { lidId = 1, gebruikerId = 1, verenigingId = 1, saldo = 15.00m, rol = "lid" },
                new Lid { lidId = 2, gebruikerId = 2, verenigingId = 2, saldo = 10.50m, rol = "lid" },
                new Lid { lidId = 3, gebruikerId = 3, verenigingId = 1, saldo = 5.55m, rol = "Admin" }
            };

            dataGebruiker = new List<Gebruiker>
            {
                new Gebruiker { gebruikerId = 1, locatieId = 1, gebruikersnaam = "Koen967", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "Koen967@hotmail.com", telefoonnummer = "0630493112", voornaam = "Koen", achternaam = "van Helvoort" },
                new Gebruiker { gebruikerId = 2, locatieId = 2, gebruikersnaam = "Nielszs", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "Nilly@gmail.com", telefoonnummer = "0634689462", voornaam = "Niels", achternaam = "Wijers" },
                new Gebruiker { gebruikerId = 3, locatieId = 3, gebruikersnaam = "MarcoP", wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F", email = "MarcoP@waterloo.defeat", telefoonnummer = "0688754132", voornaam = "Marco", achternaam = "Polio" }
            };

            var setGebruiker = new Mock<DbSet<Gebruiker>>().SetupData(dataGebruiker);
            var setLocatie = new Mock<DbSet<Lid>>().SetupData(dataLid);

            var context = new Mock<eforahbetaalappEntities>();

            context.Setup(s => s.Gebruiker).Returns(setGebruiker.Object);
            context.Setup(s => s.Lid).Returns(setLocatie.Object);

            controller = new AccountController(context.Object);

            loginModelLid = new LoginViewModel()
            {
                Gebruikersnaam = "Koen967",
                Wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F"
            };

            loginModelAdmin = new LoginViewModel()
            {
                Gebruikersnaam = "MarcoP",
                Wachtwoord = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F"
            };
        }

        [TestMethod]
        public void accountTestGetLoginNullInput()
        {
            var result = controller.Login(null);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void accountTestGetLoginEmptyInput()
        {
            var result = controller.Login("") as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void accountTestGetLoginReturnUrl()
        {
            var result = controller.Login("lid/index");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void accountTestPostLoginLid()
        {
            var result = controller.Login(loginModelLid, "lid/index");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void accountTestPostLoginAdmin()
        {
            var result = controller.Login(loginModelAdmin, "lid/index");

            Assert.IsNotNull(result);
        }
    }
}
