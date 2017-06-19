using System;
using NUnit.Framework;
using Eforah_BetaalApp.Droid.Controllers;

namespace Eforah_BetaalApp.Droid.Test
{
    [TestFixture]
    public class LoginTests
    {
        private Droid.Controllers.LoginActivity loginActivity;

        [SetUp]
        public void Setup()
        {
            loginActivity = new Droid.Controllers.LoginActivity();
        }

        [Test]
        public void NoPasswordControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { loginActivity.NoLoginDetailsControle(null, "TestUsername"); });
            Assert.That(ex.Message, Is.EqualTo("Password is null\nParameter name: passwordinput"));
            Assert.That(ex.ParamName, Is.EqualTo("passwordinput"));
        }

        [Test]
        public void NoUsernameControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { loginActivity.NoLoginDetailsControle("TestPassword", null); });
            Assert.That(ex.Message, Is.EqualTo("Username is null\nParameter name: usernameinput"));
            Assert.That(ex.ParamName, Is.EqualTo("usernameinput"));
        }

        [Test]
        public void EmptyPasswordControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { loginActivity.NoLoginDetailsControle("", "TestUsername"); });
            Assert.That(ex.Message, Is.EqualTo("Password is empty\nParameter name: passwordinput"));
            Assert.That(ex.ParamName, Is.EqualTo("passwordinput"));
        }

        [Test]
        public void EmptyUsernameControleTest()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(
                delegate { loginActivity.NoLoginDetailsControle("TestPassword", ""); });
            Assert.That(ex.Message, Is.EqualTo("Username is empty\nParameter name: usernameinput"));
            Assert.That(ex.ParamName, Is.EqualTo("usernameinput"));
        }
        
        [Test]
        public void HashCorrectPasswordTest()
        {
            string testHash = LoginActivity.GetHashStringLogin("TestPassword");
            Assert.That(testHash, Is.EqualTo("873343A194A15A840F9B0F4798AD51BD5784F0FB4C2690C7478AEEE7E4159F02F435DA9505499BEF1EEB5036A160C0527D34B7CF3260ED613B0C82417B169659"));
        }

        [Test]
        public void HashEmptyPasswordTest()
        {
            string testHash = LoginActivity.GetHashStringLogin("");
            Assert.That(testHash, Is.EqualTo(""));
        }
    }
}