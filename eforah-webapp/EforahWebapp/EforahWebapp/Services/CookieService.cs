using System;
using System.Web;

namespace EforahWebapp.Services
{
    /// <summary>
    /// Handelt het maken en gebruik van Cookies af.
    /// </summary>
    public class CookieService
    {
        #region private properties
        private readonly string cookieName = "EforahBeheerAppCookie";
        private HttpResponseBase response;
        private HttpRequestBase request;
        #endregion

        #region constructor        
        public CookieService(HttpResponseBase pResponse, HttpRequestBase pRequest)
        {
            response = pResponse;
            request = pRequest;
            if(pResponse != null)
            {
                response.Cookies[cookieName]["Cookie info"] = "EforahCookieGemaakt";
                response.Cookies[cookieName].Expires = DateTime.Now.AddDays(7d);
            }

        }
        #endregion

        #region get&setData
        /// <summary>
        /// Zet de gegeven data in de EforahBeheerAppCookie
        /// </summary>
        /// <param name="dataName">De naam de data moet hebben</param>
        /// <param name="data">De data die in de cookie gaat</param>
        public void SetData(string dataName, string data)
        {
            if (dataName == null || dataName.Length == 0)
            {
                throw new ArgumentNullException("dataName", "Argument is null or an empty string");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException("data", "Argument is null or an empty string");
            }

            response.Cookies[cookieName][dataName] = data;
        }


        /// <summary>
        /// Haal de gevraagde data uit de EforahBeheerAppCookie
        /// </summary>
        /// <param name="dataName">De Naam van de data</param>
        /// <returns>De data als string. Is null als deze niet bestaat</returns>
        public string GetData(string dataName)
        {
            if(dataName == null || dataName.Length == 0)
            {
                throw new ArgumentNullException("dataName", "Argument is null or an empty string");
            }

            var data = request.Cookies[cookieName][dataName];
            return data;
        }
        #endregion
    }
}