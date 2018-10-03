using System;
using System.Net;

namespace QQ.Framework.Utils
{
    internal class HttpWebClient : WebClient
    {
        private WebResponse _webResponse;

        public HttpWebClient()
        {
            ServicePointManager.DefaultConnectionLimit = 255;
            Cookies = new CookieContainer();
        }

        public HttpWebClient(CookieContainer cookies)
        {
            ServicePointManager.DefaultConnectionLimit = 255;
            Cookies = cookies;
        }

        public int Timeout { get; set; }

        public WebResponse Response { get; set; }

        public CookieContainer Cookies { get; set; }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            Response = base.GetWebResponse(request, result);
            return Response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = base.GetWebRequest(address);
            if (webRequest is HttpWebRequest)
            {
                var obj = webRequest as HttpWebRequest;
                obj.ServicePoint.Expect100Continue = false;
                obj.CookieContainer = Cookies;
                if (Timeout == 0)
                {
                    Timeout = 30000;
                }

                obj.Timeout = Timeout;
            }

            return webRequest;
        }
    }
}