using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EforahWebapp.Services;

namespace EforahWebapp.Tests.Services
{
    [TestClass]
    public class HashServicesTest
    {
        string inputString;
        string expectedString;

        [TestInitialize]
        public void TestInitialize()
        {
            inputString = "wachtwoord";
            expectedString = "1A43A304A70B9EB55A53791692D487F75196379DE975D4A22E5141B31D9C3652EB6CCC279A4401CDF879301E7FC7525D831A9D70BFF3CCFE600BE4013B68971F";
        }

        [TestMethod]
        public void TestHashMethodWachtwoord()
        {
            string resultString = HashServices.GetHashString(inputString);

            Assert.AreEqual(expectedString, resultString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
                "A inputString of null was inappropriately allowed.")]
        public void TestHashMethodNull()
        {
            string resultString = HashServices.GetHashString(null);
        }

        [TestMethod]
        public void TestHashMethodEmptyString()
        {
            expectedString = "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E";

            string resultString = HashServices.GetHashString("");
            
            Assert.AreEqual(expectedString, resultString);
        }
    }
}
