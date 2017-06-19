using Eforah_BetaalApp.Implementation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eforah_BetaalApp.Implementation
{
    public static class HttpRestService
    {
        private static string baseLink = "http://eforahapi.azurewebsites.net/api/";
        private static string loginLinkExtention = "isValidUser";
        private static string transactionRequestLinkExtention = "saldo/update";
        private static string mededelingenRequestLinkExtention = "mededelingen";
        private static string ledenRequestLinkExtention = "leden";
        private static string gebruikerRequestLinkExtention = "gebruiker";
        private static string saldoRequestLinkExtention = "saldo";
        private static string bestandenRequestLinkExtention = "bestanden";


        /// <summary>
        /// Sends a Post request to the RestAPI. The request sends the given username and the hashed given password. 
        /// Responds with Json containing boolean wich is true if the login was correct,
        /// together with clubs from that user and the user info.
        /// </summary>
        /// <param name="username">The given string from the username inputfield</param>
        /// <param name="password">The given string from the password inputfield</param>
        /// <returns>Return a Tuple with two variables, a GebruikerModel Object and a VerenigingModel List</returns>
        public static async Task<Tuple<GebruikerModel, List<VerenigingModel>>> LoginRequest(String username, String password, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });

                JObject resBodyAsJson = await sendPost(client, baseLink + loginLinkExtention, reqBody);
                if(resBodyAsJson == null)
                {
                    return null;
                }

                string valid = (string)resBodyAsJson["melding"];

                if (valid == "true")
                {

                    JsonSerializer serializer = new JsonSerializer();
                    GebruikerModel loggedInUser = (GebruikerModel)serializer.Deserialize(new JTokenReader(resBodyAsJson["loginInfo"]), typeof(GebruikerModel));

                    List<VerenigingModel> list = JArrayToList<VerenigingModel>("verenigingen", resBodyAsJson);

                    return new Tuple<GebruikerModel, List<VerenigingModel>>(loggedInUser, list);
                }
                return null;
            }
        }

        /// <summary>
        /// Sends a request to decrease the saldo of the scanned member.
        /// </summary>
        /// <param name="lidId"></param>
        /// <param name="verenigingId"></param>
        /// <param name="bedrag"></param>
        /// <returns>a string containing an error message if transaction failed, empty if succeeded</returns>
        /// 
        public static async Task<string> transactionRequest(string lidId, string verenigingId, string bedrag, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("lidId", lidId),
                    new KeyValuePair<string, string>("verenigingId", verenigingId),
                    new KeyValuePair<string, string>("bedrag", bedrag)
                });
                
                JObject resBodyAsJson = await sendPost(client, baseLink + transactionRequestLinkExtention, reqBody);
                if (resBodyAsJson == null)
                {
                    return null;
                }

                Debug.WriteLine(resBodyAsJson.ToString());
                return (string)resBodyAsJson["melding"];
            }
        }

        /// <summary>
        /// Sends a post request with the verenigingId in the body. Requests every "mededeling" from one "vereniging". 
        /// </summary>
        /// <param name="verenigingId"></param>
        /// <returns></returns>
        public static async Task<List<MededelingModel>> mededelingenRequest(string verenigingId, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("verenigingId", verenigingId)
                });

                JObject resBodyAsJson = await sendPost(client, baseLink + mededelingenRequestLinkExtention, reqBody);
                if (resBodyAsJson == null)
                {
                    return null;
                }

                List<MededelingModel> list = JArrayToList<MededelingModel>("mededelingen", resBodyAsJson);

                return list;
            }
        }

        /// <summary>
        /// Sends a post request to the API with the "verenigingId" in the body. Requests every "lid" from one "vereniging". 
        /// </summary>
        /// <param name="verenigingId"></param>
        /// <returns></returns>
        public static async Task<List<LidModel>> ledenRequest(string verenigingId, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("verenigingId", verenigingId)
                });

                JObject resBodyAsJson = await sendPost(client, baseLink + ledenRequestLinkExtention, reqBody);
                if (resBodyAsJson == null)
                {
                    return null;
                }

                List<LidModel> list = JArrayToList<LidModel>("leden", resBodyAsJson);

                return list;
            }
        }

        /// <summary>
        /// Sends a post request to the API with the "lidId" in the body. Requests "gebruiker" from assosiated with the "lid". 
        /// </summary>
        /// <param name="lidId"></param>
        /// <returns></returns>
        public static async Task<GebruikerModel> GebruikerRequest(int lidId, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("lidId", lidId.ToString())
                });

                JObject resBodyAsJson = await sendPost(client, baseLink + gebruikerRequestLinkExtention, reqBody);
                if (resBodyAsJson == null)
                {
                    return null;
                }

                GebruikerModel gebruiker = resBodyAsJson.ToObject<GebruikerModel>();

                return gebruiker;
            }
        }

        /// <summary>
        /// Sends a post request to the API with the "verenigingId" in the body. Requests the first five transactions and the current saldo. 
        /// </summary>
        /// <param name="lidId"></param>
        /// <param name="verenigingId"></param>
        /// <returns></returns>
        public static async Task<Tuple<string, List<TransactieModel>>> saldoRequest(string lidId, string verenigingId, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("verenigingId", verenigingId),
                    new KeyValuePair<string, string>("lidId", lidId)
                });

                JObject resBodyAsJson = await sendPost(client, baseLink + saldoRequestLinkExtention, reqBody);
                if (resBodyAsJson == null)
                {
                    return null;
                }

                List<TransactieModel> list = JArrayToList<TransactieModel>("5laatstetransacties", resBodyAsJson);

                string saldo = (string)resBodyAsJson["saldo"];
                return new Tuple<string, List<TransactieModel>>(saldo, list);
            }
        }

        /// <summary>
        /// Sends a post request to the API with the "verenigingId" in the body. Requests every "Bestand" from one "vereniging". 
        /// </summary>
        /// <param name="lidId"></param>
        /// <param name="verenigingId"></param>
        /// <returns></returns>
        public static async Task<List<BestandModel>> bestandenRequest(int verenigingId, HttpClient client = null)
        {
            if (client == null) client = new HttpClient();
            using (client)
            {
                HttpContent reqBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("verenigingId", verenigingId.ToString())
                });

                JObject resBodyAsJson = await sendPost(client, baseLink + bestandenRequestLinkExtention, reqBody);
                if (resBodyAsJson == null)
                {
                    return null;
                }

                List<BestandModel> list = JArrayToList<BestandModel>("bestanden", resBodyAsJson);

                return list;
            }
        }


        /// <summary>
        /// Sends a post request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="link"></param>
        /// <param name="reqBody"></param>
        /// <returns>returns the response body as JObject</returns>
        private static async Task<JObject> sendPost(HttpClient client, string link, HttpContent reqBody)
        {
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(link, reqBody);
            }
            catch
            {
                return null;
            }
            string resBodyAsString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(resBodyAsString);
        }

        /// <summary>
        /// Converts a JArray from an JObject to an generic List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectName"></param>
        /// <param name="body"></param>
        /// <returns>a List</returns>
        private static List<T> JArrayToList<T>(string objectName, JObject body)
        {
            if (body == null) return null;
            JArray JsonArray = (JArray)body[objectName];
            List<T> list = new List<T>();
            for (int i = 0; i < JsonArray.Count; i++)
            {
                list.Add(JsonArray[i].ToObject<T>());
            }
            return list;
        }
    }
}