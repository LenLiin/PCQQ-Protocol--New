using System;
using System.IO;
using System.Net;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.LocalIP)]
    internal class TLV002D : BaseTLV
    {
        public TLV002D()
        {
            Command = 0x002D;
            Name = "TLV_LocalIP";
            WSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer 
                data.Write(Util.IPStringToByteArray(GetLocalIP())); //本机内网IP地址
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }

            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        public static string GetLocalIP()
        {
            var localIP = "192.168.1.2";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception)
            {
                localIP = "192.168.1.2";
            }

            return localIP;
        }
    }
}