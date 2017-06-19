using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Controllers;

namespace EforahWebapp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        HomeController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            controller = new HomeController();
        }

        [TestMethod]
        public void homeTestIndexView()
        {
            var result = controller.Index();

            Assert.IsNotNull(result);
        }
    }
}
