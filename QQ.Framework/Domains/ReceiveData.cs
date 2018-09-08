using System.Net;

namespace QQ.Framework.Domains
{
    public class ReceiveData
    {
        public byte[] Data;
        public int DataLength;
        public EndPoint From;
    }
}