using System;
using System.IO;
using System.Net;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.LocalIP)]
    internal class TLV_002D : BaseTLV
    {
        public TLV_002D()
        {
            cmd = 0x002D;
            Name = "TLV_LocalIP";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer 
                data.Write(Util.IPStringToByteArray(GetLocalIP())); //本机内网IP地址
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
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