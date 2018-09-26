using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QQ.Framework.Utils
{
    class HttpWebClient : WebClient
    {
        private WebResponse _webResponse;

        public int Timeout
        {
            get;
            set;
        }

        public WebResponse Response { get; set; }

        public CookieContainer Cookies { get; set; }

        public HttpWebClient():base()
        {
            ServicePointManager.DefaultConnectionLimit = 255;
            Cookies = new CookieContainer();
        }

        public HttpWebClient(CookieContainer cookies):base()
        {
            ServicePointManager.DefaultConnectionLimit = 255;
            Cookies = cookies;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            Response = base.GetWebResponse(request, result);
            return Response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);
            if (webRequest is HttpWebRequest)
            {
                HttpWebRequest obj = webRequest as HttpWebRequest;
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
