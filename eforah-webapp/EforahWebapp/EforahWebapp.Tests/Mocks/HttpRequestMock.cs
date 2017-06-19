using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EforahWebapp.Tests.Mocks
{
    class HttpRequestMock : HttpRequestBase
    {
        private HttpCookieCollection cookies;

        public override HttpCookieCollection Cookies { get { return cookies; } }

        public HttpRequestMock(HttpCookieCollection pCookies)
        {
            cookies = pCookies;
        }
    }
}
