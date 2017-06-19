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
using Eforah_BetaalApp.Droid.Components;
using NUnit.Framework;
using Eforah_BetaalApp.Droid.Test.Mocks;
using Eforah_BetaalApp.Implementation.Models;

namespace Eforah_BetaalApp.Droid.Test
{
    class ExpandableLedenListAdapterTests
    {
        ExpandableLedenListAdapter adapter;
        LedenViewMock view;

        [SetUp]
        public void Setup()
        {
            List<LidModel> leden = new List<LidModel>();

            leden.Add(new LidModel() {
                voornaam = "Marc",
                achternaam = "von Gabain",
                email = "m_von_gabain@hotmail.com",
                telefoonnummer = "0612345789",
                postcode = "1234AB",
                huisnummer = "12",
                adres = "Vissersstraat",
                plaats = "Eindhoven",
                foto = "",
                telefoonnummerAlt = "0698754321"
            });
            leden.Add(new LidModel()
            {
                voornaam = "Frodo",
                achternaam = "Baggings",
                email = "nine_fingers@hobbitmail.com",
                telefoonnummer = "0612345789",
                postcode = "9999GA",
                huisnummer = "9",
                adres = "Balingshoek",
                plaats = "Gouw",
                foto = "",
                telefoonnummerAlt = "0698754321"
            });
            leden.Add(new LidModel()
            {
                voornaam = "Harry",
                achternaam = "Potter",
                email = "MasterOfDeath@hedwig.com",
                telefoonnummer = "0612345789",
                postcode = "2145ZW",
                huisnummer = "4",
                adres = "Ligusterlaan",
                plaats = "Klein Zanikem",
                foto = "",
                telefoonnummerAlt = "0698754321"
            });

            view = new LedenViewMock(MainActivity.MActivity);

            adapter = new ExpandableLedenListAdapter(MainActivity.MActivity, leden);
        }

        #region children
        [Test]
        public void GetChildNegatieveReturnsFirst()
        {
            String child = (String) adapter.GetChild(-1,-1);
            Assert.AreEqual("m_von_gabain@hotmail.com", child);
        }

        [Test]
        public void GetChildFirst()
        {
            String child = (String)adapter.GetChild(0, 0);
            Assert.AreEqual("m_von_gabain@hotmail.com", child);
        }

        [Test]
        public void GetChildFirstChildPositionDoesntMatter()
        {
            String child = (String)adapter.GetChild(0, 50);
            Assert.AreEqual("m_von_gabain@hotmail.com", child);
        }

        [Test]
        public void GetChildSecond()
        {
            String child = (String)adapter.GetChild(1, 0);
            Assert.AreEqual("nine_fingers@hobbitmail.com", child);
        }

        [Test]
        public void GetChildThird()
        {
            String child = (String)adapter.GetChild(2, 0);
            Assert.AreEqual("MasterOfDeath@hedwig.com", child);
        }

        [Test]
        public void GetChildBeyondLastReturnsLast()
        {
            String child = (String)adapter.GetChild(3, 0);
            Assert.AreEqual("MasterOfDeath@hedwig.com", child);
        }

        [Test]
        public void GetChildId()
        {
            long child = adapter.GetChildId(3, 0);
            Assert.AreEqual(0, child);
        }

        [Test]
        public void GetChildIdNegative()
        {
            long child = adapter.GetChildId(-5, 0);
            Assert.AreEqual(0, child);
        }

        [Test]
        public void GetChildIdHigh()
        {
            long child = adapter.GetChildId(52, 0);
            Assert.AreEqual(0, child);
        }

        [Test]
        public void GetChildCount()
        {
            long child = adapter.GetChildrenCount(3);
            Assert.AreEqual(1, child);
        }

        [Test]
        public void GetChildCountNegative()
        {
            long child = adapter.GetChildrenCount(-5);
            Assert.AreEqual(1, child);
        }

        [Test]
        public void GetChildCountHigh()
        {
            long child = adapter.GetChildrenCount(52);
            Assert.AreEqual(1, child);
        }

        //[Test]
        //public void GetChildView()
        //{
        //    var child = adapter.GetChildView(0, 0, false, view, null);

        //    TextView email = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_email);
        //    TextView telefoonnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer);
        //    TextView altnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer);
        //    TextView straat = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_straat);
        //    TextView postcode = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode);

        //    Assert.AreEqual("Email: m_von_gabain@hotmail.com", email.Text);
        //    Assert.AreEqual("Telefoon nummer: 0612345789", telefoonnummer.Text);
        //    Assert.AreEqual("Alternatief Nummer:  0698754321", altnummer.Text);
        //    Assert.AreEqual("Straat: Vissersstraat 12", straat.Text);
        //    Assert.AreEqual("Adres: 1234AB Eindhoven", postcode.Text);
        //}

        //[Test]
        //public void GetChildViewSecond()
        //{
        //    var child = adapter.GetChildView(1, 0, false, view, null);

        //    TextView email = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_email);
        //    TextView telefoonnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer);
        //    TextView altnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer);
        //    TextView straat = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_straat);
        //    TextView postcode = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode);

