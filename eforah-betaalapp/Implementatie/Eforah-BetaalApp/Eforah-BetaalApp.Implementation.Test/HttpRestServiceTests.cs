using System;
using NUnit.Framework;
using Eforah_BetaalApp.Implementation.Services;
using Eforah_BetaalApp.Implementation.Test.Mocks;
using System.Threading.Tasks;
using System.Diagnostics;
using RichardSzalay.MockHttp;
using System.Net.Http;

namespace Eforah_BetaalApp.Implementation.Test
{
    [TestFixture]
    public class HttpRestServiceTests
    {
        //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        HttpClient mock;
        
        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://eforahapi.azurewebsites.net/api/isValidUser")
                    .Respond("application/json", "{'melding': 'true','loginInfo': {'gebruikerId': '1','voornaam': 'Koen','achternaam': 'van Helvoort'},'verenigingen': [{'verenigingId': 3,'lidId': 2,'rol': 'Lid','postcode': '7966 PO','huisnummer': 24,'adres': 'Polostraat','plaats': 'Eindhoven','naam': 'I Support','facebookAdminId': 'EAATkhRQXf2IBAAXjgzUKh5VM5sERUfWD8dCqKCeOT6cJqe5BU5yhOqjgXSWc9JgzX339Aedr70Oqzde40LlfvPZA8rQtTxOtR3LndIAXC5IMwCb11ELrpm1qK9sEZCGmgZCWr3jKLxz90JiuZBALKp2bwlpEWVwZD','agendaLink': '','telefoonummer': '0645672664','email': '','facebookGroupId': '1867120583538714'},{'verenigingId': 1,'lidId': 15,'rol': 'Lid','postcode': '5404 MK','huisnummer': 6,'adres': 'Morkolf','plaats': 'Uden','naam': 'Udi 19','facebookAdminId': '','agendaLink': 'https://calendar.google.com/calendar/embed?src=vhvtejkctjee7qq6794760rp7u6rivfc%40import.calendar.google.com&ctz=Europe/Amsterdam','telefoonummer': '0636545661','email': '','facebookGroupId': '61651684686464850'}]}");
            mockHttp.When("http://eforahapi.azurewebsites.net/api/saldo/update")
                    .Respond("application/json", "{'melding': 'test'}");
            mockHttp.When("http://eforahapi.azurewebsites.net/api/mededelingen")
                    .Respond("application/json", "{'mededelingen': [{'mededelingId': '4','verenigingId': '3','plaatsingDatum': '1/1/2016 10:13:43 AM','titel': 'Gelukkig nieuwjaar!!!','mededeling': 'Nu weer snel aan het werk allemaal. Kater of niet.'}]}");
            mockHttp.When("http://eforahapi.azurewebsites.net/api/leden")
                    .Respond("application/json", "{'leden': [{'voornaam': 'Koen','achternaam': 'van Helvoort','email': 'mail@dit.nl','telefoonnummer': '55555555555','postcode': '5404 MK','huisnummer': '6','adres': 'Morkolf','plaats': 'Uden','foto': '','telefoonnummerAlt': '66666666666'}]}");
            mockHttp.When("http://eforahapi.azurewebsites.net/api/saldo")
                    .Respond("application/json", "{'saldo': '89.9000','5laatstetransacties': [{'transactieId': '26','lidId': '2','transactieDatum': '6/8/2017 10:55:17 AM','bedrag': '10.0000'},{'transactieId': '25','lidId': '2','transactieDatum': '6/7/2017 9:52:57 AM','bedrag': '0.1000'},{'transactieId': '24','lidId': '2','transactieDatum': '6/7/2017 9:48:02 AM','bedrag': '0.1000'},{'transactieId': '23','lidId': '2','transactieDatum': '6/6/2017 4:29:10 PM','bedrag': '0.1000'},{'transactieId': '22','lidId': '2','transactieDatum': '6/6/2017 4:27:39 PM','bedrag': '0.1000'}]}");
            mockHttp.When("http://eforahapi.azurewebsites.net/api/bestanden")
                   .Respond("application/json", "{'bestanden': [{'bestandId': 1,'verenigingId': 1,'url': 'temp/url','docType': '.docx','docNaam': 'Speelschema','docGrootte': 176},{'bestandId': 3,'verenigingId': 1,'url': 'non/url','docType': '.sta','docNaam': 'Document','docGrootte': 357}]}");

            mock = new HttpClient(mockHttp);
        }

        [Test]
        public async Task LoginRequestMethod()
        {
            var response = await HttpRestService.LoginRequest("test", "test", mock);
            Assert.AreEqual(response.Item1.voornaam, "Koen");
        }
        [Test]
        public async Task transactionRequestMethod()
        {
            var response = await HttpRestService.transactionRequest("test", "test", "test", mock);
            Assert.AreEqual(response, "test");
        }
        [Test]
        public async Task mededelingenRequestMethod()
        {
            var response = await HttpRestService.mededelingenRequest("0", mock);
            Assert.AreEqual(response[0].titel, "Gelukkig nieuwjaar!!!");
        }
        [Test]
        public async Task ledenRequestMethod()
        {
            var response = await HttpRestService.ledenRequest("0", mock);
            Assert.AreEqual(response[0].voornaam, "Koen");
        }
        [Test]
        public async Task saldoRequestMethod()
        {
            var response = await HttpRestService.saldoRequest("0", "0", mock);
            Assert.AreEqual(response.Item1, "89.9000");
        }
        [Test]
        public async Task bestandenRequestMethod()
        {
            var response = await HttpRestService.bestandenRequest(0, mock);
            Assert.AreEqual(response[0].docNaam, "Speelschema");
        }
    }
}
