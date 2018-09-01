using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0018 : BaseTLV
    {
        public TLV_0018()
        {
            this.cmd = 0x0018;
            this.Name = "SSO2::TLV_Ping_0x18";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_0018(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0001)
            {
                data.BEWrite(this.wSubVer); //wSubVer 
                data.BEWrite(QQGlobal.dwSSOVersion); //dwSSOVersion
                data.BEWrite(QQGlobal.dwServiceId); //dwServiceId
                data.BEWrite(QQGlobal.dwClientVer); //dwClientVer
                data.BEWrite((uint)m_PCClient.QQUser.QQ); //dwUin
                data.BEWrite((ushort)m_PCClient.wRedirectCount); //wRedirectCount 
                data.BEWrite(0); //NullBuf
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
    }
}
