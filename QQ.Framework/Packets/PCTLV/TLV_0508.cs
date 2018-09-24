using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x0508)]
    internal class TLV_0508 : BaseTLV
    {
        public TLV_0508()
        {
            cmd = 0x0508;
            Name = "SSO2::TLV_0508";
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            data.Write((byte) 1);
            data.BEWrite(0);


            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16(); //type
            var _length = buf.BEReadUInt16(); //length
            buf.ReadBytes(_length);
        }
    }
}