using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NUnit.Framework;
using Eforah_BetaalApp.Droid.Controllers;

namespace Eforah_BetaalApp.Droid.Test
{
    class DateTimeconverterTest
    {
        //private MededelingActivity mededelingactivity;

        [SetUp]
        public void Setup()
        {
            //mededelingactivity = new MededelingActivity();
        }

        [Test]
        public void nodatetimecontroletest()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
            delegate { MededelingActivity.convertDateTime("", "HH:mm d/M/yyyy"); });
            
            Assert.That(ex.ParamName, Is.EqualTo("datetime"));
        }
        [Test]
        public void nodatetimeformatcontroletest()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
            delegate { MededelingActivity.convertDateTime("5/2/2017 9:30:00 AM", ""); });

            Assert.That(ex.ParamName, Is.EqualTo("datetimeformat"));
        }
        [Test]
        public void rightdatetimecontroletest()
        {
            string datetimetest = MededelingActivity.convertDateTime("2/5/2017 9:30:00 AM", "HH:mm d\\/M\\/yyyy");
            string expected = "09:30 5/2/2017";
            Assert.AreEqual(expected, datetimetest);
        }
        [Test]
        public void wrongdatetimeformatcontroletest()
        {
            string datetimetest = MededelingActivity.convertDateTime("2/5/2017 9:30:00 AM", "LL:qq d\\/o\\/PPPP");
            string expected = "LL:qq 5/o/PPPP";
            Assert.AreEqual(expected, datetimetest);
        }
        [Test]
        public void wrongdatetimecontroletest()
        {
            FormatException ex = Assert.Throws<FormatException>(
            delegate { MededelingActivity.convertDateTime("442/5/255017 94:304:040 QM", "HH:mm d\\/M\\/yyyy"); });
            Assert.That(ex.Message, Is.EqualTo("String was not recognized as a valid DateTime."));
        }
    }
}