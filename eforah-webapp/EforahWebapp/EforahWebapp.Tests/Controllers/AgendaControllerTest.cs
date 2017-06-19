using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Controllers;
using System.Web.Mvc;

namespace EforahWebapp.Tests.Controllers
{
    [TestClass]
    public class AgendaControllerTest
    {
        AgendaController controller = new AgendaController();

        [TestMethod]
        public void agendaTestIndex()
        {
            var result = controller.Index("text") as ViewResult;

            Assert.AreEqual("text", result.ViewData["agendaLink"]);
            Assert.IsNotNull(result);
        }
    }
}
