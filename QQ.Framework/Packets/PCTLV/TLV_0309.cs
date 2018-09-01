using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0309 : BaseTLV
    {
        public TLV_0309()
        {
            this.cmd = 0x0309;
            this.Name = "SSO2::TLV_Ping_Strategy_0x309";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_0309(QQClient m_PCClient)
        {
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer
                data.Write(Util.IPStringToByteArray(m_PCClient.dwServerIP)); //LastServerIP - 服务器最后的登录IP，可以为0
                data.Write((byte)m_PCClient.QQUser.RedirectIP.Count);//cRedirectCount - 重定向的次数（IP的数量）
                foreach (var ip in m_PCClient.QQUser.RedirectIP)
                {
                    data.Write(ip);
                }
                data.Write(m_PCClient.QQUser.cPingType);//cPingType 
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


        public byte GetPingType(int val)
        {
            switch (val)
            {
                case 10://0xA
                case 20://0x14
                    return 1;
                case 30://0x1E
                    return 2;
                case 40://0x28
                    return 3;
                case 50://0x32
                case 60://0x3C
                    return 4;
                case 70://0x46
                    return 6;
                case 25://0x19
                    return 7;
                default:
                    return 4;
            }
        }
    }
}
