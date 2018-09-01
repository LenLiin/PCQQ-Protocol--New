using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.Net;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_002D : BaseTLV
    {
        public TLV_002D()
        {
            this.cmd = 0x002D;
            this.Name = "TLV_LocalIP";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_002D(QQClient m_PCClient)
        {
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer 
                data.Write(Util.IPStringToByteArray(TLV_002D.GetLocalIP())); //本机内网IP地址
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
            fill_head(this.cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public static string GetLocalIP()
        {
            string localIP = "192.168.1.2";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
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
