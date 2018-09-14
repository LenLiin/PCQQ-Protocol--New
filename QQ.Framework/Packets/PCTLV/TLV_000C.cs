using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_000C : BaseTLV
    {
        public TLV_000C()
        {
            cmd = 0x000C;
            Name = "SSO2::TLV_PingRedirect_0xC";
            wSubVer = 0x0002;
        }

        public byte[] get_tlv_000C(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                data.BEWrite(wSubVer); //wSubVer
                data.BEWrite(0);
                data.BEWrite(QQGlobal.dwIDC);
                data.BEWrite(QQGlobal.dwISP);
                data.Write(Util.IPStringToByteArray(m_PCClient.dwServerIP));
                data.BEWrite(m_PCClient.wServerPort);
                data.BEWrite(0);
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

        public void parser_tlv_0C(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0002)
            {
                buf.BEReadUInt16();
                buf.BEReadInt32(); /*dwIDC =*/
                buf.BEReadInt32(); /*dwISP =*/
                buf.ReadBytes(4); /*dwRedirectIP =*/
                buf.BEReadUInt16(); /*wRedirectPort =*/
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}