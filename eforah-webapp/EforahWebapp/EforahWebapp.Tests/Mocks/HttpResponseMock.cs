using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EforahWebapp.Tests.Mocks
{
    class HttpResponseMock : HttpResponseBase
    {
        private HttpCookieCollection cookies;

        public override HttpCookieCollection Cookies{ get { return cookies; } }

        public HttpResponseMock(HttpCookieCollection pCookies)
        {
            cookies = pCookies;
        }
    }
}
