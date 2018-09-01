using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0018 : BaseTLV
    {
        public TLV_0018()
        {
            cmd = 0x0018;
            Name = "SSO2::TLV_Ping_0x18";
            wSubVer = 0x0001;
        }

        public byte[] get_tlv_0018(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer); //wSubVer 
                data.BEWrite(QQGlobal.dwSSOVersion); //dwSSOVersion
                data.BEWrite(QQGlobal.dwServiceId); //dwServiceId
                data.BEWrite(QQGlobal.dwClientVer); //dwClientVer
                data.BEWrite((uint) m_PCClient.QQUser.QQ); //dwUin
                data.BEWrite(m_PCClient.wRedirectCount); //wRedirectCount 
                data.BEWrite(0); //NullBuf
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", Name, wSubVer));
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}