        //    Assert.AreEqual("Email: nine_fingers@hobbitmail.com", email.Text);
        //    Assert.AreEqual("Telefoon nummer: 0612345789", telefoonnummer.Text);
        //    Assert.AreEqual("Alternatief Nummer:  0698754321", altnummer.Text);
        //    Assert.AreEqual("Straat: Balingshoek 9", straat.Text);
        //    Assert.AreEqual("Adres: 9999GA Gouw", postcode.Text);
        //}

        //[Test]
        //public void GetChildViewThird()
        //{
        //    var child = adapter.GetChildView(2, 0, false, view, null);

        //    TextView email = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_email);
        //    TextView telefoonnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer);
        //    TextView altnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer);
        //    TextView straat = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_straat);
        //    TextView postcode = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode);

        //    Assert.AreEqual("Email: MasterOfDeath@hedwig.com", email.Text);
        //    Assert.AreEqual("Telefoon nummer: 0612345789", telefoonnummer.Text);
        //    Assert.AreEqual("Alternatief Nummer:  0698754321", altnummer.Text);
        //    Assert.AreEqual("Straat: Ligusterlaan 4", straat.Text);
        //    Assert.AreEqual("Adres: 2145ZW Klein Zanikem", postcode.Text);
        //}

        //[Test]
        //public void GetChildViewNegativeReturnsFirst()
        //{
        //    var child = adapter.GetChildView(-10, 0, false, view, null);

        //    TextView email = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_email);
        //    TextView telefoonnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer);
        //    TextView altnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer);
        //    TextView straat = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_straat);
        //    TextView postcode = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode);

        //    Assert.AreEqual("Email: m_von_gabain@hotmail.com", email.Text);
        //    Assert.AreEqual("Telefoon nummer: 0612345789", telefoonnummer.Text);
        //    Assert.AreEqual("Alternatief Nummer:  0698754321", altnummer.Text);
        //    Assert.AreEqual("Straat: Vissersstraat 12", straat.Text);
        //    Assert.AreEqual("Adres: 1234AB Eindhoven", postcode.Text);
        //}

        //[Test]
        //public void GetChildViewBeyondCountReturnsLast()
        //{
        //    var child = adapter.GetChildView(52, 0, false, view, null);

        //    TextView email = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_email);
        //    TextView telefoonnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer);
        //    TextView altnummer = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer);
        //    TextView straat = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_straat);
        //    TextView postcode = (TextView)child.FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode);

        //    Assert.AreEqual("Email: MasterOfDeath@hedwig.com", email.Text);
        //    Assert.AreEqual("Telefoon nummer: 0612345789", telefoonnummer.Text);
        //    Assert.AreEqual("Alternatief Nummer:  0698754321", altnummer.Text);
        //    Assert.AreEqual("Straat: Ligusterlaan 4", straat.Text);
        //    Assert.AreEqual("Adres: 2145ZW Klein Zanikem", postcode.Text);
        //}
        #endregion

        #region groep
        [Test]
        public void GetGroupNegatieveReturnsFirst()
        {
            String group = (String)adapter.GetGroup(-1);
            Assert.AreEqual("Marc von Gabain", group);
        }

        [Test]
        public void GetGroupFirst()
        {
            String group = (String)adapter.GetGroup(0);
            Assert.AreEqual("Marc von Gabain", group);
        }

        [Test]
        public void GetGroupSecond()
        {
            String group = (String)adapter.GetGroup(1);
            Assert.AreEqual("Fro" +
                "do Baggings", group);
        }

        [Test]
        public void GetGroupThird()
        {
            String group = (String)adapter.GetGroup(2);
            Assert.AreEqual("Harry Potter", group);
        }

        [Test]
        public void GetGroupBeyondLastReturnsLast()
        {
            String group = (String)adapter.GetGroup(3);
            Assert.AreEqual("Harry Potter", group);
        }

        [Test]
        public void GetGroupIdId()
        {
            long group = adapter.GetGroupId(0);
            Assert.AreEqual(0, group);
        }

        [Test]
        public void GetGroupIdNegative()
        {
            long group = adapter.GetGroupId(-5);
            Assert.AreEqual(0, group);
        }

        [Test]
        public void GetGroupIdHigh()
        {
            long child = adapter.GetChildId(52, 0);
            Assert.AreEqual(0, child);
        }

        [Test]
        public void GetGroupCount()
        {
            long child = adapter.GroupCount;
            Assert.AreEqual(3, child);
        }
        #endregion
        
        [Test]
        public void HasStableIdsIsTrue()
        {
            bool b = adapter.HasStableIds;
            Assert.AreEqual(true, b);
        }

        [Test]
        public void IsChildSelectableFalse()
        {
            bool b = adapter.IsChildSelectable(0,0);
            Assert.AreEqual(false, b);
        }

        [Test]
        public void IsChildSelectableFalseNegative()
        {
            bool b = adapter.IsChildSelectable(-5, -5);
            Assert.AreEqual(false, b);
        }

        [Test]
        public void IsChildSelectableFalseHigh()
        {
            bool b = adapter.IsChildSelectable(52, 52);
            Assert.AreEqual(false, b);
        }
    }
}