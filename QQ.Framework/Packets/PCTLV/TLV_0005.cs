using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0005 : BaseTLV
    {
        public TLV_0005()
        {
            cmd = 0x0005;
            Name = "SSO2::TLV_Uin_0x5";
            wSubVer = 0x0002;
        }

        public byte[] get_tlv_0005(QQClient m_PCClient)
        {
            if (m_PCClient.QQUser.QQ == 0)
            {
                return null;
            }

            var buf = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                buf.BEWrite(wSubVer);
                buf.BEWrite((uint) m_PCClient.QQUser.QQ);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            fill_head(cmd);
            fill_body(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}