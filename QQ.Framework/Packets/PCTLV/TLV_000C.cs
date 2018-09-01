using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_000C : BaseTLV
    {
        public TLV_000C()
        {
            this.cmd = 0x000C;
            this.Name = "SSO2::TLV_PingRedirect_0xC";
            this.wSubVer = 0x0002;
        }

        public byte[] get_tlv_000C(QQClient m_PCClient)
        {
              var data = new BinaryWriter(new MemoryStream());
            if (this.wSubVer == 0x0002)
            {
                data.BEWrite(this.wSubVer); //wSubVer
                data.BEWrite(0);
                data.BEWrite(QQGlobal.dwIDC);
                data.BEWrite(QQGlobal.dwISP);
                data.Write(Util.IPStringToByteArray(m_PCClient.dwServerIP));
                data.BEWrite(m_PCClient.wServerPort);
                data.BEWrite(0);
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

        public void parser_tlv_0C(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0002)
            {
                buf.BEReadUInt16();
                buf.BEReadInt32(); /*dwIDC =*/
                buf.BEReadInt32(); /*dwISP =*/
                buf.ReadBytes(4); /*dwRedirectIP =*/
                buf.BEReadUInt16(); /*wRedirectPort =*/
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
