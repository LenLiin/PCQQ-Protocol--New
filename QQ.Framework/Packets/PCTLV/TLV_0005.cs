using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Uin)]
    internal class TLV_0005 : BaseTLV
    {
        public TLV_0005()
        {
            cmd = 0x0005;
            Name = "SSO2::TLV_Uin_0x5";
            wSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            if (User.QQ == 0)
            {
                return null;
            }

            var buf = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                buf.BEWrite(wSubVer);
                buf.BEWrite((uint) User.QQ);
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