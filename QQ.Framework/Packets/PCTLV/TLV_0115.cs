using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.PacketMd5)]
    internal class TLV_0115 : BaseTLV
    {
        public TLV_0115()
        {
            cmd = 0x0115;
            Name = "SSO2::TLV_PacketMd5_0x115";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            var bufPacketMD5 = buf.ReadBytes(_length);
        }
    }
}