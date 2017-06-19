using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Tests.Mocks;
using EforahWebapp.Services;
using System.Web;

namespace EforahWebapp.Tests.Services
{
    [TestClass]
    public class CookieServiceTest
    {
        HttpResponseMock response;
        HttpRequestMock request;
        HttpCookieCollection cookies;

        CookieService service;

        [TestInitialize]
        public void TestInitialize()
        {
            cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("EforahBeheerAppCookie"));
            cookies["EforahBeheerAppCookie"]["Cookie test info"] = "testinfo";

            response = new HttpResponseMock(cookies);
            request = new HttpRequestMock(cookies);


            service = new CookieService(response, request);
        }

        [TestMethod]
        public void CookieServiceTestSetCorrect()
        {
            service.SetData("Tester", "test");

            Assert.AreEqual("test", cookies["EforahBeheerAppCookie"]["Tester"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "dataName, Argument is null or an empty string")]
        public void CookieServiceTestSetNullName()
        {
            service.SetData(null, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "dataName, Argument is null or an empty string")]
        public void CookieServiceTestSetEmptyName()
        {
            service.SetData("", "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "data, Argument is null or an empty string")]
        public void CookieServiceTestSetNullData()
        {
            service.SetData("test", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "data, Argument is null or an empty string")]
        public void CookieServiceTestSetEmptyData()
        {
            service.SetData("test", "");
        }

        [TestMethod]
        public void CookieServiceTestGetCorrect()
        {
            //cookies["EforahBeheerAppCookie"]["Cookie test info"] = "testinfo";
            string data = service.GetData("Cookie test info");

            Assert.AreEqual("testinfo", data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "dataName, Argument is null or an empty string")]
        public void CookieServiceTestGetNullName()
        {
            service.GetData(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "dataName, Argument is null or an empty string")]
        public void CookieServiceTestGetEmptyName()
        {
            service.GetData("");
        }
    }
}
