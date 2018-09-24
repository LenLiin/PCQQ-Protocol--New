using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.PacketMd5)]
    internal class TLV0115 : BaseTLV
    {
        public TLV0115()
        {
            Command = 0x0115;
            Name = "SSO2::TLV_PacketMd5_0x115";
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            var bufPacketMD5 = buf.ReadBytes(length);
        }
    }
}