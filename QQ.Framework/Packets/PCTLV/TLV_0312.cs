using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Misc_Flag)]
    internal class TLV_0312 : BaseTLV
    {
        public TLV_0312()
        {
            cmd = 0x0312;
            Name = "SSO2::TLV_Misc_Flag_0x312";
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write((byte)1);
            data.BEWrite(1);
            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}